using System;

namespace Celeste.Web.Messages
{
    [Serializable]
    public struct TestConnectionPayload
    {
        public const int Id = -1;
        
        public string Message;

        public static NetworkingMessage<TestConnectionPayload> Create(string message)
        {
            return new NetworkingMessage<TestConnectionPayload>
            {
                Id = Id, 
                Payload = new TestConnectionPayload
                {
                    Message = message
                }
            };
        }
    }
}