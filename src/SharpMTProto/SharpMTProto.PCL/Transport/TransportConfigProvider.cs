using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Transport
{
    public class TransportConfigProvider : ITransportConfigProvider
    {
        public TransportConfig DefaultTransportConfig { get; set; }
    }
}