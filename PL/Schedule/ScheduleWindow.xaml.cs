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

namespace PL.Schedule
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //the list of the id of the unscheduled tasks:
        public List<int> Tasks
        {
            get { return (List<int>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register("Tasks", typeof(List<int>), typeof(ScheduleWindow), new PropertyMetadata(null));
        public ScheduleWindow()
        {
            Tasks = (from BO.TaskInList task in s_bl.Task.ReadAll(t => t.ScheduledDate == null)
                     select task.Id).ToList();
            InitializeComponent();
        }

        private void ScheduleOne(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.CheckPreviousTasks((int)((Button)sender).CommandParameter);
                new ScheduleOneTaskWindow((int)((Button)sender).CommandParameter).ShowDialog();
                Tasks = (from BO.TaskInList task in s_bl.Task.ReadAll(t => t.ScheduledDate == null)
                         select task.Id).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void AutoScheduleAll(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("this action will schedule automatically all your tasks, even those you alredy scheduled. proceed anyway?", "auto schedule confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                s_bl.Task.AutoScedule();
                this.Close();
            }
        }
    }
}
