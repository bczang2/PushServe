﻿using System;

namespace PushServe.Entity.Http
{
    public class HttpResponseItem
    {
        /// <summary>
        /// 返回code
        /// </summary>
        public ResultAckCodeEnum AckCode { get; set; }

        /// <summary>
        /// 返回业务状态码
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string ResultMsg { get; set; }
    }
}
