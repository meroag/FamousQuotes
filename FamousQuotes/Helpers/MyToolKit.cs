using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FamousQuotes.Helpers
{
    public static class MyToolKit
    {
        public static void CopyModel(object source, object destination)
        {
            foreach (PropertyInfo propertyInfo in source.GetType().GetProperties())
            {
                var newValue = propertyInfo.GetValue(source);
                var oldValue = propertyInfo.GetValue(destination);
                if(oldValue == null || oldValue.Equals(newValue))
                    propertyInfo.SetValue(destination,newValue);
            }
        }
    }
}
