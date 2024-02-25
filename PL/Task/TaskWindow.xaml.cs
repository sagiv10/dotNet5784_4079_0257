using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public bool Stage; //will be true if we are in create mode and will be false if we are in update mode

    public BO.Task Task
    {
        get { return (BO.Task)GetValue(TaskProperty); }
        set { SetValue(TaskProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    public BO.ProjectStatus Status //so we could track the stage of the project for visual changes
    {
        get { return (BO.ProjectStatus)GetValue(StatusProperty); }
        set { SetValue(StatusProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StatusProperty =
        DependencyProperty.Register("Status", typeof(BO.ProjectStatus), typeof(TaskWindow), new PropertyMetadata(null));

    public List<int> DepensOnTasks
    {
        get { return (List<int>)GetValue(DepensOnTasksProperty); }
        set { SetValue(DepensOnTasksProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DepensOnTasksProperty =
        DependencyProperty.Register("DepensOnTasks", typeof(List<int>), typeof(TaskWindow), new PropertyMetadata(null));

    //so we could show all the potential dependents missions
    public List<int> PotentialTasks
    {
        get { return (List<int>)GetValue(PotentialTasksProperty); }
        set { SetValue(PotentialTasksProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PotentialTasksProperty =
        DependencyProperty.Register("PotentialTasks", typeof(List<int>), typeof(TaskWindow), new PropertyMetadata(null));

    //to save the wanted ids
    public List<int> NewDependsOnTasks
    {
        get { return (List<int>)GetValue(NewDependsOnTasksProperty); }
        set { SetValue(NewDependsOnTasksProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NewDependsOnTasksProperty =
        DependencyProperty.Register("NewDependsOnTasks", typeof(List<int>), typeof(TaskWindow), new PropertyMetadata(null));

    //to save the deleted dependencies
    public List<int> DeletedDependencies
    {
        get { return (List<int>)GetValue(DeletedDependenciesProperty); }
        set { SetValue(DeletedDependenciesProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DeletedDependenciesProperty =
        DependencyProperty.Register("DeletedDependencies", typeof(List<int>), typeof(TaskWindow), new PropertyMetadata(null));

    public List<int> DaysRange
    {
        get { return (List<int>)GetValue(DaysRangeProperty); }
        set { SetValue(DaysRangeProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DaysRangeProperty =
        DependencyProperty.Register("DaysRange", typeof(List<int>), typeof(TaskWindow), new PropertyMetadata(null));

    public List<int> WeeksRange
    {
        get { return (List<int>)GetValue(WeeksRangeProperty); }
        set { SetValue(WeeksRangeProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty WeeksRangeProperty =
        DependencyProperty.Register("WeeksRange", typeof(List<int>), typeof(TaskWindow), new PropertyMetadata(null));

    //to save the deleted dependencies
    public int NumMonths
    {
        get { return (int)GetValue(NumMonthsProperty); }
        set { SetValue(NumMonthsProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NumMonthsProperty =
        DependencyProperty.Register("NumMonths", typeof(int), typeof(TaskWindow), new PropertyMetadata(null));

    //to save the deleted dependencies
    public int NumDays
    {
        get { return (int)GetValue(NumDaysProperty); }
        set { SetValue(NumDaysProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NumDaysProperty =
        DependencyProperty.Register("numDays", typeof(int), typeof(TaskWindow), new PropertyMetadata(null));

    //to save the deleted dependencies
    public int NumWeeks
    {
        get { return (int)GetValue(NumWeeksProperty); }
        set { SetValue(NumWeeksProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NumWeeksProperty =
        DependencyProperty.Register("NumWeeks", typeof(int), typeof(TaskWindow), new PropertyMetadata(null));



    public TaskWindow(int id = 0)
    {
        Status = (BO.ProjectStatus)s_bl.Task.getProjectStatus();
        DaysRange = Enumerable.Range(0, 7).ToList();
        WeeksRange = Enumerable.Range(0, 4).ToList();
        NumDays = 7;
        NumMonths = 0;
        NumWeeks = 0;
        if (id == 0)
        {
            Stage = true;
            Task = new BO.Task(0, "", "", DateTime.Now ,BO.Status.Unscheduled,new List<BO.TaskInList>(),null, TimeSpan.FromDays(7), null,null,null,null,null,"","",null,BO.EngineerExperience.Beginner);
        }
        else
        {
            Stage=false;
            try
            {
                Task = s_bl.Task.Read(id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        DepensOnTasks = Task.Dependencies!.Select(d => d.Id).ToList();

        PotentialTasks = (from BO.TaskInList task in s_bl.Task.ReadAll() //from all the tasks
                          where Task.Dependencies!.FirstOrDefault(d => d.Id == task.Id) == null //take all the tasks that do not depends on our task - they will be potential to add them as a dependens
                          select task.Id).ToList();

        DeletedDependencies = new List<int>();
        NewDependsOnTasks = new List<int>();

        InitializeComponent();
    }

    private void Add_Update_Button(object sender, RoutedEventArgs e)
    {
        if(Stage) //if we are in the adding event
        {
            try
            {
                Task.RequiredEffortTime = TimeSpan.FromDays(NumDays + NumMonths * 30 + NumWeeks * 7);
                int newId = s_bl.Task.Create(Task);

                int ifAdded = s_bl.Task.AddDependencies(NewDependsOnTasks, newId, NewDependsOnTasks.Count);
                if(ifAdded != NewDependsOnTasks.Count) //if not all the dependencies has added, that's means that an error accured and all the proccess failed 
                {
                    s_bl.Task.DeleteDependencies(NewDependsOnTasks, newId, ifAdded); //delete all the dependencies we did added
                    MessageBox.Show("could not add your dependencies due to circular dependency", "Dependencies add problem", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("the task created seccesfully!", "amazing!", MessageBoxButton.OK);
            this.Close();
        }
        else //then we are updating
        {
            int ifAdded = s_bl.Task.AddDependencies(NewDependsOnTasks, Task.Id, NewDependsOnTasks.Count);
            if (ifAdded != NewDependsOnTasks.Count) //if not all the dependencies has added, that's means that an error accured and all the proccess failed 
            {
                s_bl.Task.DeleteDependencies(NewDependsOnTasks, Task.Id, ifAdded);//delete all the dependencies we did added
                MessageBox.Show("could not add your dependencies due to circular dependency", "Dependencies add problem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int ifDeleted = s_bl.Task.DeleteDependencies(DeletedDependencies, Task.Id, DeletedDependencies.Count);
            if(ifDeleted != DeletedDependencies.Count)
            {
                s_bl.Task.DeleteDependencies(NewDependsOnTasks, Task.Id, ifAdded);//delete all the dependencies we did added
                s_bl.Task.AddDependencies(DeletedDependencies, Task.Id, DeletedDependencies.Count); //add all the dependencies we deleted
                MessageBox.Show("could not add your dependencies due to circular dependency", "Dependencies add problem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                s_bl.Task.Update(Task);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); //update our task
                s_bl.Task.DeleteDependencies(NewDependsOnTasks, Task.Id, ifAdded);//delete all the dependencies we did added
                s_bl.Task.AddDependencies(DeletedDependencies, Task.Id, DeletedDependencies.Count); //add all the dependencies we deleted
                return;
            }
            MessageBox.Show("the task updated seccesfully!", " you are amazing!", MessageBoxButton.OK);
            this.Close();
        }
    }



    /// <summary>
    /// helping mathod that helps to artificially bind the selectedItems of an listBox to our list because you cant bind it naturally
    /// </summary>
    /// <param name="sender"> the listBox</param>
    /// <param name="e"></param>
    private void updateListAdd(object sender, SelectionChangedEventArgs e)
    {
        NewDependsOnTasks = ((ListBox)sender).SelectedItems.Cast<int>().ToList();
    }

    /// <summary>
    /// helping mathod that helps to artificially bind the selectedItems of an listBox to our list because you cant bind it naturally
    /// </summary>
    /// <param name="sender"> the listBox</param>
    /// <param name="e"></param>
    private void updateListDelete(object sender, SelectionChangedEventArgs e)
    {
        DeletedDependencies = ((ListBox)sender).SelectedItems.Cast<int>().ToList();
    }
}
