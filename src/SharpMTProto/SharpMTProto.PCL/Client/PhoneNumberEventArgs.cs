using System;

namespace SharpMTProto.Client
{
    public class PhoneNumberEventArgs : EventArgs
    {
        public PhoneNumberEventArgs(Action<string> phoneNumberCallback)
        {
            this.PhoneNumberCallback = phoneNumberCallback;
        }

        public Action<string> PhoneNumberCallback { get; private set; }
    }
}