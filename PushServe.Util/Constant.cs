using System;

namespace PushServe.Util
{
    public class Constant
    {
        /// <summary>
        /// conn
        /// </summary>
        public static string RedisClusterConn = ConfigUtils.GetConfig("RedisClusterConn", "127.0.0.1:6379");

        public const string ClientConnKey = "push_serve_clientconn";

        public const string ClientInfoKey = "push_serve_clientinfo";
        
        /// <summary>
        /// 接口验证url
        /// </summary>
        public static readonly string AuthenUrl = ConfigUtils.GetConfig("AuthenUrl", "http://localhost/PushServeApi/api/ThirdAuthen/AuthenUser/");

        public static readonly string TokenKeyt = ConfigUtils.GetConfig("TokenKeyt", "^&CEsnP0BGeYqJ&6FU!*ES!tA7^QW#");
    }
}
