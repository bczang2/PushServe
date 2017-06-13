﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Util
{
    public enum ResultAckCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 警告
        /// </summary>
        Waring = 300,

        /// <summary>
        /// 异常
        /// </summary>
        Exception = 400,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 500
    }
}
