using Microsoft.AspNet.SignalR.Hubs;
using PushServe.Entity;
using PushServe.Util;
using System;

namespace PushServe.Core
{
    public class GloableMsgProcessImpl : IMsgProcess
    {
        public void MsgProcess(ClientMsgPackEntity value, IHubCallerConnectionContext<dynamic> clients = null, string connectionId = "")
        {
            if (value != null)
            {
                GloableMsgPack gloableMsgPack = JsonUtil<GloableMsgPack>.Deserialize(value.mp_cotent.ToString());
                if (gloableMsgPack != null)
                {
                    clients.All.Push(gloableMsgPack.resdata);
                }
            }
        }
    }
}
