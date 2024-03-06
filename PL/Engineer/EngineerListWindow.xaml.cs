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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.EngineerExperience chosenLevel { get; set; } = BO.EngineerExperience.All;

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty/*how to call me in the xaml code */ = 
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        //the stage of the project
        public BO.ProjectStatus Stage
        {
            get { return (BO.ProjectStatus)GetValue(StageProperty); }
            set { SetValue(StageProperty, value); }
        }

        public static readonly DependencyProperty StageProperty/*how to call me in the xaml code */ =
            DependencyProperty.Register("Stage", typeof(BO.ProjectStatus), typeof(EngineerListWindow), new PropertyMetadata(null));

        public EngineerListWindow()
        {
            try
            {
                EngineerList = s_bl?.Engineer.ReadAllEngineers()!;
                Stage = (BO.ProjectStatus)s_bl!.Config.getProjectStatus();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void readListAgain(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EngineerList = s_bl.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenCreateWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                new EngineerWindow().ShowDialog();
                EngineerList = s_bl?.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All)!;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BO.Engineer? SpecificEngineerFromList = (sender as ListView)?.SelectedItem as BO.Engineer;
                if (SpecificEngineerFromList == null)//for case someone taps on the blank spaces in the window that doesnt have value in their line. 
                    return;
                new EngineerWindow(SpecificEngineerFromList!.Id).ShowDialog();
                //this third line is happananing only after the user pushes the add/update button and closes the mini window! :
                EngineerList = s_bl?.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All)!;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AssignTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                BO.Engineer SpecificEngineerFromList = button?.CommandParameter as BO.Engineer;
                if (SpecificEngineerFromList != null)
                {
                    new ChooseIdToAssign(SpecificEngineerFromList).ShowDialog();
                    EngineerList = s_bl?.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All)!;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeAssignTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                BO.Engineer SpecificEngineerFromList = button?.CommandParameter as BO.Engineer;
                if (SpecificEngineerFromList != null)
                {
                    s_bl.Engineer.DeAssignTask(SpecificEngineerFromList.Id);
                    MessageBox.Show("the task has be de-assgined succesfully!");
                    EngineerList = s_bl?.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All)!;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FinishTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                BO.Engineer SpecificEngineerFromList = button?.CommandParameter as BO.Engineer;
                if (SpecificEngineerFromList != null)
                {
                    s_bl.Engineer.FinishTask(SpecificEngineerFromList.Id);
                    MessageBox.Show("the task has been finished succesfully!");
                    EngineerList = s_bl?.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All)!;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (sender as Button)!;
                BO.Engineer SpecificEngineerFromList = (button!.CommandParameter as BO.Engineer)!;
                if (SpecificEngineerFromList != null)
                {
                    s_bl.Engineer.DeleteEngineer(SpecificEngineerFromList.Id);
                    MessageBox.Show("the task has been finished succesfully!");
                    EngineerList = s_bl?.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All)!;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
