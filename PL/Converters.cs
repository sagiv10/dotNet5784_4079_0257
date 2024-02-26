using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

class ConvertPlanningToEnabled : IValueConverter
{
    /// <summary>
    /// converts the planning state to enabled
    /// </summary>
    /// <param name="value"> the state </param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.ProjectStatus)value == BO.ProjectStatus.Planning ? true : false;
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

class ConvertInnerTaskToAlias : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    /// <summary>
    /// if the inner task exists then return his alias, an error message else
    /// </summary>
    /// <param name="value">the id of the task</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            return ((BO.TaskInEngineer)value).Alias;
        }
        else
        {
            return new string("");
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertInnerTaskToId : IValueConverter
{
    /// <summary>
    /// if the task is exists then return his id, an error message else
    /// </summary>
    /// <param name="value">the inner task</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            return ((BO.TaskInEngineer)value).Id.ToString();
        }
        else
        {
            return new string("no task assigned yet");
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
class ConvertTaskToBool : IValueConverter
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
        if (value == null)
            return true;
        else
            return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConvertTaskToBoolDeAssign : IValueConverter
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
        if (value == null)
            return false;
        else
            return true;
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

class ConvertStageToCollapsity : IValueConverter
{
    /// <summary>
    /// if we are at the Execution stage then the button should be visible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Execution)
        {
            return Visibility.Collapsed;    
        }
        return Visibility.Visible;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStageToNotCollapsity : IValueConverter
{
    /// <summary>
    /// if we are at the Execution stage then the button should not be visible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Execution)
        {
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertTaskToContent : IValueConverter
{
    /// <summary>
    /// if the engineer will have no task then the button will be Assign, and if the engineer has task the button will show it
    /// </summary>
    /// <param name="value">the task the engineer has</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.TaskInEngineer)value == null)
        {
            return "Assign Task";
        }
        return "Show Task";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertCollectionToList : IValueConverter
{
    /// <summary>
    /// return the listBox selected values as an list
    /// </summary>
    /// <param name="value">the selected items</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns>list of them</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (IEnumerable<int>)value;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return new List<int>();
        }
        else {
            List<int> addedList = new List<int>();
            addedList.Add((int)value);
            return addedList;
        }
    }
}

class ConvertStageToMessage : IValueConverter
{
    /// <summary>
    /// return the proper nessage
    /// </summary>
    /// <param name="value">the currennt Stage</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns>message</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if((BO.ProjectStatus)value == BO.ProjectStatus.Planning)
        {
            return "Start scheduling tasks";
        }
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Sceduling)
        {
            return "Schedule Task";
        }
        else
        {
            return "nulllllllllll";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStageToVisability : IValueConverter
{
    /// <summary>
    /// if we are at the Execution stage then the button should not be visible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Execution)
        {
            return Visibility.Visible;
        }
        return Visibility.Hidden;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStageToNotVisability : IValueConverter
{
    /// <summary>
    /// if we are at the Execution stage then the button should not be invisible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Execution)
        {
            return Visibility.Hidden;
        }
        return Visibility.Visible;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertTaskToVisability : IValueConverter
{
    /// <summary>
    /// if the engineer does not has a task then the button should not be invisible
    /// </summary>
    /// <param name="value">the task</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Visibility.Hidden;
        }
        return Visibility.Visible;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}





