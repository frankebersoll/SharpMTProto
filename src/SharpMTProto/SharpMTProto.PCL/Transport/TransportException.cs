using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Transport
{
    public class TransportException : MTProtoException
    {
        public TransportException()
        {
        }

        public TransportException(string message) : base(message)
        {
        }

        public TransportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}