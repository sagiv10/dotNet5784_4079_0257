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
        /// <summary>
        /// generic helping method to each ToString
        /// </summary>
        /// <typeparam name="T"> the object itself </typeparam>
        /// <param name="obj"></param>
        /// <returns> string that describes the object </returns>
        public static string ToStringProperty<T>(this T obj)
        {
            string finalString = "";
            Type type = obj!.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.GetType() == typeof(String)) //if our property is a string
                {
                    finalString += property.Name + ": " + property.GetValue(obj) + ".\n";
                }
                else
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType.IsGenericType) //if our property is an ienumerable
                    {
                        finalString += property.Name + ": ";
                        int counter = 0;
                        foreach (var propertyValue in (IEnumerable)property.GetValue(obj)!) //count how many items are in the collection
                        {
                            counter++;
                        }
                        if(counter == 0) //then there is no items in the collection
                        {
                            finalString += "none\n";
                        }
                        else
                        {
                            finalString+="{\n";
                            foreach (var propertyValue in (IEnumerable)property.GetValue(obj)!)
                            {
                                finalString += propertyValue + "\n";
                            }
                            finalString = finalString.Substring(0, finalString.Length - 1);
                            finalString += "}\n";
                        }
                    }
                    else
                    {
                        if (property.GetValue(obj) != null)
                        {
                            finalString += property.Name + ": " + property.GetValue(obj) + "\n";
                        }
                        else
                        {
                            finalString += property.Name + ": " + "none\n";
                        }
                    }
                }
            }
            return finalString;
        }
        //internal static class Tools
        //{
        //    public static string ToStringProperty<T>(this T obj)
        //    {
        //        Type type = obj.GetType();
        //        var members = type.GetProperties();
        //        string result = "";
        //        foreach (var member in members)
        //        {
        //            if (member.GetValue(obj) == null)
        //            {
        //                result += "\n";
        //            }  
        //            else if (member.GetType() is System.Collections.IEnumerable IENUM)
        //            {
        //                result += IENUM.ToStringProperty();
        //            }
        //            else//(!(member.GetType() is IEnumerable) && member != null)//if thats not a deep variable 
        //            {
        //                var value = member.GetValue(obj);
        //                result += $"{member.Name}:{value} \n";
        //            }
        //        }
        //        return result;
        //    }
    }
}

//    internal static class Tools
//    {
//        public static string ToStringProperty<T>(this T obj)
//        {
//            Type type = obj.GetType();
//            var members = type.GetProperties();
//            string result = "";
//            foreach (var member in members)
//            {
//                if (member.GetValue(obj) == null)
//                {
//                    result += "\n";
//                }  
//                else if (member.GetValue(obj)!.GetType() is System.Collections.IEnumerable IENUM)
//                {
//                    result += IENUM.ToStringProperty();
//                }
//                else//(!(member.GetType() is IEnumerable) && member != null)//if thats not a deep variable 
//                {
//                    var value = member.GetValue(obj);
//                    result += $"{member.Name}:{value} \n";
//                }
//            }
//            return result;
//        }
//    }
//}

