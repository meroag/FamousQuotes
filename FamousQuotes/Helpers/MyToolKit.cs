using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FamousQuotes.Helpers
{
    public static class MyToolKit
    {
        public static void CopyModel(object source, object destination)
        {
            foreach (PropertyInfo propertyInfo in source.GetType().GetProperties())
            {
                var newValue = propertyInfo.GetValue(source);
                propertyInfo.SetValue(destination,newValue);
            }
        }

        public static Uri GetBaseUrl(HttpRequest request)
        {
            return new Uri($"{request.Scheme}://{request.Host}");
        }
    }
}
