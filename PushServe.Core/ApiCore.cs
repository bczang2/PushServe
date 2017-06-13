using PushServe.Entity;
using PushServe.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Core
{
    public class ApiCore
    {

        public List<UidAuthenResult> AuthenUid(List<string> uids)
        {
            List<UidAuthenResult> ret = new List<UidAuthenResult>();
            if (uids != null && uids.Any())
            {
                var dic = RedisHelper.Hash_Exist_Batch(Constant.RedisClusterConn, Constant.ClientConnKey, uids);
                if (dic != null && dic.Any())
                {
                    foreach (var item in dic)
                    {
                        ret.Add(new UidAuthenResult()
                        {
                            Uid = item.Key,
                            OnlineStatus = item.Value
                        });
                    }
                }
            }

            return ret;
        }
    }
}
