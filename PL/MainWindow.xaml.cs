using PL.Engineer;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PL.Manager;
using PL.Schedule;

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
            if(s_bl.Task.getCurrentDate() == null)
            {
                s_bl.Task.SetCurrentDate(DateTime.Now);
            }
            ProjectCurrentDate = (DateTime)s_bl.Task.getCurrentDate()!; //now the starting date will always exist
            InitializeComponent();
        }

        private void InitializeButton(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("are you sure you want to initialize the data base?", //the messege
                "Initialization confirm", //title
                MessageBoxButton.YesNo) == MessageBoxResult.Yes) //if the user answered 'yes'
            {
                DalTest.Initialization.Do();
                ProjectCurrentDate = (DateTime)s_bl.Task.getCurrentDate()!; //get the new time we got from the init function
            }
        }
        private void Reset(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("are you sure you want to reset the data base?", //the messege
                "Reset confirm", //title
                MessageBoxButton.YesNo) == MessageBoxResult.Yes) //if the user answered 'yes'
            {
                DalTest.Initialization.Reset();
                ProjectCurrentDate = (DateTime)s_bl.Task.getCurrentDate()!; //get the new time we got from the reset function
            }
        }

        private void HandleEngineersButton(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void baruchHashem(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("!ברוך ה");
        }
        private void ManagerPicked(object sender, RoutedEventArgs e)
        {
            new ManagerChoose().ShowDialog();
        }
        private void EngineerPicked(object sender, RoutedEventArgs e)
        {
            new EngineerIdEntering().ShowDialog();
        }

        private void ResetClock(object sender, RoutedEventArgs e)
        {
            ProjectCurrentDate = DateTime.Now; //reset the clock to now
            s_bl.Task.SetCurrentDate(ProjectCurrentDate);
        }
        private void AddWeekClick(object sender, RoutedEventArgs e)
        {
            ProjectCurrentDate = ProjectCurrentDate.AddDays(7); //add 7 days - week
            s_bl.Task.SetCurrentDate(ProjectCurrentDate);
        }
        private void AddDayClick(object sender, RoutedEventArgs e)
        {
            ProjectCurrentDate = ProjectCurrentDate.AddDays(1); //add 1 day
            s_bl.Task.SetCurrentDate(ProjectCurrentDate);
        }
        private void AddMonthClick(object sender, RoutedEventArgs e)
        {
            ProjectCurrentDate = ProjectCurrentDate.AddMonths(1); //add 1 month
            s_bl.Task.SetCurrentDate(ProjectCurrentDate);
        }
    }
}