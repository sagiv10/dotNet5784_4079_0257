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
    /// Interaction logic for TaskListWindowSpecifcEngineer.xaml
    /// </summary>
    public partial class TaskListWindowSpecifcEngineer : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int idOfThisEngineer { get; set; }
        public IEnumerable<BO.TaskInList> TaskInList_List
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskInList_ListProperty); }
            set { SetValue(TaskInList_ListProperty, value); }
        }
        public static readonly DependencyProperty TaskInList_ListProperty/*how to call me in the xaml code */ =
        DependencyProperty.Register("TaskInList_ListProperty", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindowSpecifcEngineer), new PropertyMetadata(null));
        public bool IsNotTaken
        {
            get { return (bool)GetValue(IsTakenProperty); }
            set { SetValue(IsTakenProperty, value); }
        }
        public static readonly DependencyProperty IsTakenProperty/*how to call me in the xaml code */ =
        DependencyProperty.Register("IsTakenProperty", typeof(bool), typeof(TaskListWindowSpecifcEngineer), new PropertyMetadata(true));
        public TaskListWindowSpecifcEngineer(int _id)
        {
            try
            {
                TaskInList_List = s_bl.Engineer.GetPotentialTasks(_id);
                IsNotTaken = (s_bl.Engineer.ReadEngineer(_id).Task == null);
                idOfThisEngineer = _id;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AssignMeToThisTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                BO.TaskInList SpecificTaskFromList = button?.CommandParameter as BO.TaskInList;
                if (SpecificTaskFromList != null)
                {
                    s_bl.Engineer.AssignTask(idOfThisEngineer, SpecificTaskFromList.Id);
                    IsNotTaken = false;
                    TaskInList_List = s_bl.Engineer.GetPotentialTasks(idOfThisEngineer);
                    MessageBox.Show($"assign of engineer with id:{idOfThisEngineer} to task:{SpecificTaskFromList.Alias} has succeeded!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
