using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushServe.Util
{
    public class ConvertUtil
    {
        public static bool ConvertObjectToBool(object value, bool defaultValue)
        {
            bool res = defaultValue;
            if (value == null || !bool.TryParse(value.ToString(), out res))
            {
                res = defaultValue;
            }

            return res;
        }
    }
}
