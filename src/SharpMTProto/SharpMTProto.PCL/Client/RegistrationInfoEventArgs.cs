using System;

namespace SharpMTProto.Client
{
    public class RegistrationInfoEventArgs : EventArgs
    {
        public RegistrationInfoEventArgs(Action<RegistrationInfo> registrationCallback)
        {
            this.RegistrationCallback = registrationCallback;
        }

        public Action<RegistrationInfo> RegistrationCallback { get; private set; }
    }
}