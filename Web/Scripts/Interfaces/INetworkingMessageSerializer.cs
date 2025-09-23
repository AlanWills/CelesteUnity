using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingMessageSerializer
    {
        string Serialize<T>(NetworkingMessage<T> networkingMessage);
    }
}