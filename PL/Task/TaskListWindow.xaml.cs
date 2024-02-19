﻿using PL.Engineer;
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
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Status chosenStatus { get; set; } = BO.Status.Unscheduled;

        public IEnumerable<BO.TaskInList> TaskInList_List
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskInList_ListProperty); }
            set { SetValue(TaskInList_ListProperty, value); }
        }

        public static readonly DependencyProperty TaskInList_ListProperty/*how to call me in the xaml code */ =
            DependencyProperty.Register("TaskInList_ListProperty", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
        public TaskListWindow()
        {
            InitializeComponent();
            TaskInList_List = s_bl?.Task.ReadAll()!;
        }

        private void ShowSpecificTask(object sender, MouseButtonEventArgs e)//sender has the details of which task we been sent from. 
        {
            BO.Task? SpecificTaskFromList = (sender as ListView)?.SelectedItem as BO.Task; //get inside "SpecificTaskFromList" the details from the sender about which task we clicked on.
            //new SpecificTaskWindow/*GO SAGIV!*/(SpecificTaskFromList!.Id).ShowDialog();//open the window of showing specifc task with the details from last line.
            //this third line is happananing only after the user pushes the assign/de-assign button and closes the mini window! :
            TaskInList_List = s_bl?.Task.ReadAll(e => e.Status == chosenStatus || chosenStatus == BO.Status.Unscheduled)!;
        }

        private void ReadListAgain(object sender, SelectionChangedEventArgs e)//this method happans after everytime we change the combobox. (to update the view of the shown list) 
        {
            TaskInList_List = s_bl?.Task.ReadAll(e => e.Status == chosenStatus || chosenStatus == BO.Status.Unscheduled)!;
        }
        private void AddTaskClick(object sender, RoutedEventArgs e)
        {
            //new SpecificTaskWindow/*GO SAGIV!*/().ShowDialog();//open the window of showing specifc task without details. (because we want to add new task)
        }
        private void addDependencyClick(object sender, RoutedEventArgs e) 
        {
            new AddRemoveDependency(true/*indicates we are in add mode*/).ShowDialog();//open the window of the add of dependency
        }
        private void removeDependencyClick(object sender, RoutedEventArgs e)
        {
            new AddRemoveDependency(false/*indicates we are in remove mode*/).ShowDialog();//open the window of the remove of dependency
        }
    }
}
