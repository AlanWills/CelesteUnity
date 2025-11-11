using System;
using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class DisabledNetworkingClient : INetworkingClient
    {
        public ulong Id => ulong.MaxValue;
        public bool Exists => false;
        public bool HasNetworkObject => false;
        public IReadOnlyList<INetworkMessageHandler> NetworkMessageHandlers { get; } = Array.Empty<INetworkMessageHandler>();

        public void PingServer(string message)
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Discarding ping to Server: {message}.", CelesteLog.Web);
        }

        public void Ping(string message)
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Discarding ping: {message}.", CelesteLog.Web);
        }

        public void RequestDisconnectFromServer()
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Discarding disconnect request.", CelesteLog.Web);
        }

        public void FinaliseDisconnectFromServer()
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Discarding finalise of disconnect.", CelesteLog.Web);
        }

        public void Disconnect()
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Ignoring {nameof(Disconnect)}.", CelesteLog.Web);
        }

        public T GetNetworkMessageHandler<T>() where T : INetworkMessageHandler
        {
            return default;
        }

        public void AddOnWillDisconnectCallback(Action<ulong> callback)
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Ignoring {nameof(AddOnWillDisconnectCallback)}.", CelesteLog.Web);
        }

        public void RemoveOnWillDisconnectCallback(Action<ulong> callback)
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Ignoring {nameof(RemoveOnWillDisconnectCallback)}.", CelesteLog.Web);
        }
    }
}