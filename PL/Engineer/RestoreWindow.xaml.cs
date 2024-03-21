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
    /// Interaction logic for RestoreWindow.xaml
    /// </summary>
    public partial class RestoreWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.Engineer> NotActiveList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(NotActiveListProperty); }
            set { SetValue(NotActiveListProperty, value); }
        }

        public static readonly DependencyProperty NotActiveListProperty/*how to call me in the xaml code */ =
            DependencyProperty.Register("NotActiveList", typeof(IEnumerable<BO.Engineer>), typeof(RestoreWindow), new PropertyMetadata(null));
        public RestoreWindow()
        {
            NotActiveList = s_bl.Engineer.getDeleted();
            InitializeComponent();
        }

        private void RestoreEngineerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (sender as Button)!;
                BO.Engineer SpecificEngineerFromList = (button!.CommandParameter as BO.Engineer)!;
                if (SpecificEngineerFromList != null)
                {
                    s_bl.Engineer.GetEngineerToActive(SpecificEngineerFromList.Id);
                    MessageBox.Show("the engineer has been restored succesfully!");
                    Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
