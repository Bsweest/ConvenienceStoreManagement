using System;

namespace ConvenienceStoreManagement.Tools
{
    public static class Utils
    {
        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyymmdd");
        }
    }
}
