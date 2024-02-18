using PL.Engineer;
using PL.Manager;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerOrEngineer.xaml
    /// </summary>
    public partial class ManagerOrEngineer : Window
    {
        public ManagerOrEngineer()
        {
            InitializeComponent();
        }

        private void DecideButton(object sender, RoutedEventArgs e)
        {
            new ManagerChoose().ShowDialog();
        }
    }
}
