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
            ProjectCurrentDate = DateTime.Now;
            InitializeComponent();
        }

        private void InitializeButton(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("are you sure you want to initialize the data base?", //the messege
                "Initialization confirm", //title
                MessageBoxButton.YesNo) == MessageBoxResult.Yes) //if the user answered 'yes'
            {
                DalTest.Initialization.Do();
            }
        }
        private void Reset(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("are you sure you want to reset the data base?", //the messege
                "Reset confirm", //title
                MessageBoxButton.YesNo) == MessageBoxResult.Yes) //if the user answered 'yes'
            {
                DalTest.Initialization.Reset();
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

        private void ResetClockClick(object sender, RoutedEventArgs e)
        {
            ProjectCurrentDate = s_bl.Engineer.ResetClock();  //returnes dateTime.now
        }
    }
}