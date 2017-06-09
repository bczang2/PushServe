using PushServe.Entity;
using PushServe.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Core
{
    public class ClientDisconnHelp
    {
        public static void DisconnectedOp(string connectId)
        {
            ClientInfo client = RedisHelper.Hash_Get<ClientInfo>(Constant.RedisClusterConn, Constant.ClientInfoKey, connectId);
            if (client != null)
            {
                RedisHelper.Hash_Remove(Constant.RedisClusterConn, Constant.ClientInfoKey, connectId);
                List<string> connIds = RedisHelper.Hash_Get<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey, client.uid);
                if (connIds != null && connIds.Any())
                {
                    connIds.Remove(connectId);
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
