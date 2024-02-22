using PL.Engineer;
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
        public ManagerChoose()
        {
            InitializeComponent();
        }

        private void ShowTaskList(object sender, RoutedEventArgs e)
        {
            new TaskListWindow(true).Show();
        }

        private void ShowEngineerList(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
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
    }
}
