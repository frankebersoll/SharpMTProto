using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpMTProto.Client
{
    public class PhoneCodeEventArgs : EventArgs
    {
        public PhoneCodeEventArgs(Action<string> phoneCodeCallback)
        {
            this.PhoneCodeCallback = phoneCodeCallback;
        }

        public Action<string> PhoneCodeCallback { get; private set; }
    }
}