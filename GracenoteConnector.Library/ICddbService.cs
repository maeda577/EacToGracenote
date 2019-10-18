using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace GracenoteConnector.Library
{
    /// <summary>
    /// CDDBWebサービス
    /// </summary>
    [ServiceContract]
    public interface ICddbService
    {
        /// <summary>
        /// CDDBサービスのエンドポイント
        /// </summary>
        /// <param name="cmd">CDDBコマンド</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet]
        Task<Message> Cddb(string cmd);
    }
}
