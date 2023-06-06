using System.Collections.Generic;

namespace ConvenienceStoreManagement.Tools
{
    public static class Extension
    {
        public static Dictionary<string, object?> ToSingle(this Dictionary<string, object?> inDict)
        {
            if (inDict["data"] != null)
            {
                if (inDict["data"] is Dictionary<string, object>) return inDict;
                if (inDict["data"] is List<Dictionary<string, object>> listData)
                {

                    inDict["data"] = listData[0];
                }
            }
            return inDict;
        }
    }
}
