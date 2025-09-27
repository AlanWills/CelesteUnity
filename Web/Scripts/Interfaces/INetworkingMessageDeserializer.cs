using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingMessageDeserializer
    {
        NetworkingMessage Deserialize(string message);
    }
}