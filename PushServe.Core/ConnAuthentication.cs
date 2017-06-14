using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Security.Principal;
using PushServe.Util;
using PushServe.Entity.Http;
using System;

namespace PushServe.Core
{
    public class ConnAuthenticationAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            bool flag = false;
            string uid = request.QueryString.Get("uid");
            string token = request.QueryString.Get("token");

            if (string.IsNullOrWhiteSpace(uid))
            {
                return flag;
            }
            //token 验证
            if (!string.IsNullOrWhiteSpace(token) && Tool.MD5Check(uid, token))
            {
                flag = true;
            }
            else
            {
                //接口验证
                HttpRequestItem req = new HttpRequestItem()
                {
                    URL = Constant.AuthenUrl,
                    Postdata = string.Format("uid={0}", uid)
                };
                string resData = HttpHelp.SendHttpRequest(req);
                if (!string.IsNullOrWhiteSpace(resData))
                {
                    HttpResponseItem res = JsonUtil<HttpResponseItem>.Deserialize(resData);
                    flag = res.AckCode == ResultAckCodeEnum.Success && res.ResultCode == 1;
                }
            }

            //return flag ? base.AuthorizeHubConnection(hubDescriptor, request) : false;
            return true;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            return base.AuthorizeHubMethodInvocation(hubIncomingInvokerContext, appliesToMethod);
        }

        protected override bool UserAuthorized(IPrincipal user)
        {
            return true;
        }
    }
}
