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

        public static readonly DependencyProperty EngineerListProperty = 
            DependencyProperty.Register("EngineerList"/*how to call me in the xaml code */, typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));


        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All)!;

        }

        private void readListAgain(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = s_bl.Engineer.ReadAllEngineers(e => e.Level == chosenLevel || chosenLevel == BO.EngineerExperience.All);
        }

        private void OpenCreateWindow(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }
    }
}
