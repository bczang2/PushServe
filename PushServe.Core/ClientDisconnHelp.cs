using PushServe.Entity;
using PushServe.Util;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PushServe.Core
{
    public class ClientDisconnHelp
    {
        /// <summary>
        /// 客户端断开连接处理
        /// </summary>
        /// <param name="connectId"></param>
        public static void DisconnectedOp(string connectId)
        {
            ClientInfo client = RedisHelper.Hash_Get<ClientInfo>(Constant.RedisClusterConn, Constant.ClientInfoKey, connectId);
            if (client != null)
            {
                //deleted clientinfo
                RedisHelper.Hash_Remove(Constant.RedisClusterConn, Constant.ClientInfoKey, connectId);
                //deleted clientconninfo
                List<string> connIds = RedisHelper.Hash_Get<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey, client.uid);
                if (connIds != null && connIds.Any())
                {
                    if (connIds.Remove(connectId))
                    {
                        if (connIds.Count > 0)
                        {
                            RedisHelper.Hash_Set<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey, client.uid, connIds);
                        }
                        else
                        {
                            RedisHelper.Hash_Remove(Constant.RedisClusterConn, Constant.ClientConnKey, client.uid);
                        }
                    }
                }
            }
        }
    }
}
