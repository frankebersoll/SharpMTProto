using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Services
{
    public interface INonceGenerator
    {
        byte[] GetNonce(uint length);
    }
}