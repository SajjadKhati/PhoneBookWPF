using CommunityToolkit.Mvvm.Input;
using Model.PhoneBookModule.Class;
using Model.PhoneBookModule.Interface;
using Model.ValidationModule.Class;
using Model.ValidationModule.Interface;
using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel.PhoneBookViewModelModule.Interface;

namespace ViewModel.PhoneBookViewModelModule.Class
{
    public class PhoneBookViewModel : IPhoneBookViewModelAggregator
    {
        public IPhoneBookAggregator PhoneBook { get; }


        public IAsyncRelayCommand LoadPhoneBookAsyncCommand { get; }


        public ICommand LoadCitiesByProvinceIdCommand { get; }




        private Action<Tuple<bool, Exception>> _personDeleteStatusAction;


        private Action<Exception> _loadExceptionOccuredAction;


        private Action<string> _saveSuccessedAction;




        public PhoneBookViewModel(Action<Tuple<bool, Exception>> personDeleteStatusAction, 
            Action<Exception> loadExceptionOccuredAction, Action<string> saveSuccessedAction)
        {
            this.PhoneBook = new PhoneBook();
            this.PhoneBook.PersonDeleteStatus += this.PhoneBook_PersonDeleteStatus;

            this.LoadPhoneBookAsyncCommand = new AsyncRelayCommand(this.LoadPhoneBookAsync);
            this.LoadCitiesByProvinceIdCommand = new RelayCommand<int>(this.LoadCitiesByProvinceId);

            this._personDeleteStatusAction = personDeleteStatusAction;
            this._loadExceptionOccuredAction = loadExceptionOccuredAction;
            this._saveSuccessedAction = saveSuccessedAction;
        }




        public Tuple<IEnumerable, object> GetPhoneBookBindingOperationsInfo()
        {
            IEnumerable collectionForSynchronization = this.PhoneBook.People;
            object collectionSynchronizationLock = Model.PhoneBookModule.Class.PhoneBook.AddPeopleTaskLock;
            return new Tuple<IEnumerable, object>(collectionForSynchronization, collectionSynchronizationLock);
        }



        private async Task LoadPhoneBookAsync()
        {
            try
            {
                await this.PhoneBook.LoadPeopleAsync();
                await this.PhoneBook.LoadProvincesAsync();
            }
            catch (Exception exception)
            {
                this._loadExceptionOccuredAction?.Invoke(exception);
            }
        }


        private void LoadCitiesByProvinceId(int provinceId)
        {
            if (provinceId < 1)
            {
                this._loadExceptionOccuredAction?.Invoke(new Exception("مقدار شناسه ی استان ، باید بزرگتر از صفر باشد ."));
                return;
            }

            try
            {
                this.PhoneBook.LoadCitiesByProvinceId(provinceId);
            }
            catch (Exception exception)
            {
                this._loadExceptionOccuredAction?.Invoke(exception);
            }
        }


        private void PhoneBook_PersonDeleteStatus(object sender, DeleteStatusEventArgs e)
        {
            this._personDeleteStatusAction?.Invoke(new Tuple<bool, Exception>(e.IsDeletionSuccessed, e.DeletionException));
        }


        public bool AddPerson(object person)
        {
            if (person == null)
                return false;

            Person personConverted = person as Person;
            if (personConverted == null)
                return false;

            bool isSaved = this.PhoneBook.AddPerson(personConverted);
            if (isSaved == true)
                this._saveSuccessedAction?.Invoke("عملیات ذخیره سازی ، با موفقیت انجام شد");
            return isSaved;
        }


        public bool EditPerson(object person)
        {
            if (person == null)
                return false;

            Person personConverted = person as Person;
            if (personConverted == null)
                return false;

            bool isSaved = this.PhoneBook.EditPerson(personConverted);
            if (isSaved == true)
                this._saveSuccessedAction?.Invoke("عملیات ذخیره سازی ، با موفقیت انجام شد");
            return isSaved;
        }


    }
}
