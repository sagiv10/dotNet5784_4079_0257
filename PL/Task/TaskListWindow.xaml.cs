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
        public IEnumerable<BO.TaskInList> TaskInList_List
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskInList_ListProperty); }
            set { SetValue(TaskInList_ListProperty, value); }
        }

        public static readonly DependencyProperty TaskInList_ListProperty/*how to call me in the xaml code */ =
            DependencyProperty.Register("TaskInList_ListProperty", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
        public TaskListWindow()
        {
            InitializeComponent();
            TaskInList_List = s_bl?.Task.ReadAll()!;
        }
    }
}
