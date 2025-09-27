using System;

namespace Celeste.Web.Messages
{
    [Serializable]
    public struct NetworkingMessageId
    {
        public bool IsValid => Id != 0;
        
        public int Id;
    }
    
    [Serializable]
    public struct NetworkingMessage
    {
        public static NetworkingMessage Invalid => new();
        
        public bool IsValid => Id != 0;
        
        public int Id;
        public object Payload;

        public bool Is<T>()
        {
            return Payload is T;
        }

        public NetworkingMessage<T> As<T>()
        {
            return new NetworkingMessage<T> { Id = Id, Payload = (T)Payload };
        }
    }
    
    [Serializable]
    public struct NetworkingMessage<T>
    {
        public static NetworkingMessage<T> Invalid => new();
        
        public bool IsValid => Id != 0;
        
        public int Id;
        public T Payload;

        public NetworkingMessage Typeless()
        {
            return  new NetworkingMessage { Id = Id, Payload = Payload };
        }
    }
}