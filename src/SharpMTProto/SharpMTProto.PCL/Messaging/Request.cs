using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMTProto.Connection;
using SharpMTProto.Schema.MTProto;
using SharpMTProto.Services;
using SharpTL;

namespace SharpMTProto.Messaging
{
    public class Request<TResponse> : IRequest
    {
        private readonly TaskCompletionSource<TResponse> _completionSource;
        private readonly IGZipService _gzipService;
        private readonly Action _sendAction;

        public Request(TaskCompletionSource<TResponse> completionSource, IGZipService gzipService, Action sendAction)
        {
            this._completionSource = completionSource;
            this._gzipService = gzipService;
            this._sendAction = sendAction;
        }

        public void Receive(object o)
        {
             this.ReceiveContent((dynamic) o);
        }

        private void ReceiveContent(object o)
        {
            if (typeof(TResponse) == typeof(bool) && Equals(o, false))
            {
                this._completionSource.TrySetException(new MTProtoException("RPC Error."));
                return;
            }

            this._completionSource.TrySetResult((TResponse)o);  
        }

        private void ReceiveContent(bool success)
        {
            if (!success)
            {
                this._completionSource.TrySetException(new MTProtoException("RPC Error."));
            }
            else
            {
                this._completionSource.TrySetResult(default(TResponse));
            }
        }

        private void ReceiveContent(GzipPacked o)
        {
            var unpacked = this._gzipService.Unpack(o.PackedData);
            var content = TLRig.Default.Deserialize(unpacked);

            this._completionSource.TrySetResult((TResponse)content);  
        }

        private void ReceiveContent(IRpcError error)
        {
            this._completionSource.TrySetException(new RpcException(error));
        }

        public void Resend()
        {
            this._sendAction();
        }

        public bool CanReceive(object o)
        {
            return o is TResponse;
        }

        public void Acknowledge()
        {
            // TODO
        }
    }

    public class RpcException : MTProtoException
    {
        public RpcException(IRpcError error)
            :base(string.Format("RPC error {0}: {1}", error.ErrorCode, error.ErrorMessage))
        {
        }

        public RpcException()
            :base("Unspecified RPC error.")
        {
        }
    }
}