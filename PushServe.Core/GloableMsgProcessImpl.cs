using Microsoft.AspNet.SignalR.Hubs;
using PushServe.Entity;
using PushServe.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
