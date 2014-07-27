using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SharpMTProto.Authentication;
using SharpMTProto.Schema.Api;
using SharpMTProto.Utils;

namespace SharpMTProto.Client
{
    [DataContract]
    public class PersistanceInfo
    {
        [DataMember]
        public byte[] AuthKey { get; set; }

        [DataMember]
        public ulong Salt { get; set; }

        [DataMember]
        public DateTime? AuthorizationExpires { get; set; }

        [DataMember]
        public uint UserId { get; set; }

        [DataMember]
        public string UserPhone { get; set; }

        internal void SetAuthorization(IAuthAuthorization authorization)
        {
            DateTime expires = UnixTimeUtils.DateTimeFromUnixTimestampSeconds(authorization.Expires);
            this.AuthorizationExpires = expires;

            var user = (UserSelf) authorization.User;
            this.UserId = user.Id;
            this.UserPhone = user.Phone;
        }

        internal void SetAuthInfo(AuthenticationInfo info)
        {
            this.AuthKey = info.AuthKey;
            this.Salt = info.Salt;
        }
    }
}