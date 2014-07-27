using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMTProto.Schema.Api;

namespace SharpMTProto.Client
{
    public class AuthorizationInfo
    {
        public AuthorizationInfo(PersistanceInfo persistanceInfo)
        {
            this.UserId = persistanceInfo.UserId;
            this.UserPhone = persistanceInfo.UserPhone;
        }

        public uint UserId { get; private set; }

        public string UserPhone { get; private set; }
    }

    public class AuthorizationNegotiator
    {
        private readonly ApiAsyncMethods _api;
        private readonly MTProtoAppConfiguration _config;
        private readonly IPersistance _persistance;
        private readonly Action<Action<string>> _phoneNumberRequest;
        private readonly Action<Action<RegistrationInfo>> _registrationInfoRequest;
        private readonly Action<Action<string>> _phoneCodeRequest;

        public AuthorizationNegotiator(
            ApiAsyncMethods api, 
            MTProtoAppConfiguration config, 
            IPersistance persistance,
            Action<Action<string>> phoneNumberRequest,
            Action<Action<RegistrationInfo>> registrationInfoRequest,
            Action<Action<string>> phoneCodeRequest)
        {
            this._api = api;
            this._config = config;
            this._persistance = persistance;
            this._phoneNumberRequest = phoneNumberRequest;
            this._registrationInfoRequest = registrationInfoRequest;
            this._phoneCodeRequest = phoneCodeRequest;
        }

        public async Task<AuthorizationInfo> Authorize()
        {
            var persistanceInfo = await _persistance.Load();
            var authExpires = persistanceInfo.AuthorizationExpires;

            if (authExpires == null && !(authExpires > DateTime.UtcNow))
            {
                var phoneNumber = await this.GetInput(this._phoneNumberRequest);

                var sendCode = new AuthSendCodeArgs
                               {
                                   ApiHash = this._config.ApiHash,
                                   ApiId = (uint) this._config.ApiId,
                                   PhoneNumber = phoneNumber,
                                   SmsType = 1,
                                   LangCode = "de"
                               };

                var authSentCode = await this._api.AuthSendCodeAsync(sendCode);

                var phoneCode = await this.GetInput(this._phoneCodeRequest);

                IAuthAuthorization authorization;
                if (!authSentCode.PhoneRegistered)
                {
                    var registrationInfo = await this.GetInput(this._registrationInfoRequest);

                    var signUp = new AuthSignUpArgs
                                 {
                                     PhoneCode = phoneCode,
                                     PhoneCodeHash = authSentCode.PhoneCodeHash,
                                     PhoneNumber = sendCode.PhoneNumber,
                                     FirstName = registrationInfo.FirstName,
                                     LastName = registrationInfo.LastName
                                 };

                    authorization = await this._api.AuthSignUpAsync(signUp);
                }
                else
                {
                    var signIn = new AuthSignInArgs
                                 {
                                     PhoneCode = phoneCode,
                                     PhoneCodeHash = authSentCode.PhoneCodeHash,
                                     PhoneNumber = sendCode.PhoneNumber
                                 };

                    authorization = await this._api.AuthSignInAsync(signIn);
                }

                persistanceInfo = await this._persistance.Load();
                persistanceInfo.SetAuthorization(authorization);
                await this._persistance.Save(persistanceInfo);
            }

            var info = new AuthorizationInfo(persistanceInfo);
            return info;
        }

        private async Task<T> GetInput<T>(Action<Action<T>> callback)
        {
            var completionSource
                = new TaskCompletionSource<T>();
            
            callback(s => completionSource.TrySetResult(s));

            var result = await completionSource.Task;
            return result;
        }
    }
}