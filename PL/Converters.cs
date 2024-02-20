using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PL;

class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConvertBoolToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "Add" : "Remove";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


}

class ConvertIdToAlias : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    /// <summary>
    /// if the task id is exists then return his alias, an error message else
    /// </summary>
    /// <param name="value">the id of the task</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            BO.Task theTask = s_bl.Task.Read((int)value);
            return theTask.Alias;
        }
        catch(BO.BLNotFoundException)
        {
            return new string("no task is maching this id");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToBool : IValueConverter
{
    /// <summary>
    /// returns true if the id is 0 and false else
    /// </summary>
    /// <param name="value">the id</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


}

class ConvertTaskInEngineerToIdString : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    /// <summary>
    /// if his task is exists then return his alias, an error message else
    /// </summary>
    /// <param name="value">the TaskInEngineer </param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value == null)
        {
            return "none."; //he has no task
        }
        try
        {
            BO.Task theTask = s_bl.Task.Read((int)value);
            return theTask.Alias;
        }
        catch (BO.BLNotFoundException)
        {
            return new string("no task is maching this id");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStringToWelcomeString : IValueConverter
{
    /// <summary>
    /// put an welcome before the engineer's name
    /// </summary>
    /// <param name="value">the Engineer name </param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return "Welcome " + (string)value + "!";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIntToString : IValueConverter
{
    /// <summary>
    /// return int instead of string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int num;
        return int.TryParse((String)value, out num) ? num : 0;
    }
    /// <summary>
    /// make an int string
    /// </summary>
    /// <param name="value">the int </param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((int)value).ToString();
    }
    
}


