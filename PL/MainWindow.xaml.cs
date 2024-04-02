using PL.Engineer;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PL.Manager;
using PL.Schedule;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        /// <summary>
        /// the time to the clock
        /// </summary>
        public DateTime ProjectCurrentDate
        {
            get { return (DateTime)GetValue(ProjectCurrentDateProperty); }
            set { SetValue(ProjectCurrentDateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectCurrentDateProperty =
            DependencyProperty.Register("ProjectCurrentDate", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

        /// <summary>
        /// the Stage of the project
        /// </summary>
        public BO.ProjectStatus CurrentStatus
        {
            get { return (BO.ProjectStatus)GetValue(CurrentStatusProperty); }
            set { SetValue(CurrentStatusProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentStatusProperty =
            DependencyProperty.Register("CurrentStatus", typeof(BO.ProjectStatus), typeof(MainWindow), new PropertyMetadata(null));



        public MainWindow()
        {
            try
            {
                s_bl.initializeClock();//critic for setting from xml
                ProjectCurrentDate = s_bl.Clock; //now the starting date will always exist
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void baruchHashem(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("!ברוך ה");
        }
        private void ManagerPicked(object sender, RoutedEventArgs e)
        {
            try
            {
                new ManagerChoose().ShowDialog();
                ProjectCurrentDate = s_bl.Clock;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EngineerPicked(object sender, RoutedEventArgs e)
        {
            try
            {
                new EngineerIdEntering().ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetClock(object sender, RoutedEventArgs e)
        {
            try
            {
                ProjectCurrentDate = DateTime.Now; //reset the clock to now
                s_bl.ResetClock(); //get the new time we got from the init function
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddWeekClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ProjectCurrentDate = ProjectCurrentDate.AddDays(7); //add 7 days - week
                s_bl.AddWeek();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddDayClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ProjectCurrentDate = ProjectCurrentDate.AddDays(1); //add 1 day
                s_bl.AddDay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddMonthClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ProjectCurrentDate = ProjectCurrentDate.AddMonths(1); //add 1 month
                s_bl.AddMonth();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            s_bl.Config.SaveProjectCurrentDateIntoXml(ProjectCurrentDate);
        }
    }
}