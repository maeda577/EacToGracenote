using System;
using System.Collections.Generic;
using System.Text;

namespace GracenoteConnector.Library
{
    class CddbUtil
    {
        private CddbUtil()
        {
        }

        /// <summary>
        /// クエリコマンドの応答文字列を作成する
        /// </summary>
        /// <param name="query"></param>
        /// <param name="albums"></param>
        /// <returns></returns>
        /// <remarks>
        /// ヒット数1件の場合の処理を作るべきだが、
        /// そうすると後続のReadコマンドにDiscID(GN-IDじゃない)が渡ってくるため処理が面倒
        /// </remarks>
        public static string CreateQueryResponse(QueryCommand query, IList<Album> albums)
        {
            if (albums == null || albums.Count == 0)
            {
                return string.Format("202 No match for disc ID {0}.", query.DiscId);
            }
            else
            {
                StringBuilder result = new StringBuilder();

                result.AppendLine("211 close matches found");

                foreach (Album album in albums)
                {
                    result.AppendLine("Misc " + album.GN_ID + " " + album.ARTIST + " / " + album.TITLE);
                }

                result.AppendLine(".");

                return result.ToString();
            }
        }

        /// <summary>
        /// リードコマンドの応答文字列を作成する
        /// </summary>
        /// <param name="command"></param>
        /// <param name="album"></param>
        /// <returns></returns>
        public static string CreateReadResponse(ReadCommand command, Album[] albums)
        {
            if (albums == null || albums.Length == 0)
            {
                return "401 " + command.Category + " " + command.DiscId + " No such CD entry in database." + Environment.NewLine;
            }

            Album album = albums[0];

            StringBuilder result = new StringBuilder();

            result.AppendLine("210 Misc " + album.GN_ID + " CD database entry follows (until terminating `.')");
            result.AppendLine("DISCID=" + album.GN_ID);
            result.AppendLine("DTITLE=" + album.ARTIST + " / " + album.TITLE);
            result.AppendLine("DYEAR=" + album.DATE);
            result.AppendLine("DGENRE=" + album.GENRE.Value);

            for (int i = 0; i < album.TRACK.Length; i++)
            {
                string title = album.TRACK[i].TITLE;
                if (string.IsNullOrWhiteSpace(album.TRACK[i].ARTIST) == false)
                {
                    title = album.TRACK[i].ARTIST + " / " + title;
                }

                result.AppendLine("TTITLE" + i + "=" + title);
            }

            result.AppendLine("EXTD=");

            for (int i = 0; i < album.TRACK.Length; i++)
            {
                result.AppendLine("EXTT" + i + "=");
            }

            result.AppendLine("PLAYORDER=");
            result.AppendLine(".");

            return result.ToString();
        }
    }
}
