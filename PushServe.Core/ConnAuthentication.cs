using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Security.Principal;
using PushServe.Util;

namespace PushServe.Core
{
    public class ConnAuthenticationAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            string uid = request.QueryString.Get("uid");

            if (string.IsNullOrWhiteSpace(uid))
            {
                return false;
            }

            HttpRequestItem req = new HttpRequestItem()
            {
                URL = Constant.AuthenUrl,
                Postdata = string.Format("uid={0}", uid)
            };
            string resData = HttpHelp.SendHttpRequest(req);
            if (!string.IsNullOrWhiteSpace(resData))
            {
                HttpResponseItem res = JsonUtil<HttpResponseItem>.Deserialize(resData);
                return res.AckCode == ResultAckCodeEnum.Success && res.ResultCode == 1;
            }

            return base.AuthorizeHubConnection(hubDescriptor, request);
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
