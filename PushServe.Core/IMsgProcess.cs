using Microsoft.AspNet.SignalR.Hubs;
using PushServe.Entity;
using System;

namespace PushServe.Core
{
    public interface IMsgProcess
    {
        void MsgProcess(ClientMsgPackEntity value, IHubCallerConnectionContext<dynamic> clients = null, string connectionId = "");
    }
}
