using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PushServe.Util
{

    public class ConfigUtils
    {
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="configName">配置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetConfig(string configName, string defaultValue)
        {
            return ConfigurationManager.AppSettings[configName] == null ? defaultValue : ConfigurationManager.AppSettings[configName];
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="configName">配置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetConfig(string configName, int defaultValue)
        {
            int result = defaultValue;
            if (ConfigurationManager.AppSettings[configName] != null)
            {
                if (!Int32.TryParse(ConfigurationManager.AppSettings[configName], out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="configName">配置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime GetConfig(string configName, DateTime defaultValue)
        {

            DateTime result = defaultValue;
            if (ConfigurationManager.AppSettings[configName] != null)
            {
                if (!DateTime.TryParse(ConfigurationManager.AppSettings[configName], out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="configName">配置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetConfig(string configName, bool defaultValue)
        {
            return ConfigurationManager.AppSettings[configName] == null ? defaultValue : ((ConfigurationManager.AppSettings[configName].ToLower() == "true" || ConfigurationManager.AppSettings[configName] == "1") ? true : false);
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="configName">配置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static double GetConfig(string configName, double defaultValue)
        {
            double result = defaultValue;
            if (ConfigurationManager.AppSettings[configName] != null)
            {
                if (!Double.TryParse(ConfigurationManager.AppSettings[configName], out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
    }
}