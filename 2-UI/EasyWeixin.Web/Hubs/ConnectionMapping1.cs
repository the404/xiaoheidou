using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyWeixin.Web.Hubs
{
    /// <summary>
    /// 封装了将signalr连接存储在内存中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConnectionMapping1<T>
    {
        private readonly Dictionary<T, List<LinkBag>> _connections =
            new Dictionary<T, List<LinkBag>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, LinkBag linkBag)
        {
            lock (_connections)
            {
                List<LinkBag> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new List<LinkBag>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(linkBag);
                }
            }
        }

        public IEnumerable<LinkBag> GetConnections(T key)
        {
            List<LinkBag> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<LinkBag>();
        }

        public void Remove(T key, LinkBag linkBag)
        {
            lock (_connections)
            {
                List<LinkBag> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(linkBag);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }

    public class LinkBag : IEquatable<LinkBag>
    {
        public string ConnectionId { get; set; }

        public bool IsMobile { get; set; }

        public bool Equals(LinkBag other)
        {
            return other.ConnectionId == ConnectionId && other.IsMobile == IsMobile;
        }
    }
}