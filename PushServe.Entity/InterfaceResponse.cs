using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Entity
{
    public class InterfaceResponse<T>
    {
        public int ResultCode { get; set; }

        public string ResultMsg { get; set; }

        public T Data { get; set; }

        public long Timespan
        {
            get
            {
                return DateTime.Now.Ticks;
            }
        }
    }
}
