using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Client
{
    public class MTProtoAppConfiguration
    {
        public string ServerIpAddress { get; set; }

        public int ServerPort { get; set; }

        public int ApiId { get; set; }

        public string ServerCert { get; set; }

        public string ApiHash { get; set; }
    }
}