using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ViewModel.PresentationLogicModule;
using System.Windows;
using ViewModel.PhoneBookViewModelModule.Class;
using XceedNS = Xceed.Wpf.Toolkit;
using System.Windows.Data;
using ViewModel.SortingModule;

namespace PhoneBook.Window
{
    public partial class MainWindow
    {
        private bool _allowAddFirstNameFilter = true;


        private bool _allowAddLastNameFilter = true;


        private bool _allowAddMobileNumberFilter = true;


        private bool _allowAddProvinceFilter = true;


        private bool _allowAddCityFilter = true;


        private bool _allowAddAddressDetailFilter = true;


        private bool _allowAddPostalCodeFilter = true;





        private void InitializeViewModelsModules()
        {
            this.InitializePhoneBookViewModel();
        }


        private void InitializePhoneBookViewModel()
        {
            this.PhoneBookViewModel = new PhoneBookViewModel(this.PersonDeleteStatus, this.LoadExceptionOccured, this.SaveSuccessed);
            this.PhoneBookViewModel.AnyPersonOperationCanceled += PhoneBookViewModel_AnyPersonOperationCanceled;
        }


        private void DecideVisibilityForRowDetailsElement(object newSelectedItemPerson, IList unSelectedItemPersons)
        {
            if (unSelectedItemPersons != null && unSelectedItemPersons.Count > 0)
            {
                foreach (object unSelectedItemPerson in unSelectedItemPersons)
                {
                    this.CollapseUnselectedRowDetailsElement(unSelectedItemPerson);
                }
            }

            if (newSelectedItemPerson == null)
                return;
            DataGridRow newSelectedDataGridRow = this.PhoneBookDataGrid.ItemContainerGenerator.
                ContainerFromItem(newSelectedItemPerson) as DataGridRow;
            if (newSelectedDataGridRow == null)
                return;

            if (PresentationLogicModule.HasValuePostalCodeOrAddressDetails(newSelectedItemPerson))
                newSelectedDataGridRow.DetailsVisibility = Visibility.Visible;
            else
                newSelectedDataGridRow.DetailsVisibility = Visibility.Collapsed;
        }


        private void CollapseUnselectedRowDetailsElement(object unSelectedItemPerson)
        {
            DataGridRow unSelectedDataGridRow = this.PhoneBookDataGrid.ItemContainerGenerator.
                ContainerFromItem(unSelectedItemPerson) as DataGridRow;
            if (unSelectedDataGridRow == null)
                return;

            unSelectedDataGridRow.DetailsVisibility = Visibility.Collapsed;
        }


        private void PersonDeleteStatus(Tuple<bool, Exception> personDeleteInfo)
        {
            if (personDeleteInfo.Item2 != null)
            {
                string errorMessage = $"خطایی زمان حذف اطلاعات و رکورد مربوط به شخص از پایگاه داده اتفاق افتاد  :\n\n{personDeleteInfo.Item2.Message}";
                XceedNS.MessageBox.Show(errorMessage, "خطای حذف اطلاعات");
            }
        }


        private void LoadExceptionOccured(Exception loadException)
        {
            string errorMessage = $"خطایی زمان بارگذاری اطلاعات از پایگاه داده اتفاق افتاد  :\n\n{loadException.Message}";
            XceedNS.MessageBox.Show(errorMessage, "خطای بارگذاری");
        }


        private void SaveSuccessed(string message)
        {
            XceedNS.MessageBox.Show(message, "ذخیره ی موفقیت آمیز");
            this.SearchAndGroupingGrid.IsEnabled = true;
        }


        private void PhoneBookViewModel_AnyPersonOperationCanceled()
        {
            XceedNS.MessageBox.Show("عملیات ، لغو شد");
            this.SearchAndGroupingGrid.IsEnabled = true;
        }


        private bool ShowDeleteMessage()
        {
            string deleteMessage = PresentationLogicModule.GetPersonDeleteMessage(this.PhoneBookDataGrid.CurrentItem);
            string deleteCaption = "حذف سطر";

            MessageBoxResult deletionQuestionResult = XceedNS.MessageBox.Show(deleteMessage, deleteCaption,
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (deletionQuestionResult == MessageBoxResult.Yes)
                return true;
            else if (deletionQuestionResult == MessageBoxResult.No)
                return false;
            return false;
        }


        private void SetCustomSortInDataGridSource()
        {
            ListCollectionView dataGridsSourceCollectionView = (this.Resources["PeopleCollectionViewSourceKey"] as CollectionViewSource)
                ?.View as ListCollectionView;
            if (dataGridsSourceCollectionView == null)
                return;

            IList<DataGridRow> allDataGridRow = this.GetAllDataGridRows();
            if (allDataGridRow == null || allDataGridRow.Count < 2)
                return;

            RowIndexComparer rowIndexComparer = new RowIndexComparer
            {
                RowIndexPersons = new Dictionary<object, int>(allDataGridRow.Count)
            };
            foreach (DataGridRow dataGridRow in allDataGridRow)
            {
                if (dataGridRow.Item != null)
                    rowIndexComparer.RowIndexPersons.Add(dataGridRow.Item, dataGridRow.AlternationIndex);
            }

            dataGridsSourceCollectionView.CustomSort = dataGridsSourceCollectionView.CustomSort ?? rowIndexComparer;
        }


        private IList<DataGridRow> GetAllDataGridRows()
        {
            IList<DataGridRow> addDataGridRows = new List<DataGridRow>();

            foreach (object person in this.PhoneBookDataGrid.ItemsSource)
            {
                DataGridRow dataGridRow = this.PhoneBookDataGrid.ItemContainerGenerator.ContainerFromItem(person) as DataGridRow;
                if (dataGridRow != null)
                    addDataGridRows.Add(dataGridRow);
            }

            return (addDataGridRows.Count > 0) ? addDataGridRows : null;
        }


        private void UpdateFilterOnTextChanged(TextBox filterTextBox, FilterEventHandler filterEventHandler, ref bool allowAddFilter)
        {
            CollectionViewSource peopleCollectionViewSource = this.Resources["PeopleCollectionViewSourceKey"] as CollectionViewSource;
            if (filterTextBox == null || peopleCollectionViewSource == null)
                return;

            if (string.IsNullOrEmpty(filterTextBox.Text) == true)
            {
                peopleCollectionViewSource.Filter -= filterEventHandler;
                allowAddFilter = true;
            }
            else
            {
                if (allowAddFilter == true)
                {
                    peopleCollectionViewSource.Filter += filterEventHandler;
                    allowAddFilter = false;
                }
                else
                    peopleCollectionViewSource.View.Refresh();
            }
        }



    }
}
