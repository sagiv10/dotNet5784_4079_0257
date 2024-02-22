using PL.Task;
using System;
using BL;
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
using BO;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for ChooseIdToAssign.xaml
    /// </summary>
    public partial class ChooseIdToAssign : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public List<int> ListOfAllTasksIDWithoutEngineer
        {
            get { return (List<int>)GetValue(ListOfAllTasksIDWithoutEngineerProperty); }
            set { SetValue(ListOfAllTasksIDWithoutEngineerProperty, value); }
        }
        public static readonly DependencyProperty ListOfAllTasksIDWithoutEngineerProperty =
        DependencyProperty.Register("ListOfAllTasksIDWithoutEngineer", typeof(List<int>), typeof(ChooseIdToAssign), new PropertyMetadata(null));

        public int IdToAssign
        {
            get { return (int)GetValue(IdToAssignProperty); }
            set { SetValue(IdToAssignProperty, value); }
        }
        public static readonly DependencyProperty IdToAssignProperty =
            DependencyProperty.Register("IdToAssign", typeof(int), typeof(ChooseIdToAssign), new PropertyMetadata(0));

        BO.Engineer engFromEngineerList;

        public ChooseIdToAssign(BO.Engineer? eng)
        {
            List<BO.TaskInList> tempList = s_bl.Engineer.GetPotentialTasks(eng!.Id);
            ListOfAllTasksIDWithoutEngineer = (from TaskInListEngineer in tempList
                                              select TaskInListEngineer.Id).ToList();
            engFromEngineerList = eng;
            InitializeComponent();
        }
        private void AssignClick(object sender, RoutedEventArgs e)
        {
            if (IdToAssign == 0)
                MessageBox.Show("you didn't choose an id of task to assign...");
            else
            {
                s_bl.Engineer.AssignTask(engFromEngineerList!.Id, IdToAssign);
                MessageBox.Show($"task {IdToAssign} has been assigned to engineer with id:{engFromEngineerList!.Id} succesfully!");
                this.Close();
            }
        }
    }
}
