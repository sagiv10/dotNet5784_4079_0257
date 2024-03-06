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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL.Schedule
{
    /// <summary>
    /// Interaction logic for ScheduleOneTaskWindow.xaml
    /// </summary>
    public partial class ScheduleOneTaskWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //our current task's id:
        public int Task
        {
            get { return (int)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(int), typeof(ScheduleOneTaskWindow), new PropertyMetadata(null));

        //the optional date to the task:
        public DateTime FirstOptionalDate
        {
            get { return (DateTime)GetValue(FirstOptionalDateProperty); }
            set { SetValue(FirstOptionalDateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstOptionalDateProperty =
            DependencyProperty.Register("FirstOptionalDate", typeof(DateTime), typeof(ScheduleOneTaskWindow), new PropertyMetadata(null));

        //the chosen date to the task:
        public DateTime ChosenDate
        {
            get { return (DateTime)GetValue(ChosenDateProperty); }
            set { SetValue(ChosenDateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChosenDateProperty =
            DependencyProperty.Register("ChosenDate", typeof(DateTime), typeof(ScheduleOneTaskWindow), new PropertyMetadata(null));



        public ScheduleOneTaskWindow(int taskId)
        {
            try
            {
                Task = taskId;
                FirstOptionalDate = s_bl.Task.findOptionalDate(taskId);
                ChosenDate = FirstOptionalDate;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void ScheduleOneTask(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.ManualScedule(Task, ChosenDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();

        }
    }
}
