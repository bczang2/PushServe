using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PushServe.Entity;
using PushServe.Core;

namespace PushServe.Hubs
{
    [ConnAuthentication]
    public class PushServeHub : Hub
    {
        public void Send(ClientMsgPackEntity pack)
        {
            if (pack != null)
            {
                IMsgProcess msgOp = MsgProcessFactory.GetInstance(pack.mp_type);
                msgOp.MsgProcess(pack, Clients, this.Context.ConnectionId);
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            ClientDisconnHelp.DisconnectedOp(this.Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}