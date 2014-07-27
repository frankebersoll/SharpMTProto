using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMTProto.Schema.MTProto;

namespace SharpMTProto.Messaging
{
    public interface IMessageDispatcher 
    {
        void Receive(IMessage logicalMessage);

        Task<T> Register<T>(ulong messageId, TimeSpan timeout, Action reSendAction);

        void ReRegister(ulong newMessageId, ulong oldMessageId);

        void Receive(Schema.MTProto.IMessage mtProtoMessage);
    }
}