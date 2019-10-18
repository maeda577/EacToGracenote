using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace GracenoteConnector.Library
{
    /// <summary>
    /// CDDBWebサービス 実装クラス
    /// </summary>
    public class CddbService : ICddbService
    {
        /// <summary>
        /// Gracenoteアクセスのクライアント
        /// </summary>
        private readonly GracenoteClient gracenoteClient;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CddbService()
        {
            // Gracenote アプリケーション固有のクライアントID
            string clientId = ConfigurationManager.AppSettings.Get("clientId");

            this.gracenoteClient = new GracenoteClient(clientId);
        }

        /// <summary>
        /// CDDBサービスのエンドポイント
        /// </summary>
        /// <param name="cmd">CDDBコマンド</param>
        /// <returns></returns>
        public async Task<Message> Cddb(string cmd)
        {
            WebOperationContext currentContext = WebOperationContext.Current;

            // コマンドなし
            if (string.IsNullOrWhiteSpace(cmd))
            {
                return CreateMessage(currentContext, "500 Empty command input.");
            }

            string[] cmdArray = cmd.Split(' ');

            // コマンド不正
            if (cmdArray.Length < 2 || cmdArray[0].Equals("cddb") == false)
            {
                return CreateMessage(currentContext, "500 Unrecognized command.");
            }

            string resultString = null;

            // コマンドタイプで分岐
            try
            {
                switch (cmdArray[1])
                {
                    case "query":
                        resultString = await this.Query(cmdArray);
                        break;
                    case "read":
                        resultString = await this.Read(cmdArray);
                        break;
                    default:
                        resultString = "500 Unrecognized command.";
                        break;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.StackTrace);

                resultString = "402 Server Error. " + e.Message;
            }

            return CreateMessage(currentContext, resultString);
        }

        /// <summary>
        /// CDDB クエリコマンドの実行
        /// </summary>
        /// <param name="cmdArray"></param>
        /// <returns></returns>
        private async Task<string> Query(string[] cmdArray)
        {
            QueryCommand command = null;
            if (QueryCommand.TryCreate(cmdArray, out command) == false)
            {
                return "500 Command syntax error.";
            }

            Album[] albums = await this.gracenoteClient.GetAlbumInfo(command.Toc);

            return CddbUtil.CreateQueryResponse(command, albums);
        }

        /// <summary>
        /// CDDB リードコマンドの実行
        /// </summary>
        /// <param name="cmdArray"></param>
        /// <returns></returns>
        private async Task<string> Read(string[] cmdArray)
        {
            ReadCommand command = null;

            if (ReadCommand.TryCreate(cmdArray, out command) == false)
            {
                return "500 Command syntax error.";
            }

            Album[] albums = await this.gracenoteClient.GetAlbumInfo(command.DiscId);

            return CddbUtil.CreateReadResponse(command, albums);
        }

        private static Message CreateMessage(WebOperationContext context, string text)
        {
            return context.CreateTextResponse(text, "text/plain", new UTF8Encoding(false));
        }
    }
}
