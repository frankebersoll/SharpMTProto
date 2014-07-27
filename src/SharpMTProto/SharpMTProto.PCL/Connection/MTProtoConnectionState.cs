using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Connection
{
    public enum MTProtoConnectionState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2
    }
}