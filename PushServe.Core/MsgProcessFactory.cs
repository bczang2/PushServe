using PushServe.Core;
using PushServe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushServe.Core
{
    public class MsgProcessFactory
    {
        public static IMsgProcess GetInstance(MpTypeEnum type)
        {
            IMsgProcess instance = null;
            switch (type)
            {
                case MpTypeEnum.ClientConn:
                    instance = new ClientConnMsgProcessImpl();
                    break;
                case MpTypeEnum.NewMsgPush:
                    instance = new NewMsgPushProcessImpl();
                    break;
                case MpTypeEnum.GloableMsg:
                    instance = new GloableMsgProcessImpl();
                    break;
            }
            return instance;
        }
    }
}
