using PL.Engineer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    using DalTest;
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
            if(MessageBox.Show("are you sure you want to initialize the data base?", //the messege
                "Initialization confirm", //title
                MessageBoxButton.YesNo) == MessageBoxResult.Yes) //if the user answered 'yes'
            {
                DalTest.Initialization.Do();
            }
        }

        private void HandleEngineersButton(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
    }
}