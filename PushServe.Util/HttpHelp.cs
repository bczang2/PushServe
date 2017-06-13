using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Util
{
    public class HttpHelp
    {
        public static string SendHttpRequest(HttpRequestItem item)
        {
            string result = string.Empty;
            switch (item.Method.ToLower())
            {
                case "get":
                    result = HttpGet(item);
                    break;
                case "post":
                    result = HttpPost(item);
                    break;
                default:
                    result = HttpPost(item);
                    break;
            }

            return result;
        }


        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string HttpPost(HttpRequestItem item)
        {
            Stream respStream = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.URL);
                request.Method = item.Method;
                request.ContentType = item.ContentType;
                request.Timeout = item.Timeout;

                byte[] dataBuffer = item.Encoding.GetBytes(item.Postdata);
                request.ContentLength = dataBuffer.Length;
                request.GetRequestStream().Write(dataBuffer, 0, dataBuffer.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    respStream = response.GetResponseStream();
                    streamReader = new StreamReader(respStream, item.Encoding); ;
                    var result = streamReader.ReadToEnd();

                    return result;
                }
            }
            catch (System.Exception ex)
            {
                //LogUtility.Error("HttpPost异常", ex);
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                if (respStream != null)
                {
                    respStream.Close();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// httpGet
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string HttpGet(HttpRequestItem item)
        {
            Stream respStream = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.URL);
                request.Method = item.Method;
                request.ContentType = item.ContentType;
                request.Timeout = item.Timeout;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    respStream = response.GetResponseStream();
                    streamReader  = new StreamReader(respStream, item.Encoding); ;
                    var result = streamReader.ReadToEnd();

                    return result;
                }
            }
            catch (System.Exception ex)
            {
                //LogUtility.Error("HttpGet异常", ex);
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                if(respStream != null)
                {
                    respStream.Close();
                }
            }

            return string.Empty;
        }
    }
}
