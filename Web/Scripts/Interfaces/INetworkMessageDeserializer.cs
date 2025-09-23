using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkMessageDeserializer
    {
        NetworkingMessage<T> Deserialize<T>(string message);
    }
}