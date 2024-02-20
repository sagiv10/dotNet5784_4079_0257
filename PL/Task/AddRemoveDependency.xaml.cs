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
        public int IdOfDependentTask
        {
            get { return (int)GetValue(IdOfDependentTaskProperty); }
            set { SetValue(IdOfDependentTaskProperty, value); }
        }
        public static readonly DependencyProperty IdOfDependentTaskProperty =
            DependencyProperty.Register("IdOfDependentTaskProperty", typeof(int), typeof(AddRemoveDependency), new PropertyMetadata(0));
        public int IdOfDependsOnTask
        {
            get { return (int)GetValue(IdOfDependsOnTaskProperty); }
            set { SetValue(IdOfDependsOnTaskProperty, value); }
        }

        public static readonly DependencyProperty IdOfDependsOnTaskProperty =
            DependencyProperty.Register("IdOfDependsOnTaskProperty", typeof(int), typeof(AddRemoveDependency), new PropertyMetadata(0));
        public bool IsAdd
        {
            get { return (bool)GetValue(IsAddProperty); }
            set { SetValue(IsAddProperty, value); }
        }

        public static readonly DependencyProperty IsAddProperty =
            DependencyProperty.Register("IsAdd", typeof(bool), typeof(AddRemoveDependency), new PropertyMetadata(false));

        public AddRemoveDependency(bool _isAdd = false) //if id==0 -> add, else -> remove.             
        {                                                
            InitializeComponent();
            IsAdd = _isAdd;
        }

        private void AddRemoveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAdd)//if we in add mode
                {
                    s_bl.Task.AddDependency(IdOfDependentTask,IdOfDependsOnTask);
                    MessageBox.Show("dependency created succesfully!");
                }
                else     //if we in remove mode
                {
                    s_bl.Task.DeleteDependency(IdOfDependentTask,IdOfDependsOnTask);
                    MessageBox.Show("dependency removed succesfully!");
                }
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
    }
}
