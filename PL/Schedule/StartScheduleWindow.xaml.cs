using PL.Manager;
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

namespace PL.Schedule
{
    /// <summary>
    /// Interaction logic for StartScheduleWindow.xaml
    /// </summary>
    public partial class StartScheduleWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DateTime ChosenDate
        {
            get { return (DateTime)GetValue(ChosenDateProperty); }
            set { SetValue(ChosenDateProperty, value); }
        }

        public static readonly DependencyProperty ChosenDateProperty/*how to call me in the xaml code */ =
            DependencyProperty.Register("ChosenDate", typeof(DateTime), typeof(StartScheduleWindow), new PropertyMetadata(null));
        public StartScheduleWindow()
        {
            try
            {
                ChosenDate = s_bl.Clock; //the default value will be the clock time
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void StartSchedule(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("are you shure this is the date that you want?", "Date confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    s_bl.Config.StartSchedule(ChosenDate);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
