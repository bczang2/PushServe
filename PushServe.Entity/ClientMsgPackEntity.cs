using System;

namespace PushServe.Entity
{
    public class ClientMsgPackEntity
    {
        public MpTypeEnum mp_type { get; set; }

        public object mp_cotent { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    public enum MpTypeEnum
    {
        ClientConn = 1,
        NewMsgPush = 2,
        GloableMsg = 3
    }
}