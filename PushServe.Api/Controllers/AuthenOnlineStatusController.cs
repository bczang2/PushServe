using PushServe.Core;
using PushServe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PushServe.Api.Controllers
{
    public class AuthenOnlineStatusController : ApiController
    {
        /// <summary>
        /// 获取用户在线状态
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        [HttpPost]
        public InterfaceResponse<List<UidAuthenResult>> AuthenUid(List<string> uids)
        {
            InterfaceResponse<List<UidAuthenResult>> ret = new InterfaceResponse<List<UidAuthenResult>>();
            try
            {
                if (uids == null || !uids.Any())
                {
                    ret.ResultMsg = "参数非法";
                }
                else
                {
                    ApiCore api = new ApiCore();
                    ret.Data = api.AuthenUid(uids);
                }
            }
            catch (Exception ex)
            {
                //log
            }

            return ret;
        }
    }
}
