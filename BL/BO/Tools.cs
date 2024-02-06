using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;


namespace BO
{

    internal static class Tools
    {
        /// <summary>
        /// generic helping method to each ToString
        /// </summary>
        /// <typeparam name="T"> the object itself </typeparam>
        /// <param name="obj"></param>
        /// <returns> string that describes the object </returns>
        public static string ToStringProperty<T>(this T obj){
            string finalString = "";
            Type type = obj!.GetType();
            var properties = type.GetProperties();
            foreach( var property in properties)
            {
                if (property.GetType() == typeof(String)) //if our property is a string
                {
                    finalString += property.Name + ": " + property.GetValue(obj) + "\n";
                }
                else
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType.IsGenericType) //if our property is an ienumerable
                    {
                        finalString += property.Name + ": " + "\n";
                        foreach (var propertyValue in (IEnumerable)property.GetValue(obj)!)
                        {
                            finalString += propertyValue +"\n";
                        }

                }
                    else
                    {
                        if(property.GetValue(obj) != null)
                        {
                            finalString += property.Name + ": " + property.GetValue(obj) + "\n";
                        }
                    }
                }
            }
            return finalString;
        }

    }
}
