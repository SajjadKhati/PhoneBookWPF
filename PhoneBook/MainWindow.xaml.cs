﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ViewModel;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void PhoneBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var people = myViewModel.PhoneBook.People;
            
            //BindingOperations.EnableCollectionSynchronization(people, PhoneBook.AddPeopleTaskLock)

            // فراخوانی متدهای مربوط به لود در ViewModel و سپس در Model . بعد از آن هم فراخوانی متد Disable در BindingOperation
        }


        private async void ButtonBase_Click(object sender, RoutedEventArgs e)
        {
            TestInViewModel testInViewModel = new TestInViewModel();
            await testInViewModel.Test();

            //MessageBox.Show("Task Completed");

        }





    }


}
