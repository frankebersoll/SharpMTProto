using System;
using SharpMTProto.Transport;

namespace SharpMTProto.Extra.WinRT
{
    public class TransportFactory : ITransportFactory
    {
        /// <summary>
        ///     Creates a new TCP transport.
        /// </summary>
        /// <param name="transportConfig">Transport info.</param>
        /// <returns>TCP transport.</returns>
        public ITransport CreateTransport(TransportConfig transportConfig)
        {
            // TCP.
            var tcpTransportConfig = transportConfig as TcpTransportConfig;
            if (tcpTransportConfig != null)
            {
                return new RTTcpTransport(tcpTransportConfig);
            }

            throw new NotSupportedException(string.Format("Transport type '{0}' is not supported.", transportConfig.TransportName));
        }
    }
}