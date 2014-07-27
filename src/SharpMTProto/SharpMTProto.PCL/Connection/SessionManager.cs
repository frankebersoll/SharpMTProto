using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Connection
{
    public class SessionManager
    {
        private static readonly Random Rnd = new Random();

        public ulong SessionId { get; private set; }

        public SessionManager()
        {
            this.SessionId = GetNextSessionId();
        }

        private static ulong GetNextSessionId()
        {
            return ((ulong) Rnd.Next()) << 32 + Rnd.Next();
        }
    }
}