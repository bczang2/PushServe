﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Entity
{
    public class ClientViewInfo
    {
        public ClientViewInfo()
        {
            ConnInfoList = new List<ConnInfo>();
        }

        public long ConnNum { get; set; }

        public List<ConnInfo> ConnInfoList { get; set; }
    }

    public class ConnInfo
    {
        public string Uid { get; set; }

        public string ConnId { get; set; }
    }
}
