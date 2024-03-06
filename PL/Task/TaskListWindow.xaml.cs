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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //------------------------------------------------------------------------------------
        //Dependency Properties:
        public BO.Status chosenStatus
        {
            get { return (BO.Status)GetValue(chosenStatusProperty); }
            set { SetValue(chosenStatusProperty, value); }
        }
        public static readonly DependencyProperty chosenStatusProperty/*how to call me in the xaml code */ =
        DependencyProperty.Register("chosenStatus", typeof(BO.Status), typeof(TaskListWindow), new PropertyMetadata(null));

        /// <summary>
        /// the stage of the project property
        /// </summary>
        public BO.ProjectStatus Status
        {
            get { return (BO.ProjectStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty/*how to call me in the xaml code */ =
        DependencyProperty.Register("Status", typeof(BO.ProjectStatus), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience chosenComplexity
        {
            get { return (BO.EngineerExperience)GetValue(chosenComplexityProperty); }
            set { SetValue(chosenComplexityProperty, value); }
        }
        public static readonly DependencyProperty chosenComplexityProperty/*how to call me in the xaml code */ =
        DependencyProperty.Register("chosenComplexity", typeof(BO.EngineerExperience), typeof(TaskListWindow), new PropertyMetadata(null));
        
        public IEnumerable<BO.TaskInList> TaskInList_List
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskInList_ListProperty); }
            set { SetValue(TaskInList_ListProperty, value); }
        }
        public static readonly DependencyProperty TaskInList_ListProperty/*how to call me in the xaml code */ =
        DependencyProperty.Register("TaskInList_List", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
        
        //-------------------------------------------------------------------------------------
        public TaskListWindow()
        {
            try
            {
                chosenComplexity = EngineerExperience.All;
                chosenStatus = BO.Status.All;
                Status = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
                TaskInList_List = s_bl.Task.ReadAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void ShowSpecificTask(object sender, MouseButtonEventArgs e)//sender has the details of which task we been sent from. 
        {
            try
            {
                BO.TaskInList? SpecificTaskFromList = (sender as ListView)!.SelectedItem as BO.TaskInList; //get inside "SpecificTaskFromList" the details from the sender about which task we clicked on.
                if (SpecificTaskFromList == null)
                    return;
                new TaskWindow(true, SpecificTaskFromList!.Id).ShowDialog();//open the window of showing specifc task with the details from last line.
                TaskInList_List = s_bl?.Task.ReadAll(e => (e.Status == chosenStatus || chosenStatus == BO.Status.All) && (e.Complexity == chosenComplexity || chosenComplexity == BO.EngineerExperience.All))!;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReadListAgain(object sender, SelectionChangedEventArgs e)//this method happans after everytime we change the combobox. (to update the view of the shown list) 
        {
            try
            {
                IEnumerable<BO.TaskInList> tempList = s_bl?.Task.ReadAll(e => (e.Status == chosenStatus || chosenStatus == BO.Status.All) && (e.Complexity == chosenComplexity || chosenComplexity == BO.EngineerExperience.All))!;
                TaskInList_List = tempList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new TaskWindow(true, 0).ShowDialog();//open the window of showing specifc task without details. (because we want to add new task)
                TaskInList_List = s_bl.Task.ReadAll(e => (e.Status == chosenStatus || chosenStatus == BO.Status.All) && (e.Complexity == chosenComplexity || chosenComplexity == BO.EngineerExperience.All));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void addDependencyClick(object sender, RoutedEventArgs e) 
        {
            try
            {
                if (TaskInList_List.Count() == 0) //if there is no tasks in the database
                {
                    MessageBox.Show("there is no tasks to create dependencies for!", "no task error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    new AddRemoveDependency(true/*indicates we are in add mode*/).ShowDialog();//open the window of the add of dependency
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void removeDependencyClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskInList_List.Count() == 0) //if there is no tasks in the database
                {
                    MessageBox.Show("there is no tasks to delete their dependencies!", "no task error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    new AddRemoveDependency(false/*indicates we are in remove mode*/).ShowDialog();//open the window of the remove of dependency
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //delete an task in case the user pressed on his button
        private void DeleteTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Delete(((BO.TaskInList)((Button)sender).CommandParameter).Id); //delete him
                MessageBox.Show("Task seccessfully deleted!");
                TaskInList_List = s_bl?.Task.ReadAll(e => (e.Status == chosenStatus || chosenStatus == BO.Status.All) && (e.Complexity == chosenComplexity || chosenComplexity == BO.EngineerExperience.All))!;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
