using BO;
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
using static System.Net.Mime.MediaTypeNames;

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



        public EngineerWindow(int id=0)//this method runs every time we open the window
        {
            InitializeComponent();
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\User\\Desktop\\minip\\dotNet5784_4079_0257\\PL\\Engineer\\Photos\\output-onlinepngtools (3).png", UriKind.RelativeOrAbsolute)));
            this.Background = imageBrush;
            Stage = (id == 0);
            if (id != 0)//if id isn't default so get all details
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
            else //if the id is 0 
            {
                Engineer = new BO.Engineer(
                    id,
                    "",
                    "",
                    BO.EngineerExperience.Beginner,
                    0.0,
                    new BO.TaskInEngineer(0, "")//builds en empty TaskInEngineer to show his details in the singleShow window
                    );
            }
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (Stage) //if we are at create Mode and id=0
            {
                try
                {
                    if (Engineer.Task!.Id == 0)
                    {
                        s_bl.Engineer.CreateEngineer(Engineer);
                        MessageBox.Show("engineer created succesfully!");
                    }
                    else
                    {
                        
                        //s_bl.Engineer.AssignTask(Engineer.Id, Engineer.Task!.Id);
                        s_bl.Engineer.CreateEngineer(Engineer);
                        MessageBox.Show("engineer created succesfully!");
                    }
                    this.Close();
                }
                catch (BO.BLWrongInputException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BLAlreadyExistException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLNotFoundException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLHasTaskException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLNotAvialableTaskException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLWrongStageException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    s_bl.Engineer.UpdateEngineer(Engineer);
                    MessageBox.Show("engineer updated succesfully!");
                    this.Close();

                }
                catch (BO.BLWrongIdException ex) //somehow we got into negative id!
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLWrongEmailException ex) //if the id is not found
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLWrongCostException ex) //if the id is not found
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLNotFoundException ex) //if the id is not found
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.BLCannotLowerLevelException ex) //if the id is not found
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        
    }
}
