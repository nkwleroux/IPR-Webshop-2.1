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
        public static dynamic getCredentialDynamic(string username, string password, bool isEditor)
        {
            return new
            {
                username,
                password,
                isEditor
            };
        }

        public static dynamic getProductListRequest(string category)
        {
            return new
            {
                category
            };
        }
        public static dynamic getUserListRequest()
        {
            return new
            {
                //todo criteria maybe
            };
        }
        public static dynamic getProductChangeDynamic(string typeOfChange, Product product)
        {
            return new
            {
                typeOfChange,
                product
            };
        }
        public static dynamic getUserChangeDynamic(string typeOfChange, User user)
        {
            return new
            {
                typeOfChange,
                user
            };
        }

        public static dynamic getUserChangeDynamic(User user)
        {
            return new
            {
                user
            };
        }
    }
}
