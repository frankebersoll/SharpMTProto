using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Client;

namespace SharpMTProto.Connection
{
    public interface IMessageReaderFactory
    {
        IMessageReader CreateReader();

        void Initialize(DefaultContainer container);
    }
}