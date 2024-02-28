using PL.Engineer;
using PL.Task;
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

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for ganttChart.xaml
    /// </summary>
    public partial class ganttChart : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public List<BO.Task> TasksList
        {
            get { return (List<BO.Task>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TasksListProperty =
        DependencyProperty.Register("TasksList", typeof(List<BO.Task>), typeof(ganttChart), new PropertyMetadata(null));
        public DateTime StartDateOfProject
        {
            get { return (DateTime)GetValue(StartDateOfProjectProperty); }
            set { SetValue(StartDateOfProjectProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateOfProjectProperty =
        DependencyProperty.Register("StartDateOfProject", typeof(DateTime), typeof(ganttChart), new PropertyMetadata(null));
        public ganttChart()
        {
            StartDateOfProject = (DateTime)s_bl.Task.getStartingDate()!;
            //the making of taskList from list of TaskInList:
            List<BO.TaskInList> TasksInList_Tasks = s_bl.Task.ReadAll().ToList();
            TasksList = (from tempTaskInList in TasksInList_Tasks
                        select s_bl.Task.Read(tempTaskInList.Id)).ToList();
            InitializeComponent();
        }

        private void ShowDetailsOfTaskClick(object sender, MouseButtonEventArgs e)
        {
            //Button button = sender as Button;
            //BO.Task SpecificTaskFromList = button?.CommandParameter as BO.Task;
            //new TaskWindow(SpecificTaskFromList.Id);
        }
    }
}
