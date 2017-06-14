using Microsoft.AspNet.SignalR.Hubs;
using PushServe.Entity;
using PushServe.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace PushServe.Core
{
    public class NewMsgPushProcessImpl : IMsgProcess
    {
        public void MsgProcess(ClientMsgPackEntity value, IHubCallerConnectionContext<dynamic> clients = null, string connectionId = "")
        {
            Task<bool> ts = new Task<bool>((arg) =>
            {
                bool ret = ConvertUtil.ConvertObjectToBool(arg, false);
                try
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
                                    ret = true;
                                }
                            }
                        }
                    }
                }
                catch { }
                return ret;
            }, false);

            ts.Start();
            ts.ContinueWith(t =>
            {
                clients.Client(connectionId).Push(ts.Result ? "推送消息到客户端成功!" : "推送消息到客户端失败!");
            });

            clients.Client(connectionId).Push("服务端接受到消息!");
        }
    }
}
