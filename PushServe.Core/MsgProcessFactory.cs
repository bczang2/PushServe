using PushServe.Entity;
using System.Collections.Concurrent;
using System;

namespace PushServe.Core
{
    public class MsgProcessFactory
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static ConcurrentDictionary<MpTypeEnum, IMsgProcess> _instanceDic = new ConcurrentDictionary<MpTypeEnum, IMsgProcess>();

        public static IMsgProcess GetInstance(MpTypeEnum type)
        {
            IMsgProcess instance = null;
            if (_instanceDic.ContainsKey(type))
            {
                instance = _instanceDic[type];
            }
            else
            {
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

                if (!_instanceDic.ContainsKey(type))
                {
                    _instanceDic.TryAdd(type, instance);
                }
            }

            return instance;
        }
    }
}
