using System;

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
