// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SharpTL compiler (https://github.com/Taggersoft/SharpTL).
//     Generated at 07/21/2014 01:02:14
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#pragma warning disable 1591
namespace SharpMTProto.Schema.MTProto
{
    using SharpTL;
    using System.Threading.Tasks;
    
    // TL constructors.

    [TLObject(0x05162463)]
    public partial class ResPQ : IResPQ
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public System.Byte[] Pq { get; set; }

        [TLProperty(4)]
        public System.Collections.Generic.List<System.UInt64> ServerPublicKeyFingerprints { get; set; }

    }

    [TLObject(0x83C95AEC)]
    public partial class PQInnerData : IPQInnerData
    {
        [TLProperty(1)]
        public System.Byte[] Pq { get; set; }

        [TLProperty(2)]
        public System.Byte[] P { get; set; }

        [TLProperty(3)]
        public System.Byte[] Q { get; set; }

        [TLProperty(4)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(5)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(6)]
        public BigMath.Int256 NewNonce { get; set; }

    }

    [TLObject(0x79CB045D)]
    public partial class ServerDHParamsFail : IServerDHParams
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public BigMath.Int128 NewNonceHash { get; set; }

    }

    [TLObject(0xD0E8075C)]
    public partial class ServerDHParamsOk : IServerDHParams
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public System.Byte[] EncryptedAnswer { get; set; }

    }

    [TLObject(0xB5890DBA)]
    public partial class ServerDHInnerData : IServerDHInnerData
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public System.UInt32 G { get; set; }

        [TLProperty(4)]
        public System.Byte[] DhPrime { get; set; }

        [TLProperty(5)]
        public System.Byte[] GA { get; set; }

        [TLProperty(6)]
        public System.UInt32 ServerTime { get; set; }

    }

    [TLObject(0x6643B654)]
    public partial class ClientDHInnerData : IClientDHInnerData
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public System.UInt64 RetryId { get; set; }

        [TLProperty(4)]
        public System.Byte[] GB { get; set; }

    }

    [TLObject(0x3BCBF734)]
    public partial class DhGenOk : ISetClientDHParamsAnswer
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public BigMath.Int128 NewNonceHash1 { get; set; }

    }

    [TLObject(0x46DC1FB9)]
    public partial class DhGenRetry : ISetClientDHParamsAnswer
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public BigMath.Int128 NewNonceHash2 { get; set; }

    }

    [TLObject(0xA69DAE02)]
    public partial class DhGenFail : ISetClientDHParamsAnswer
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public BigMath.Int128 NewNonceHash3 { get; set; }

    }

    [TLObject(0xF35C6D01)]
    public partial class RpcResult : IRpcResult
    {
        [TLProperty(1)]
        public System.UInt64 ReqMsgId { get; set; }

        [TLProperty(2)]
        public System.Object Result { get; set; }

    }

    [TLObject(0x2144CA19)]
    public partial class RpcError : IRpcError
    {
        [TLProperty(1)]
        public System.UInt32 ErrorCode { get; set; }

        [TLProperty(2)]
        public System.String ErrorMessage { get; set; }

    }

    [TLObject(0x5E2AD36E)]
    public partial class RpcAnswerUnknown : IRpcDropAnswer
    {
    }

    [TLObject(0xCD78E586)]
    public partial class RpcAnswerDroppedRunning : IRpcDropAnswer
    {
    }

    [TLObject(0xA43AD8B7)]
    public partial class RpcAnswerDropped : IRpcDropAnswer
    {
        [TLProperty(1)]
        public System.UInt64 MsgId { get; set; }

        [TLProperty(2)]
        public System.UInt32 SeqNo { get; set; }

        [TLProperty(3)]
        public System.UInt32 Bytes { get; set; }

    }

    [TLObject(0x0949D9DC)]
    public partial class FutureSalt : IFutureSalt
    {
        [TLProperty(1)]
        public System.UInt32 ValidSince { get; set; }

        [TLProperty(2)]
        public System.UInt32 ValidUntil { get; set; }

        [TLProperty(3)]
        public System.UInt64 Salt { get; set; }

    }

    [TLObject(0xAE500895)]
    public partial class FutureSalts : IFutureSalts
    {
        [TLProperty(1)]
        public System.UInt64 ReqMsgId { get; set; }

        [TLProperty(2)]
        public System.UInt32 Now { get; set; }

        [TLProperty(3, TLSerializationMode.Bare)]
        public System.Collections.Generic.List<IFutureSalt> Salts { get; set; }

    }

    [TLObject(0x347773C5)]
    public partial class Pong : IPong
    {
        [TLProperty(1)]
        public System.UInt64 MsgId { get; set; }

        [TLProperty(2)]
        public System.UInt64 PingId { get; set; }

    }

    [TLObject(0xE22045FC)]
    public partial class DestroySessionOk : IDestroySessionRes
    {
        [TLProperty(1)]
        public System.UInt64 SessionId { get; set; }

    }

    [TLObject(0x62D350C9)]
    public partial class DestroySessionNone : IDestroySessionRes
    {
        [TLProperty(1)]
        public System.UInt64 SessionId { get; set; }

    }

    [TLObject(0x9EC20908)]
    public partial class NewSessionCreated : INewSession
    {
        [TLProperty(1)]
        public System.UInt64 FirstMsgId { get; set; }

        [TLProperty(2)]
        public System.UInt64 UniqueId { get; set; }

        [TLProperty(3)]
        public System.UInt64 ServerSalt { get; set; }

    }

    [TLObject(0x73F1F8DC)]
    public partial class MsgContainer : IMessageContainer
    {
        [TLProperty(1, TLSerializationMode.Bare)]
        public System.Collections.Generic.List<Message> Messages { get; set; }

    }

    [TLObject(0x5BB8E511)]
    public partial class Message : IMessage
    {
        [TLProperty(1)]
        public System.UInt64 MsgId { get; set; }

        [TLProperty(2)]
        public System.UInt32 Seqno { get; set; }

        [TLProperty(3)]
        public System.UInt32 Bytes { get; set; }

        [TLProperty(4)]
        public System.Object Body { get; set; }

    }

    [TLObject(0xE06046B2)]
    public partial class MsgCopy : IMessageCopy
    {
        [TLProperty(1)]
        public IMessage OrigMessage { get; set; }

    }

    [TLObject(0x3072CFA1)]
    public partial class GzipPacked
    {
        [TLProperty(1)]
        public System.Byte[] PackedData { get; set; }

    }

    [TLObject(0x62D6B459)]
    public partial class MsgsAck : IMsgsAck
    {
        [TLProperty(1)]
        public System.Collections.Generic.List<System.UInt64> MsgIds { get; set; }

    }

    [TLObject(0xA7EFF811)]
    public partial class BadMsgNotification : IBadMsgNotification
    {
        [TLProperty(1)]
        public System.UInt64 BadMsgId { get; set; }

        [TLProperty(2)]
        public System.UInt32 BadMsgSeqno { get; set; }

        [TLProperty(3)]
        public System.UInt32 ErrorCode { get; set; }

    }

    [TLObject(0xEDAB447B)]
    public partial class BadServerSalt : IBadMsgNotification
    {
        [TLProperty(1)]
        public System.UInt64 BadMsgId { get; set; }

        [TLProperty(2)]
        public System.UInt32 BadMsgSeqno { get; set; }

        [TLProperty(3)]
        public System.UInt32 ErrorCode { get; set; }

        [TLProperty(4)]
        public System.UInt64 NewServerSalt { get; set; }

    }

    [TLObject(0x7D861A08)]
    public partial class MsgResendReq : IMsgResendReq
    {
        [TLProperty(1)]
        public System.Collections.Generic.List<System.UInt64> MsgIds { get; set; }

    }

    [TLObject(0xDA69FB52)]
    public partial class MsgsStateReq : IMsgsStateReq
    {
        [TLProperty(1)]
        public System.Collections.Generic.List<System.UInt64> MsgIds { get; set; }

    }

    [TLObject(0x04DEB57D)]
    public partial class MsgsStateInfo : IMsgsStateInfo
    {
        [TLProperty(1)]
        public System.UInt64 ReqMsgId { get; set; }

        [TLProperty(2)]
        public System.Byte[] Info { get; set; }

    }

    [TLObject(0x8CC0D131)]
    public partial class MsgsAllInfo : IMsgsAllInfo
    {
        [TLProperty(1)]
        public System.Collections.Generic.List<System.UInt64> MsgIds { get; set; }

        [TLProperty(2)]
        public System.Byte[] Info { get; set; }

    }

    [TLObject(0x276D3EC6)]
    public partial class MsgDetailedInfo : IMsgDetailedInfo
    {
        [TLProperty(1)]
        public System.UInt64 MsgId { get; set; }

        [TLProperty(2)]
        public System.UInt64 AnswerMsgId { get; set; }

        [TLProperty(3)]
        public System.UInt32 Bytes { get; set; }

        [TLProperty(4)]
        public System.UInt32 Status { get; set; }

    }

    [TLObject(0x809DB6DF)]
    public partial class MsgNewDetailedInfo : IMsgDetailedInfo
    {
        [TLProperty(1)]
        public System.UInt64 AnswerMsgId { get; set; }

        [TLProperty(2)]
        public System.UInt32 Bytes { get; set; }

        [TLProperty(3)]
        public System.UInt32 Status { get; set; }

    }

    [TLObject(0x60469778)]
    public partial class ReqPqArgs
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

    }

    [TLObject(0xD712E4BE)]
    public partial class ReqDHParamsArgs
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public System.Byte[] P { get; set; }

        [TLProperty(4)]
        public System.Byte[] Q { get; set; }

        [TLProperty(5)]
        public System.UInt64 PublicKeyFingerprint { get; set; }

        [TLProperty(6)]
        public System.Byte[] EncryptedData { get; set; }

    }

    [TLObject(0xF5045F1F)]
    public partial class SetClientDHParamsArgs
    {
        [TLProperty(1)]
        public BigMath.Int128 Nonce { get; set; }

        [TLProperty(2)]
        public BigMath.Int128 ServerNonce { get; set; }

        [TLProperty(3)]
        public System.Byte[] EncryptedData { get; set; }

    }

    [TLObject(0x58E4A740)]
    public partial class RpcDropAnswerArgs
    {
        [TLProperty(1)]
        public System.UInt64 ReqMsgId { get; set; }

    }

    [TLObject(0xB921BD04)]
    public partial class GetFutureSaltsArgs
    {
        [TLProperty(1)]
        public System.UInt32 Num { get; set; }

    }

    [TLObject(0x7ABE77EC)]
    public partial class PingArgs
    {
        [TLProperty(1)]
        public System.UInt64 PingId { get; set; }

    }

    [TLObject(0xF3427B8C)]
    public partial class PingDelayDisconnectArgs
    {
        [TLProperty(1)]
        public System.UInt64 PingId { get; set; }

        [TLProperty(2)]
        public System.UInt32 DisconnectDelay { get; set; }

    }

    [TLObject(0xE7512126)]
    public partial class DestroySessionArgs
    {
        [TLProperty(1)]
        public System.UInt64 SessionId { get; set; }

    }

    [TLObject(0x9299359F)]
    public partial class HttpWaitArgs
    {
        [TLProperty(1)]
        public System.UInt32 MaxDelay { get; set; }

        [TLProperty(2)]
        public System.UInt32 WaitAfter { get; set; }

        [TLProperty(3)]
        public System.UInt32 MaxWait { get; set; }

    }


    // TL types.

    [TLType(typeof(ResPQ))]
    public interface IResPQ
    {
		BigMath.Int128 Nonce { get; }
		BigMath.Int128 ServerNonce { get; }
		System.Byte[] Pq { get; }
		System.Collections.Generic.List<System.UInt64> ServerPublicKeyFingerprints { get; }
    }

    [TLType(typeof(PQInnerData))]
    public interface IPQInnerData
    {
		System.Byte[] Pq { get; }
		System.Byte[] P { get; }
		System.Byte[] Q { get; }
		BigMath.Int128 Nonce { get; }
		BigMath.Int128 ServerNonce { get; }
		BigMath.Int256 NewNonce { get; }
    }

    [TLType(typeof(ServerDHParamsFail), typeof(ServerDHParamsOk))]
    public interface IServerDHParams
    {
		BigMath.Int128 Nonce { get; }
		BigMath.Int128 ServerNonce { get; }
    }

    [TLType(typeof(ServerDHInnerData))]
    public interface IServerDHInnerData
    {
		BigMath.Int128 Nonce { get; }
		BigMath.Int128 ServerNonce { get; }
		System.UInt32 G { get; }
		System.Byte[] DhPrime { get; }
		System.Byte[] GA { get; }
		System.UInt32 ServerTime { get; }
    }

    [TLType(typeof(ClientDHInnerData))]
    public interface IClientDHInnerData
    {
		BigMath.Int128 Nonce { get; }
		BigMath.Int128 ServerNonce { get; }
		System.UInt64 RetryId { get; }
		System.Byte[] GB { get; }
    }

    [TLType(typeof(DhGenOk), typeof(DhGenRetry), typeof(DhGenFail))]
    public interface ISetClientDHParamsAnswer
    {
		BigMath.Int128 Nonce { get; }
		BigMath.Int128 ServerNonce { get; }
    }

    [TLType(typeof(RpcResult))]
    public interface IRpcResult
    {
		System.UInt64 ReqMsgId { get; }
		System.Object Result { get; }
    }

    [TLType(typeof(RpcError))]
    public interface IRpcError
    {
		System.UInt32 ErrorCode { get; }
		System.String ErrorMessage { get; }
    }

    [TLType(typeof(RpcAnswerUnknown), typeof(RpcAnswerDroppedRunning), typeof(RpcAnswerDropped))]
    public interface IRpcDropAnswer
    {
    }

    [TLType(typeof(FutureSalt))]
    public interface IFutureSalt
    {
		System.UInt32 ValidSince { get; }
		System.UInt32 ValidUntil { get; }
		System.UInt64 Salt { get; }
    }

    [TLType(typeof(FutureSalts))]
    public interface IFutureSalts
    {
		System.UInt64 ReqMsgId { get; }
		System.UInt32 Now { get; }
		System.Collections.Generic.List<IFutureSalt> Salts { get; }
    }

    [TLType(typeof(Pong))]
    public interface IPong
    {
		System.UInt64 MsgId { get; }
		System.UInt64 PingId { get; }
    }

    [TLType(typeof(DestroySessionOk), typeof(DestroySessionNone))]
    public interface IDestroySessionRes
    {
		System.UInt64 SessionId { get; }
    }

    [TLType(typeof(NewSessionCreated))]
    public interface INewSession
    {
		System.UInt64 FirstMsgId { get; }
		System.UInt64 UniqueId { get; }
		System.UInt64 ServerSalt { get; }
    }

    [TLType(typeof(MsgContainer))]
    public interface IMessageContainer
    {
		System.Collections.Generic.List<Message> Messages { get; }
    }

    [TLType(typeof(Message))]
    public interface IMessage
    {
		System.UInt64 MsgId { get; }
		System.UInt32 Seqno { get; }
		System.UInt32 Bytes { get; }
		System.Object Body { get; }
    }

    [TLType(typeof(MsgCopy))]
    public interface IMessageCopy
    {
		IMessage OrigMessage { get; }
    }

    [TLType(typeof(MsgsAck))]
    public interface IMsgsAck
    {
		System.Collections.Generic.List<System.UInt64> MsgIds { get; }
    }

    [TLType(typeof(BadMsgNotification), typeof(BadServerSalt))]
    public interface IBadMsgNotification
    {
		System.UInt64 BadMsgId { get; }
		System.UInt32 BadMsgSeqno { get; }
		System.UInt32 ErrorCode { get; }
    }

    [TLType(typeof(MsgResendReq))]
    public interface IMsgResendReq
    {
		System.Collections.Generic.List<System.UInt64> MsgIds { get; }
    }

    [TLType(typeof(MsgsStateReq))]
    public interface IMsgsStateReq
    {
		System.Collections.Generic.List<System.UInt64> MsgIds { get; }
    }

    [TLType(typeof(MsgsStateInfo))]
    public interface IMsgsStateInfo
    {
		System.UInt64 ReqMsgId { get; }
		System.Byte[] Info { get; }
    }

    [TLType(typeof(MsgsAllInfo))]
    public interface IMsgsAllInfo
    {
		System.Collections.Generic.List<System.UInt64> MsgIds { get; }
		System.Byte[] Info { get; }
    }

    [TLType(typeof(MsgDetailedInfo), typeof(MsgNewDetailedInfo))]
    public interface IMsgDetailedInfo
    {
		System.UInt64 AnswerMsgId { get; }
		System.UInt32 Bytes { get; }
		System.UInt32 Status { get; }
    }


    /// <summary>
    ///		MTProto async methods.
    /// </summary>
    public interface IMTProtoAsyncMethods
    {
        Task<IResPQ> ReqPqAsync(ReqPqArgs args);
        Task<IServerDHParams> ReqDHParamsAsync(ReqDHParamsArgs args);
        Task<ISetClientDHParamsAnswer> SetClientDHParamsAsync(SetClientDHParamsArgs args);
        Task<IRpcDropAnswer> RpcDropAnswerAsync(RpcDropAnswerArgs args);
        Task<IFutureSalts> GetFutureSaltsAsync(GetFutureSaltsArgs args);
        Task<IPong> PingAsync(PingArgs args);
        Task<IPong> PingDelayDisconnectAsync(PingDelayDisconnectArgs args);
        Task<IDestroySessionRes> DestroySessionAsync(DestroySessionArgs args);
        Task HttpWaitAsync(HttpWaitArgs args);
    }

    /// <summary>
    ///		MTProto async methods.
    /// </summary>
    public class MTProtoAsyncMethods : IMTProtoAsyncMethods
    {
		private readonly ITLConnectionAdapter connection;

		public MTProtoAsyncMethods(ITLConnectionAdapter connection)
		{
			this.connection = connection;
		}
        public Task<IResPQ> ReqPqAsync(ReqPqArgs args)
		{
			return this.connection.Send<IResPQ>(args);
		}
        public Task<IServerDHParams> ReqDHParamsAsync(ReqDHParamsArgs args)
		{
			return this.connection.Send<IServerDHParams>(args);
		}
        public Task<ISetClientDHParamsAnswer> SetClientDHParamsAsync(SetClientDHParamsArgs args)
		{
			return this.connection.Send<ISetClientDHParamsAnswer>(args);
		}
        public Task<IRpcDropAnswer> RpcDropAnswerAsync(RpcDropAnswerArgs args)
		{
			return this.connection.Send<IRpcDropAnswer>(args);
		}
        public Task<IFutureSalts> GetFutureSaltsAsync(GetFutureSaltsArgs args)
		{
			return this.connection.Send<IFutureSalts>(args);
		}
        public Task<IPong> PingAsync(PingArgs args)
		{
			return this.connection.Send<IPong>(args);
		}
        public Task<IPong> PingDelayDisconnectAsync(PingDelayDisconnectArgs args)
		{
			return this.connection.Send<IPong>(args);
		}
        public Task<IDestroySessionRes> DestroySessionAsync(DestroySessionArgs args)
		{
			return this.connection.Send<IDestroySessionRes>(args);
		}
        public Task HttpWaitAsync(HttpWaitArgs args)
		{
			return this.connection.Send<bool>(args);
		}
    }




}
#pragma warning restore 1591
