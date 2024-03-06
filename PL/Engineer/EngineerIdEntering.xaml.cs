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
    /// Interaction logic for EngineerIdEntering.xaml
    /// </summary>
    public partial class EngineerIdEntering : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Engineer", typeof(string), typeof(EngineerIdEntering), new PropertyMetadata(null));
        public EngineerIdEntering()
        {
            Id = "0";
            InitializeComponent();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void EnterId(object sender, RoutedEventArgs e)
        {
            try
            {
                int realId = s_bl.Config.ParseToInt(Id, "Id");
                s_bl.Engineer.ReadEngineer(realId);
                new EngineerMenu(realId).Show();
                this.Close();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
