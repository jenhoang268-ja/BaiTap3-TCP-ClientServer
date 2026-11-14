using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class ClientInfo
    {
        public Guid ClientId { get; set; }

        public string Username { get; set; } = "";

        public string Address { get; set; } = "";

        public DateTime ConnectedAt { get; set; }
    }
}
