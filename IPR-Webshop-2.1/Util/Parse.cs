using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    class Parse
    {
        public static byte[] SerializeData(string tag, dynamic data)
        {
            dynamic command = new
            {
                tag = tag,
                data = data
            };
            return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(command)); ;
        }
    }
}
