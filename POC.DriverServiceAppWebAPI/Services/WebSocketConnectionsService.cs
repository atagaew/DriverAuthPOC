using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace POC.DriverServiceAppWebAPI.Services
{
    public class WebSocketConnectionsService
    {
        private readonly ConcurrentDictionary<string, WebSocket> connections = new();

        public bool AddConnection(string clientId, WebSocket conn)
        {
            return connections.TryAdd(clientId, conn);
        }

        public WebSocket GetConnection(string clientId)
        {
            if (connections.TryGetValue(clientId, out WebSocket conn))
            {
                return conn;
            }

            return null;
        }

        public WebSocket RemoveConnection(string clientId)
        {
            if (connections.TryRemove(clientId, out WebSocket conn))
            {
                return conn;
            }

            return null;
        }

    }
}
