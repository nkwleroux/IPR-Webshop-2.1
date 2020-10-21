using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class DataProtocol
    {
        public static string getJsonMessage(string type, dynamic data)
        {
            dynamic message = new
            {
                type,
                data
            };
            return JsonConvert.SerializeObject(message);
        }
        
        public static ProductSerializable MakeSerializableProduct(Product product)
        {
            return new ProductSerializable(product);
        }
    }
}
