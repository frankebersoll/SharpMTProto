using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Connection
{
    public interface IMessageReader {
        /// <summary>
        /// Processes incoming message bytes.
        /// </summary>
        /// <param name="messageBytes">Incoming bytes.</param>
        void ProcessIncomingMessageBytes(byte[] messageBytes);
    }
}