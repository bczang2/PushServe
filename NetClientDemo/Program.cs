using Microsoft.AspNet.SignalR.Client;
using PushServe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var hub = new HubConnection("http://localhost/PushServe/signalr");
            var proxy = hub.CreateHubProxy("pushServeHub");
            hub.Error += (ex) => { Console.WriteLine("conn error!" + ex.Message); };
            hub.Start().Wait();
            if (hub.State == ConnectionState.Connected)
            {
                proxy.On("push", (resMsg) =>
                {
                    Console.WriteLine("服务返回：" + resMsg);
                });

                NewPushMsgPack data = new NewPushMsgPack()
                {
                    uid = "100001",
                    resdata = "response data\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                ClientMsgPackEntity msg = new ClientMsgPackEntity()
                {
                    mp_type = MpTypeEnum.NewMsgPush,
                    mp_cotent = data
                };

                proxy.Invoke("send", msg).Wait();
            }

            Console.ReadKey();
        }
    }
}
