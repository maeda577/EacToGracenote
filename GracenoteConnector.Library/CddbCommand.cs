using System.Collections.Generic;
using System.Linq;

namespace GracenoteConnector.Library
{
    /// <summary>
    /// FreeDBのTOC参照コマンド
    /// </summary>
    public class QueryCommand
    {
        /// <summary>
        /// TOC情報(ReadOutを含む)
        /// </summary>
        public int[] Toc { get; internal set; }

        /// <summary>
        /// ディスクID
        /// </summary>
        public string DiscId { get; internal set; }

        /// <summary>
        /// 総トラック数
        /// </summary>
        public int TrackCount { get; internal set; }

        /// <summary>
        /// 総秒数
        /// </summary>
        public int TotalSec { get; internal set; }

        /// <summary>
        /// 文字列をパースする
        /// </summary>
        /// <param name="cmdArray"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>
        /// 仕様書の該当箇所
        /// 
        /// Client command:
        /// -> cddb query discid ntrks off1 off2 ... nsecs
        ///
        /// discid:
        ///     CD disc ID number.  Example: f50a3b13
        /// ntrks:
        ///     Total number of tracks on CD.
        /// off1, off2, ...:
        ///	    Frame offset of the starting location of each track.
        /// nsecs:
        ///	    Total playing length of CD in seconds.
        /// </remarks>
        public static bool TryCreate(string[] cmdArray, out QueryCommand command)
        {
            command = null;

            // 足りない
            if (cmdArray.Length < 6)
            {
                return false;
            }

            // 最初の3つ以降は必ず数値のためParseする
            List<int> nums = new List<int>();

            foreach (string item in cmdArray.Skip(3))
            {
                int num = 0;

                // 一つでもParse失敗したら全体も失敗とする
                if (int.TryParse(item, out num) == false)
                {
                    return false;
                }

                nums.Add(num);
            }

            // TOCを作成。最初の1つはトラック数
            int[] toc = nums.Skip(1).ToArray();
            // 最後は総秒数なので、75倍してフレーム数にしReadOutとする
            toc[toc.Length - 1] = toc[toc.Length - 1] * 75;

            // 最初のTOCが0の場合、プリギャップ分をずらす
            if (toc[0] == 0)
            {
                for (int i = 0; i < toc.Length; i++)
                {
                    toc[i] += 150;
                }
            }

            command = new QueryCommand()
            {
                DiscId = cmdArray[2],
                TrackCount = nums[0],
                TotalSec = nums.Last(),
                Toc = toc,
            };

            return true;
        }
    }

    /// <summary>
    /// FreeDBの読み込みコマンド
    /// </summary>
    public class ReadCommand
    {
        /// <summary>
        /// ディスクID
        /// </summary>
        public string DiscId { get; internal set; }

        /// <summary>
        /// カテゴリ
        /// </summary>
        public string Category { get; internal set; }

        /// <summary>
        /// 文字列をパースする
        /// </summary>
        /// <param name="cmdArray"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>
        /// 仕様書の該当箇所
        /// 
        /// Client command:
        ///  -> cddb read categ discid
        /// categ:
        /// 	CD category.  Example: rock
        /// discid:
        /// 	CD disc ID number.  Example: f50a3b13
        /// </remarks>
        public static bool TryCreate(string[] cmdArray, out ReadCommand command)
        {
            command = null;

            // 足りない
            if (cmdArray.Length < 4)
            {
                return false;
            }

            command = new ReadCommand
            {
                Category = cmdArray[2],
                DiscId = cmdArray[3],
            };

            return true;
        }
    }
}
