using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMTProto.Messaging;
using SharpTL;

namespace SharpMTProto.Connection
{
    public class ConnectionAdapter : ITLConnectionAdapter
    {
        private readonly IMTProtoConnection _connection;
        private readonly MessageType _messageType;

        public ConnectionAdapter(IMTProtoConnection connection, MessageType messageType)
        {
            this._connection = connection;
            this._messageType = messageType;
        }

        public Task<T> Send<T>(object args)
        {
            return this._connection.Send<T>(args, this._connection.DefaultRpcTimeout, this._messageType);
        }
    }
}