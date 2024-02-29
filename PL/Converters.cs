using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

class ConvertTimeSpanToNumDays : IValueConverter
{
    /// <summary>
    /// converts the number of days of timeSpan to string describe its amount of days
    /// </summary>
    /// <param name="value"> the TimeSpan </param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (((TimeSpan)value).Days).ToString() + " days.";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class DateToStringConverter : IValueConverter
{
    /// <summary>
    /// converts the number of days of timeSpan to string describe its amount of days
    /// </summary>
    /// <param name="value"> the TimeSpan </param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((DateTime)value).ToString("yyyy-MM-dd");
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


class ConvertMultiplyToWidth : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value*10;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConvertMultiplyAndFindStartTime : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime startDate = (DateTime)s_bl.Task.getStartingDate()!;
        DateTime currentScheduled = (DateTime)value;
        return ((currentScheduled-startDate).Days*10)!;//value=scheduledTime;
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
        return int.TryParse((string)value, out num) ? num : 0;
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

class ConvertExecutionToCollapsity : IValueConverter
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

class ConvertExecutionToNotCollapsity : IValueConverter
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

class ConvertExecutionToVisability : IValueConverter
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

class ConvertPlanningToVisability : IValueConverter
{
    /// <summary>
    /// if we are at the Planning stage then the button should not be visible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Planning)
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

class ConvertScheduleToVisability : IValueConverter
{
    /// <summary>
    /// if we are at the Schedule stage then the button should not be visible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Sceduling)
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

class ConvertExecutionToNotVisability : IValueConverter
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
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Sceduling)
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

class ConvertPlanningToNotVisability : IValueConverter
{
    /// <summary>
    /// if we are at the Planning stage then the button should not be invisible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Planning)
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

class ConvertScheduleToNotVisability : IValueConverter
{
    /// <summary>
    /// if we are at the Schedule stage then the button should not be invisible
    /// </summary>
    /// <param name="value">the status</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.ProjectStatus)value == BO.ProjectStatus.Sceduling)
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

class ConvertTaskToColor : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    /// <summary>
    /// get the right color to each task
    /// </summary>
    /// <param name="value">the id of the task</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns> the color </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double precent = s_bl.Task.GetPrecentage((int)value);
        if(precent < 0) //hasn't started yet
        {
            return Brushes.Cyan; //color of not started yet
        }
        if(0 <= precent && precent <0.33) { //on the first third

            return Brushes.Green; //color of on track
        }
        if (0.33 <= precent && precent < 0.66) //on the second third
        {
            return Brushes.Yellow; //color of getting dangerous
        }
        if (0.66 <= precent && precent < 1) //on the third third
        {
            return Brushes.Orange; //color of dangerous
        }
        if(precent == 1) //way out of track
        {
            return Brushes.Red; //color of to late
        }
        return Brushes.Magenta; //color of to late
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class ConvertDateToMargin : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    /// <summary>
    /// get the right Margin for each task
    /// </summary>
    /// <param name="value">the time should be bounded to the margin</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns> the margin </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime startDate = (DateTime)s_bl.Task.getStartingDate()!;
        DateTime? theDate = (DateTime?)value; //the time is not null because we are in the execution stage
        if (theDate == null)
        {
            return new Thickness(0, 0, 0, 0);
        }
        else //then there is a finish date
        {
            int numDays = ((DateTime)theDate - startDate).Days + 2;
            return new Thickness(numDays * 10, 0, 0, 0); //so the relate to the witdh will be 10/10
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertNotNullToVisibility : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    /// <summary>
    /// if the item is null then the item will not be visible
    /// </summary>
    /// <param name="value">the time should be bounded to the margin</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns> bool </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value == null)
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
