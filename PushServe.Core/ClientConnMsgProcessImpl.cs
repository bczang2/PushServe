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
    public class ClientConnMsgProcessImpl : IMsgProcess
    {
        /// <summary>
        /// 客户端连接维护
        /// </summary>
        /// <param name="value"></param>
        /// <param name="clients"></param>
        /// <param name="connectionId"></param>
        public void MsgProcess(ClientMsgPackEntity value, IHubCallerConnectionContext<dynamic> clients = null, string connectionId = "")
        {
            if (value != null)
            {
                ClientConnMsgPack clientPack = JsonUtil<ClientConnMsgPack>.Deserialize(value.mp_cotent.ToString());
                if (clientPack != null)
                {
                    Parallel.Invoke(
                        () => { AddClientInfo(connectionId, clientPack); },
                        () => { AddClientConnInfo(connectionId, clientPack); }
                    );
                }
            }
        }

        private static void AddClientInfo(string connectionId, ClientConnMsgPack clientPack)
        {
            ClientInfo info = new ClientInfo()
            {
                uid = clientPack.uid,
                connid = connectionId,
                timespan = DateTime.Now
            };
            RedisHelper.Hash_Set<ClientInfo>(Constant.RedisClusterConn, Constant.ClientInfoKey, connectionId, info);
        }

        private static void AddClientConnInfo(string connectionId, ClientConnMsgPack clientPack)
        {
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
