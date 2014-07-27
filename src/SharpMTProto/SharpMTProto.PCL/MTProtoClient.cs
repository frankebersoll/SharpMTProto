// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MTProtoClient.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Catel.Logging;
using Raksha.Asn1;
using Raksha.Utilities.IO.Pem;
using SharpMTProto.Authentication;
using SharpMTProto.Client;
using SharpMTProto.Connection;
using SharpMTProto.Messaging;
using SharpMTProto.Properties;
using SharpMTProto.Schema.Api;
using SharpMTProto.Transport;
using SharpTL;

namespace SharpMTProto
{
    /// <summary>
    ///     MTProto client.
    /// </summary>
    [UsedImplicitly]
    public class MTProtoClient : IDisposable
    {
        private readonly MTProtoAppConfiguration _configuration;
        private readonly IPersistance _persistance;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private bool _isDisposed;
        private readonly DefaultContainer _container;
        private ApiAsyncMethods _api;

        public ApiAsyncMethods Api
        {
            get { return this._api; }
        }

        #region Disposable
        public void Dispose()
        {
            Dispose(true);
        }

        public MTProtoClient(MTProtoAppConfiguration configuration, ITransportFactory transportFactory, IPersistance persistance)
        {
            this._configuration = configuration;
            this._persistance = persistance;
            this._container = new DefaultContainer(transportFactory);
        }

        private static void AddServerCert(IKeyChain keyChain, string serverCert)
        {
            var stringReader = new StringReader(serverCert);
            var r = new PemReader(stringReader);

            var publicKey = r.ReadPemObject();

            var parser = new Asn1StreamParser(publicKey.Content);
            var o = (DerSequenceParser)parser.ReadObject();
            var o1 = (DerSequence)o.ToAsn1Object();

            var a = (DerInteger)o1[0];
            var b = (DerInteger)o1[1];
            var modulus = a.Value.ToString("X");
            var exponent = b.Value.ToString("X");

            var x = new PublicKey(modulus, exponent, 0);
            var fingerprint = ((KeyChain)keyChain).ComputeFingerprint(x.Modulus, x.Exponent);
            keyChain.Add(new PublicKey(a.PositiveValue.ToByteArray(), b.PositiveValue.ToByteArray(), fingerprint));
        }

        public async Task Start()
        {
            var phoneCodeRequested = this.PhoneCodeRequested;
            if (phoneCodeRequested == null)
            {
                throw new Exception("PhoneCodeRequested event must be subscribed.");
            }

            var phoneNumberRequested = this.PhoneNumberRequested;
            if (phoneNumberRequested == null)
            {
                throw new Exception("PhoneNumberRequested event must be subscribed.");
            }

            var registrationRequested = this.RegistrationRequested;
            if (registrationRequested == null)
            {
                throw new Exception("RegistrationRequested event must be subscribed.");
            }

            RegisterSerializers();

            var transportConfigProvider = _container.TransportConfigProvider;
            transportConfigProvider.DefaultTransportConfig
                = new TcpTransportConfig(_configuration.ServerIpAddress, _configuration.ServerPort);

            AddServerCert(this._container.KeyChain, this._configuration.ServerCert);

            var persistanceInfo = await this._persistance.Load();
            if (persistanceInfo == null)
            {
                var key = await NegotiateAuthInfo(this._container);
                persistanceInfo = new PersistanceInfo();
                persistanceInfo.SetAuthInfo(key);
                await this._persistance.Save(persistanceInfo);
            }

            var authenticationInfo = new AuthenticationInfo(persistanceInfo.AuthKey, persistanceInfo.Salt);
            _container.AuthenticationManager.SetInfo(authenticationInfo);

            var connection = this._container.ConnectionFactory.Create();
            await connection.Connect();

            var initConnection = new InvokeWithLayer14Args
            {
                Query = new InitConnectionArgs
                {
                    ApiId = 13216,
                    AppVersion = "0.1",
                    DeviceModel = "WIN8",
                    SystemVersion = "14",
                    LangCode = "de",
                    Query = new HelpGetConfigArgs(),
                }
            };

            this._api = new ApiAsyncMethods(new ConnectionAdapter(connection, MessageType.Encrypted));

            object configResult = await this._api.InvokeWithLayer14Async(initConnection);

            Action<Action<string>> phoneCodeRequest
                = callback => phoneCodeRequested(this, new PhoneCodeEventArgs(callback));

            Action<Action<RegistrationInfo>> registrationRequest
                = callback => registrationRequested(this, new RegistrationInfoEventArgs(callback));

            Action<Action<string>> phoneNumberRequest
                = callback => phoneNumberRequested(this, new PhoneNumberEventArgs(callback));

            var authNegotiator = new AuthorizationNegotiator(
                this._api,
                this._configuration,
                this._persistance,
                phoneNumberRequest,
                registrationRequest,
                phoneCodeRequest);

            var authorizationInfo = await authNegotiator.Authorize();

            this.UserId = authorizationInfo.UserId;
            this.UserPhone = authorizationInfo.UserPhone;
        }

        public uint UserId{get; private set; }

        public string UserPhone { get; private set; }

        public event EventHandler<RegistrationInfoEventArgs> RegistrationRequested;

        public event EventHandler<PhoneNumberEventArgs> PhoneNumberRequested;

        public event EventHandler<PhoneCodeEventArgs> PhoneCodeRequested;

        private static void RegisterSerializers()
        {
            TLRig.Default.PrepareSerializersForAllTLObjectsInAssembly(typeof(MTProtoClient).Assembly());
        }

        private static async Task<AuthenticationInfo> NegotiateAuthInfo(DefaultContainer container)
        {
            var negotiator = new AuthKeyNegotiator(
                container.ConnectionFactory,
                container.TLRig,
                container.NonceGenerator,
                container.HashServices,
                container.EncryptionServices,
                container.KeyChain,
                container.TransportConfigProvider);

            AuthenticationInfo key;

            while (true)
            {
                try
                {
                    key = await negotiator.CreateAuthKey();
                    break;
                }
                catch (Exception)
                {
                }
            }

            return key;
        }

        protected void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                
                throw new ObjectDisposedException(GetType().FullName, "Can not access disposed client.");
            }
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            if (isDisposing)
            {
            }
        }
        #endregion
    }
}
