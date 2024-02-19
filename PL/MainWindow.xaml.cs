using PL.Engineer;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
        private void ShowDecideWindow(object sender, RoutedEventArgs e)
        {
            new ManagerOrEngineer().Show();
        }
    }
}