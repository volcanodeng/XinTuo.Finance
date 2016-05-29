using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using XinTuo.Finance.Models;
using System.Data;

namespace XinTuo.Finance.Services
{
    public static class Utility
    {
        public static T Convert<D,T>(D source)
        {
            string json = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> Convert<T>(DataTable dt)
        {
            string json = JsonConvert.SerializeObject(dt);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }


    }
}