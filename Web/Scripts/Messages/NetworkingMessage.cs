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
    public struct NetworkingMessage<T>
    {
        public static NetworkingMessage<T> Invalid => new();
        
        public bool IsValid => Id != 0;
        
        public int Id;
        public T Payload;
    }
}