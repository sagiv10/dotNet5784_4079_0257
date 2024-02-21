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


        public TaskWindow(int id = 0)
        {
            if(id == 0)
            {
                Stage = true;
                Task = new BO.Task(0, "", "", DateTime.Now ,BO.Status.Unscheduled,null,null, TimeSpan.FromDays(7), null,null,null,null,null,"","",null,BO.EngineerExperience.Beginner);
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
            InitializeComponent();
        }
    }
}
