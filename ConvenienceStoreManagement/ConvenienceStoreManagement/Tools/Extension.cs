using System.Collections.Generic;

namespace ConvenienceStoreManagement.Tools
{
    public static class Extension
    {
        public static Dictionary<string, object?> ToSingle(this Dictionary<string, object?> inDict)
        {
            if (inDict["data"] != null)
            {
                List<Dictionary<string, object>> data = (List<Dictionary<string, object>>)inDict["data"];
                inDict["data"] = data[0];
            }
            return inDict;
        }
    }
}
