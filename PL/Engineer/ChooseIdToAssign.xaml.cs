using PL.Task;
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
    /// Interaction logic for ChooseIdToAssign.xaml
    /// </summary>
    public partial class ChooseIdToAssign : Window
    {
        public List<int> ListOfAllTasksWithoutEngineer
        {
            get { return (List<int>)GetValue(ListOfAllTasksWithoutEngineerProperty); }
            set { SetValue(ListOfAllTasksWithoutEngineerProperty, value); }
        }
        public static readonly DependencyProperty ListOfAllTasksWithoutEngineerProperty =
        DependencyProperty.Register("ListOfAllTasksWithoutEngineer", typeof(List<int>), typeof(ChooseIdToAssign), new PropertyMetadata(null));

        public int IdToAssign
        {
            get { return (int)GetValue(IdToAssignProperty); }
            set { SetValue(IdToAssignProperty, value); }
        }
        public static readonly DependencyProperty IdToAssignProperty =
            DependencyProperty.Register("IdToAssign", typeof(int), typeof(ChooseIdToAssign), new PropertyMetadata(0));


        public ChooseIdToAssign(BO.Engineer? eng)
        {
            InitializeComponent();
        }
    }
}
