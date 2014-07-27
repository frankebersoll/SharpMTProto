using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Messaging
{
    public interface IRequest
    {
        void Receive(object o);

        void Resend();

        bool CanReceive(object o);

        void Acknowledge();
    }
}