using PL.Engineer;
using PL.Schedule;
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
    /// Interaction logic for ManagerChoose.xaml
    /// </summary>
    public partial class ManagerChoose : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.ProjectStatus Stage
        {
            get { return (BO.ProjectStatus)GetValue(StageProperty); }
            set { SetValue(StageProperty, value); }
        }

        public static readonly DependencyProperty StageProperty/*how to call me in the xaml code */ =
            DependencyProperty.Register("Stage", typeof(BO.ProjectStatus), typeof(ManagerChoose), new PropertyMetadata(null));

        public ManagerChoose()
        {
            try
            {
                Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void ShowTaskList(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void ShowEngineerList(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void InitializeButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("are you sure you want to initialize the data base?", //the messege
                "Initialization confirm", //title
                MessageBoxButton.YesNo) == MessageBoxResult.Yes) //if the user answe
                                                                 //
                                                                 //
                                                                 //
                                                                 //
                                                                 //
                                                                 //
                                                                 //'yes'
                {
                    DalTest.Initialization.Do();
                    Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
                    s_bl.ResetClock();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Reset(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("are you sure you want to reset the data base?", //the messege
                "Reset confirm", //title
                MessageBoxButton.YesNo) == MessageBoxResult.Yes) //if the user answered 'yes'
                {
                    DalTest.Initialization.Reset();
                    Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
                    s_bl.ResetClock();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Schedile_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Stage == BO.ProjectStatus.Sceduling)
                {
                    new ScheduleWindow().ShowDialog();
                    Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
                }

                if (Stage == BO.ProjectStatus.Planning)
                {
                    new StartScheduleWindow().ShowDialog();
                    Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //three different buttons in the same place:
        private void OpenGanttWindowClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new ganttChart().ShowDialog();
                Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OpenScheduleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new ScheduleWindow().ShowDialog();
                Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OpenStartScheduleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new StartScheduleWindow().ShowDialog();
                Stage = (BO.ProjectStatus)s_bl.Config.getProjectStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
