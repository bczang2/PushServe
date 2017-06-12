using Microsoft.AspNet.SignalR.Hubs;
using PushServe.Entity;
using PushServe.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushServe.Core
{
    public class NewMsgPushProcessImpl : IMsgProcess
    {
        public void MsgProcess(ClientMsgPackEntity value, IHubCallerConnectionContext<dynamic> clients = null, string connectionId = "")
        {
            if (value != null)
            {
                NewPushMsgPack newMsgPack = JsonUtil<NewPushMsgPack>.Deserialize(value.mp_cotent.ToString());
                if (newMsgPack != null)
                {
                    List<string> connIds = RedisHelper.Hash_Get<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey, newMsgPack.uid);
                    if (connIds != null && connIds.Any())
                    {
                        if (clients != null)
                        {
                            clients.Clients(connIds).Push(newMsgPack.resdata);
                        }
                    }
                }
            }

            clients.Client(connectionId).Push("接受到消息!");
        }
    }
}
