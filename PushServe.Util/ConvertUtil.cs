using System;

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

        public static int ConvertStringToInt32(string value, int defaultValue)
        {
            int res = defaultValue;
            if (string.IsNullOrWhiteSpace(value) || !Int32.TryParse(value, out res))
            {
                res = defaultValue;
            }

            return res;
        }
    }
}
