using PushServe.Entity;
using PushServe.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Core
{
    public class DashboardHelp
    {
        public static ClientViewInfo QueryViewInfo()
        {
            ClientViewInfo ret = new ClientViewInfo();
            ret.ConnNum = RedisHelper.Hash_GetCount(Constant.RedisClusterConn, Constant.ClientConnKey);

            var dic = RedisHelper.Hash_GetAllEntries<List<string>>(Constant.RedisClusterConn, Constant.ClientConnKey);
            if (dic != null && dic.Any())
            {
                ret.ConnInfoList = new List<ConnInfo>();
                foreach (var item in dic)
                {
                    var values = item.Value;
                    values.ForEach(t =>
                    {
                        ret.ConnInfoList.Add(new ConnInfo()
                        {
                            Uid = item.Key,
                            ConnId = t
                        });
                    });
                }
            }

            return ret;
        }
    }
}
