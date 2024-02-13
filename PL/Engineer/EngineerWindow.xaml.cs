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
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public bool Stage; //will be true if we are in create mode and will be false if we are in update mode

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));



        public EngineerWindow(int id=0)
        {
            Stage = (id == 0);
            if (id != 0)
            {
                try
                {
                    Engineer = s_bl.Engineer.ReadEngineer(id);
                }
                catch (BO.BLWrongIdException ex) //somehow we got into negative id!
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLNotFoundException ex) //if the id is not found
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else //if the id is 0 we need an empty task
            {
                Engineer = new BO.Engineer(
                    id,
                    "",
                    "",
                    BO.EngineerExperience.Beginner,
                    0.0,
                    new BO.TaskInEngineer(0,"")//builds en empty TaskInEngineer to show his details in the singleShow window
                    ); ;
            }
            InitializeComponent();

        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (Stage) //if we are at create Mode
            {

            }
        }
    }
}
