﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ViewModel.PhoneBookViewModelModule.Class;
using ViewModel.PhoneBookViewModelModule.Interface;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IPhoneBookViewModelAggregator _phoneBookViewModel;




        public IPhoneBookViewModelAggregator PhoneBookViewModel
        {
            get
            {
                return this._phoneBookViewModel;
            }
            set
            {
                if (this._phoneBookViewModel != value)
                {
                    this._phoneBookViewModel = value;
                    this.OnPropertyChanged();
                }
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;




        public MainWindow()
        {
            InitializeComponent();

            this.InitializeViewModelsModules();
        }




        private async void PhoneBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await this.LoadPhoneBookAsync();
        }


        private async void ButtonBase_Click(object sender, RoutedEventArgs e)
        {
            //object aaa = this.PhoneBookViewModel;
            //object bb = this.Combobox;
            //PhoneBookViewModel viewModel = new PhoneBookViewModel();
            //viewModel.LoadPhoneBookAsyncCommand.Execute(null);

            ////////////////////////////////
            /// 
            //TestInViewModel testInViewModel = new TestInViewModel();
            //await testInViewModel.Test();
            //MessageBox.Show("Task Completed");
        }


        ///////////////////////////////


        private void InitializeViewModelsModules()
        {
            this.InitializePhoneBookViewModel();
        }


        private void InitializePhoneBookViewModel()
        {
            this.PhoneBookViewModel = new PhoneBookViewModel(this.PersonDeleteStatus, this.LoadExceptionOccured, this.SaveSuccessed);
        }



        private async Task LoadPhoneBookAsync()
        {
            Tuple<IEnumerable, object> phoneBookBindingOperationsInfo = this.PhoneBookViewModel.GetPhoneBookBindingOperationsInfo();

            BindingOperations.EnableCollectionSynchronization(phoneBookBindingOperationsInfo.Item1,
                phoneBookBindingOperationsInfo.Item2);

            await this.PhoneBookViewModel.LoadPhoneBookAsyncCommand.ExecuteAsync(null);

            BindingOperations.DisableCollectionSynchronization(phoneBookBindingOperationsInfo.Item1);
        }


        private void PersonDeleteStatus(Tuple<bool, Exception> personDeleteInfo)
        {
            if (personDeleteInfo.Item2 != null)
            {
                string errorMessage = $"خطایی زمان حذف اطلاعات و رکورد مربوط به شخص از پایگاه داده اتفاق افتاد  :\n\n{personDeleteInfo.Item2.Message}";
                MessageBox.Show(errorMessage, "خطای حذف اطلاعات");
            }
        }


        private void LoadExceptionOccured(Exception loadException)
        {
            string errorMessage = $"خطایی زمان بارگذاری اطلاعات از پایگاه داده اتفاق افتاد  :\n\n{loadException.Message}";
            MessageBox.Show(errorMessage, "خطای بارگذاری");
        }


        private void SaveSuccessed(string message)
        {
            MessageBox.Show(message, "ذخیره ی موفقیت آمیز");
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }


}
