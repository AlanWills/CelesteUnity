using System;
using System.Collections.Generic;

namespace Celeste.Web
{
    public interface INetworkingClient : INetworkingObject
    {
        ulong Id { get; }
        bool HasNetworkObject { get; }
        IReadOnlyList<INetworkMessageHandler> NetworkMessageHandlers { get; }

        void PingServer(string message);
        void Ping(string message);

        void RequestDisconnectFromServer();
        void FinaliseDisconnectFromServer();

        T GetNetworkMessageHandler<T>() where T : INetworkMessageHandler;

        void AddOnWillDisconnectCallback(Action<ulong> callback);
        void RemoveOnWillDisconnectCallback(Action<ulong> callback);
    }
}