using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Transport;

namespace SharpMTProto.Connection
{
    public interface IMTProtoConnectionFactory
    {
        TimeSpan DefaultRpcTimeout { get; set; }
        TimeSpan DefaultConnectTimeout { get; set; }
        IMTProtoConnection Create();
        IMTProtoConnection Create(TransportConfig transportConfig);
    }
}