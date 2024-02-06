using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    internal static class Tools
    {
        public static string  toStringProperty <T>(this T obj)
        {
            Type type = obj.GetType();
            var members = type.GetProperties();
            string result = "";
            foreach (var member in members)
            {
                if(member==null)
                {
                    result += "";
                }
                else if(!(member.GetType() is IEnumerable) && member!= null)//if thats not a deep variable 
                {
                    var value= member.GetValue(obj);
                    result += $"{member}:{value} \n";
                }
                else if(member.GetType() is IEnumerable IENUM )
                {
                    result += IENUM.toStringProperty();
                }
            }
            return result;
        }
    }
}
