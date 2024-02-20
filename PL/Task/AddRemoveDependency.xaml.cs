using BO;
using PL.Engineer;
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
    /// Interaction logic for AddRemoveDependency.xaml
    /// </summary>
    public partial class AddRemoveDependency : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public bool IsAdd
        {
            get { return (bool)GetValue(IsAddProperty); }
            set { SetValue(IsAddProperty, value); }
        }
        //by using the data context we able to use this property in our xaml code:
        public static readonly DependencyProperty IsAddProperty/*how to call me in the xaml code */ =
            DependencyProperty.Register("IsAddProperty", typeof(bool), typeof(AddRemoveDependency), new PropertyMetadata(null));
        
        public AddRemoveDependency(bool _isAdd = false) //if id==0 -> add, else -> remove.             
        {                                                
            InitializeComponent();
           
            IsAdd = (_isAdd);
        }

        private void AddRemoveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAdd)//if we in add mode
                {
                    //s_bl.Task.AddDependency(/*getfromfirstcheckbox*/,/*getformsecondcheckbox*/);
                    MessageBox.Show("dependency created succesfully!");
                }
                else     //if we in remove mode
                {
                    //s_bl.Task.DeleteDependency(/*getfromfirstcheckbox*/,/*getformsecondcheckbox*/);
                    MessageBox.Show("dependency removed succesfully!");
                }
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
    }
}
