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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for RestoreTaskWindow.xaml
    /// </summary>
    public partial class RestoreTaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.TaskInList> NotActiveList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(NotActiveListProperty); }
            set { SetValue(NotActiveListProperty, value); }
        }
        public static readonly DependencyProperty NotActiveListProperty/*how to call me in the xaml code */ =
        DependencyProperty.Register("NotActiveList", typeof(IEnumerable<BO.TaskInList>), typeof(RestoreTaskWindow), new PropertyMetadata(null));

        public RestoreTaskWindow()
        {
            NotActiveList = s_bl.Task.getDeleted();
            InitializeComponent();
        }

        private void RestoreTask(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (sender as Button)!;
                BO.TaskInList SpecificTaskFromList = (button!.CommandParameter as BO.TaskInList)!;
                if (SpecificTaskFromList != null)
                {
                    s_bl.Task.GetTaskToActive(SpecificTaskFromList.Id);
                    MessageBox.Show("the task has been restored succesfully!");
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
