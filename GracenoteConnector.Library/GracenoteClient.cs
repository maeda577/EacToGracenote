using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GracenoteConnector.Library
{
    /// <summary>
    /// Gracenoteへのアクセスを行うクラス
    /// </summary>
    public class GracenoteClient
    {
        /// <summary>
        /// GracenoteエンドポイントのUri
        /// </summary>
        private const string UriBase = "https://c{0}.web.cddbp.net/webapi/xml/1.0/";

        /// <summary>
        /// GracenoteのクライアントID
        /// </summary>
        private readonly string clientId;

        /// <summary>
        /// Gracenoteのエンドポイント
        /// </summary>
        private readonly Uri endPoint;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="clientId">アプリケーションのクライアントID</param>
        public GracenoteClient(string clientId)
        {
            this.clientId = clientId;

            string uri = string.Format(UriBase, clientId.Split('-').FirstOrDefault());

            this.endPoint = new Uri(string.Format(UriBase, clientId), UriKind.Absolute);
        }

        /// <summary>
        /// 新しいユーザーIDを取得する
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNewUserId()
        {
            Queries<RegisterQuery> queries = new Queries<RegisterQuery>()
            {
                QUERY = new RegisterQuery() { CLIENT = this.clientId },
            };

            RegisterResponse response = await PostToGracenote<RegisterResponse>(queries.ToXmlString(), this.endPoint);

            return response.USER;
        }

        /// <summary>
        /// アルバム情報を取得する
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="toc">TOC情報</param>
        /// <returns></returns>
        public async Task<Album[]> GetAlbumInfo(int[] toc, string userId)
        {
            Queries<AlbumTocQuery> queries = new Queries<AlbumTocQuery>()
            {
                AUTH = new Auth { CLIENT = this.clientId, USER = userId },
                QUERY = new AlbumTocQuery(toc),
            };

            AlbumTocResponse response = await PostToGracenote<AlbumTocResponse>(queries.ToXmlString(), this.endPoint);

            return response.ALBUM;
        }

        /// <summary>
        /// アルバム情報を取得する
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="gracenoteId">アルバムのGNID</param>
        /// <returns></returns>
        public async Task<Album[]> GetAlbumInfo(string gracenoteId, string userId)
        {
            Queries<AlbumFetchQuery> queries = new Queries<AlbumFetchQuery>()
            {
                AUTH = new Auth { CLIENT = this.clientId, USER = userId },
                QUERY = new AlbumFetchQuery(gracenoteId),
            };

            AlbumTocResponse response = await PostToGracenote<AlbumTocResponse>(queries.ToXmlString(), this.endPoint);

            return response.ALBUM;
        }

        /// <summary>
        /// 指定のURLにPOSTし、帰ってきたXMLをデシリアライズする
        /// </summary>
        /// <typeparam name="T">デシリアライズの型</typeparam>
        /// <param name="postContent">POSTする文字列</param>
        /// <param name="uri">URL</param>
        /// <returns></returns>
        private static async Task<T> PostToGracenote<T>(string postContent, Uri uri) where T : Responce
        {
            using (HttpClient client = new HttpClient())
            using (HttpContent content = new StringContent(postContent))
            using (HttpResponseMessage responseMessage = await client.PostAsync(uri, content))
            using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                XmlSerializer responseSerializer = new XmlSerializer(typeof(Responses<T>));

                Responses<T> responses = responseSerializer.Deserialize(responseStream) as Responses<T>;

                if (responses == null)
                {
                    throw new InvalidDataException("Gracenote Web API returns unexpected data.");
                }
                else if (responses.RESPONSE == null)
                {
                    throw new InvalidDataException("Gracenote Web API returns no data.");
                }
                else if (string.IsNullOrWhiteSpace(responses.MESSAGE) == false)
                {
                    throw new GracenoteException(responses.MESSAGE);
                }

                return responses.RESPONSE;
            }
        }
    }
}
