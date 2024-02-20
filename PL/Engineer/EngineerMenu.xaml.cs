using BlApi;
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
    /// Interaction logic for EngineerMenu.xaml
    /// </summary>
    public partial class EngineerMenu : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        BO.ProjectStatus currentStatus
        {
            get { return (BO.ProjectStatus)GetValue(currentStatusProperty); }
            set { SetValue(currentStatusProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty currentStatusProperty =
            DependencyProperty.Register("currentStatus", typeof(BO.ProjectStatus), typeof(EngineerMenu), new PropertyMetadata(null));

        //the engineer of this window
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerMenu), new PropertyMetadata(null));

        public EngineerMenu(int id = 0)
        {
            try
            {
                Engineer = s_bl.Engineer.ReadEngineer(id);
            }
            catch (Exception ex) //if somhow the engineer is not exists
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close(); //close this window
            }
            currentStatus = (BO.ProjectStatus)s_bl.Task.getProjectStatus();
            InitializeComponent();
        }

        private void Assign_ShowTask(object sender, RoutedEventArgs e)
        {

        }

    }
}
