// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlainMessage.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Catel;
using SharpMTProto.Properties;
using SharpTL;

namespace SharpMTProto.Messaging
{
    /// <summary>
    ///     Plain (unencrypted) message.
    /// </summary>
    public class PlainMessage : IMessage
    {
        /// <summary>
        ///     Message header length in bytes.
        /// </summary>
        public const int HeaderLength = 20;

        private readonly int _dataLength;
        private readonly int _length;
        private readonly byte[] _messageBytes;
        private readonly byte[] _messageData;
        private readonly ulong _messageId;

        public PlainMessage(ulong messageId, [NotNull] byte[] messageData)
        {
            Argument.IsNotNull(() => messageData);

            int dataLength = messageData.Length;
            this._messageId = messageId;
            this._messageData = messageData;
            this._dataLength = dataLength;
            this._length = HeaderLength + dataLength;
            this._messageBytes = new byte[this._length];

            using (var streamer = new TLStreamer(this._messageBytes))
            {
                // Writing header.
                streamer.WriteInt64(0); // Plain unencrypted message must always have zero auth key id.
                streamer.WriteUInt64(this._messageId);
                streamer.WriteInt32(this._dataLength);

                // Writing data.
                streamer.Write(messageData, 0, this._dataLength);
            }
        }

        public ulong MessageId
        {
            get { return this._messageId; }
        }

        public int DataLength
        {
            get { return this._dataLength; }
        }

        public int Length
        {
            get { return this._length; }
        }

        public byte[] MessageBytes
        {
            get { return this._messageBytes; }
        }

        public byte[] MessageData
        {
            get { return this._messageData; }
        }
    }
}
