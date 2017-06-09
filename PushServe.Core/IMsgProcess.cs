using Microsoft.AspNet.SignalR.Hubs;
using PushServe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushServe.Core
{
    public interface IMsgProcess
    {
        void MsgProcess(ClientMsgPackEntity value, IHubCallerConnectionContext<dynamic> clients = null, string connectionId = "");
    }
}
