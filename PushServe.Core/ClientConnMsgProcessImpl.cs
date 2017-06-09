using Microsoft.AspNet.SignalR.Hubs;
using PushServe.Entity;
using PushServe.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushServe.Core
{
    public class ClientConnMsgProcessImpl : IMsgProcess
    {
        public void MsgProcess(ClientMsgPackEntity value, IHubCallerConnectionContext<dynamic> clients = null, string connectionId = "")
        {
            if (value != null)
            {
                ClientConnMsgPack clientPack = JsonUtil<ClientConnMsgPack>.Deserialize(value.mp_cotent.ToString());
                if (clientPack != null)
                {
                    ClientInfo info = new ClientInfo()
                    {
                        uid = clientPack.uid,
                        connid = connectionId,
                        timespan = DateTime.Now
                    };
                    RedisHelper.Hash_Set<ClientInfo>(Constant.RedisClusterConn, Constant.ClientInfoKey, connectionId, info);

                    if (RedisHelper.Hash_Exist(Constant.RedisClusterConn, Constant.ClientConnKey, clientPack.uid))
                    {
                        List<string> connIds = RedisHelper.Hash_Get<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey, clientPack.uid);
                        connIds = connIds ?? new List<string>();
                        if (!connIds.Contains(connectionId))
                        {
                            connIds.Add(connectionId);
                        }
                        RedisHelper.Hash_Set<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey, clientPack.uid, connIds);
                    }
                    else
                    {
                        RedisHelper.Hash_Set<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey, clientPack.uid, new List<string>() { connectionId });
                    }
                }
            }
        }
    }
}
