using PushServe.Entity.Http;
using PushServe.Util;
using System.Web.Http;
using System;
using System.Web;
using PushServe.Entity;

namespace PushServe.Api.Controllers
{
    public class ThirdAuthenController : ApiController
    {
        [HttpPost]
        [HttpGet]
        public HttpResponseItem AuthenUser()
        {
            string uid = HttpContext.Current.Request["uid"];
            HttpResponseItem ret = new HttpResponseItem()
            {
                AckCode = ResultAckCodeEnum.Success
            };

            if (ConvertUtil.ConvertStringToInt32(uid, 0) % 2 == 0)
            {
                ret.ResultCode = 1;
            }
            else
            {
                ret.ResultCode = 0;
                ret.ResultMsg = "user offline-status";
            }

            return ret;
        }
    }
}
