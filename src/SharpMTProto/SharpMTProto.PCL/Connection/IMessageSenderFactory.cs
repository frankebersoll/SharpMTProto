using System;
using System.Collections.Generic;
using System.Linq;
using SharpMTProto.Client;

namespace SharpMTProto.Connection
{
    public interface IMessageSenderFactory
    {
        IMessageSender CreateSender(MTProtoConnection mtProtoConnection);

        void Initialize(DefaultContainer container);
    }
}