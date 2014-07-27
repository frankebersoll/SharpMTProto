using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMTProto.Messaging;

namespace SharpMTProto.Connection
{
    public interface IMessageSender
    {
        TimeSpan DefaultRpcTimeout { get; set; }

        Task<T> SendMessage<T>(object request, TimeSpan timeout, MessageType messageType);
    }
}