using PushServe.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PushServe
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var temp = DashboardHelp.QueryViewInfo();
            if (temp != null)
            {
                usercount.InnerText = temp.ConnNum.ToString();
                linkcount.InnerText = temp.ConnInfoList.Count.ToString();
                lastertime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                clientList.DataSource = temp.ConnInfoList;
                clientList.DataBind();
            }
        }
    }
}