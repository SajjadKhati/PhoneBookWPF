using PhoneBook.VisualElementModule;
using PoshtibangirTolo.View.CustomControl;
using System;
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
using ViewModel.CollectionViewOperationsModule.Class;
using ViewModel.CollectionViewOperationsModule.Interface;
using ViewModel.PhoneBookValidationRuleModule;
using ViewModel.PhoneBookViewModelModule.Class;
using ViewModel.PhoneBookViewModelModule.Interface;
using ViewModel.PresentationLogicModule;
using ViewModel.SortingModule;
using XceedNS = Xceed.Wpf.Toolkit;

namespace PhoneBook.Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window, INotifyPropertyChanged
    {
        private readonly PersonValidationRule _personValidationRule;


        private readonly ICollectionViewOperationsAggregator _collectionViewOperations;


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


        public int SelectedStyleIndex{ get; set; }




        public event PropertyChangedEventHandler PropertyChanged;




        public MainWindow()
        {
            InitializeComponent();

            ///
            /// System.Diagnostics.PresentationTraceSources.SetTraceLevel(this.PhoneBookDataGrid.ItemContainerGenerator,
            ///    System.Diagnostics.PresentationTraceLevel.High);
            ///

            this.InitializeViewModelsModules();

            this._personValidationRule = Application.Current.Resources["PersonValidationRuleKey"] as PersonValidationRule;
            if (this._personValidationRule != null)
                this._personValidationRule.PhoneBookViewModel = this.PhoneBookViewModel;

            this._collectionViewOperations = Application.Current.Resources["CollectionViewOperationsKey"] 
                as ICollectionViewOperationsAggregator;
        }




        private async void PhoneBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await this.PhoneBookViewModel.LoadPhoneBookAsyncCommand.ExecuteAsync(null);

            (double centerX, double centerY) centerPoint = PresentationLogicModule.GetWindowCenterPoint(this.Width, this.Height);
            this.Left = centerPoint.centerX;
            this.Top = centerPoint.centerY;
        }


        private void PhoneBookDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            this.SearchAndGroupingBorder.IsEnabled = false;
        }


        private void PhoneBookDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            this._personValidationRule.AllowCallAddOrUpdate = true;
        }


        private void PhoneBookDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            this._personValidationRule.ShouldCallAdd = true;
        }


        private void PhoneBookDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            bool isConfirmToDelete = this.ShowDeleteMessage();
            /// اگر جواب پرسش حذف ، منفی بود ، هندلرِ رویداد PreviewKeyDown در DataGrid را اجرا نکند
            /// و این هندلر ، یعنی هندلرِ PhoneBookDataGrid_PreviewKeyDown، آخرین هندلرِ مربوط به این رویداد باشد که اجرا میکند .
            /// 
            /// چون رویدادهای مربوط به Preview ، بصورت استراتژی تونل ، یعنی از ریشه که پنجره ی اصلی باشند ، به سمت هدف که کنترل PhoneBookDataGrid هست ،
            /// حرکت میکند . بنابراین در ابتدا ، این هندلرِ PhoneBookDataGrid_PreviewKeyDown را اجرا میکند و سپس در صورت false بودنِ e.Handled ، هندلر رویداد
            /// UIElement.OnPreviewKeyDown را در DataGrid اجرا میکند .
            if (isConfirmToDelete == false)
                e.Handled = true;
        }


        private void MobileDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button mobileDeleteButton = sender as Button;
            if (mobileDeleteButton == null)
                return;

            int currentMobileCollectionIndex;
            try
            {
                currentMobileCollectionIndex = Convert.ToInt32(mobileDeleteButton.Tag);
            }
            catch (Exception exception)
            {
                return;
            }
            
            IEditableCollectionViewAddNewItem editingMobileCollectionViewSource =
                (VisualParentFinder.GetVisualParent<StackPanel>(mobileDeleteButton, "EditMobileCollectionColumnCellStackPanel")
                    ?.Resources["EditingMobileCollectionViewSourceKey"] as CollectionViewSource)?.View as IEditableCollectionViewAddNewItem;
            if (editingMobileCollectionViewSource == null)
                return;

            this._collectionViewOperations.DeleteItemFromCollectionViewCommand.Execute
            (new CollectionViewItemDeleteInfo()
            {
                EditingCollectionView = editingMobileCollectionViewSource,
                CollectionViewIndex = currentMobileCollectionIndex
            });
        }


        private void AddressDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button addressDeleteButton = sender as Button;
            if (addressDeleteButton == null)
                return;

            int currentAddressCollectionIndex;
            try
            {
                currentAddressCollectionIndex = Convert.ToInt32(addressDeleteButton.Tag);
            }
            catch (Exception exception)
            {
                return;
            }

            IEditableCollectionViewAddNewItem editingAddressCollectionViewSource =
                (VisualParentFinder.GetVisualParent<StackPanel>(addressDeleteButton, "EditAddressCollectionColumnCellStackPanel")
                    ?.Resources["EditingAddressCollectionViewSourceKey"] as CollectionViewSource)?.View as IEditableCollectionViewAddNewItem;
            if (editingAddressCollectionViewSource == null)
                return;

            this._collectionViewOperations.DeleteItemFromCollectionViewCommand.Execute
            (new CollectionViewItemDeleteInfo()
            {
                EditingCollectionView = editingAddressCollectionViewSource,
                CollectionViewIndex = currentAddressCollectionIndex
            });
        }


        private void PhoneBookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object newSelectedItemPerson = (e.AddedItems.Count > 0) ? e.AddedItems[0] : null;
            this.DecideVisibilityForRowDetailsElement(newSelectedItemPerson, e.RemovedItems);
        }


        private void EditProvinceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox editProvinceComboBox = sender as ComboBox;
            if (editProvinceComboBox == null || editProvinceComboBox.SelectedItem == null) 
                return;

            this.PhoneBookViewModel.LoadCitiesByProvinceIdCommand.Execute(editProvinceComboBox.SelectedItem);
        }


        private void PhoneBookDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header.ToString() != "ردیف" || e.Column.SortDirection != ListSortDirection.Ascending)
                return;

            e.Handled = true;
            e.Column.SortDirection = ListSortDirection.Descending;
            this.SetCustomSortInDataGridSource();
        }


        private void FilterFirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFilterOnTextChanged(sender as TextBox,
                this._collectionViewOperations.PersonsCollectionViewSource_FirstNameFilter, ref this._allowAddFirstNameFilter);
        }


        private void FilterLastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFilterOnTextChanged(sender as TextBox,
                this._collectionViewOperations.PersonsCollectionViewSource_LastNameFilter, ref this._allowAddLastNameFilter);
        }


        private void FilterMobileNumbersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFilterOnTextChanged(sender as TextBox,
                this._collectionViewOperations.PersonsCollectionViewSource_MobileNumbersFilter, ref this._allowAddMobileNumberFilter);
        }


        private void FilterProvincesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFilterOnTextChanged(sender as TextBox,
                this._collectionViewOperations.PersonsCollectionViewSource_ProvincesFilter, ref this._allowAddProvinceFilter);
        }


        private void FilterCitiesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFilterOnTextChanged(sender as TextBox,
                this._collectionViewOperations.PersonsCollectionViewSource_CitiesFilter, ref this._allowAddCityFilter);
        }


        private void FilterAddressDetailsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFilterOnTextChanged(sender as TextBox,
                this._collectionViewOperations.PersonsCollectionViewSource_AddressDetailsFilter, ref this._allowAddAddressDetailFilter);
        }


        private void FilterPostalCodesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFilterOnTextChanged(sender as TextBox,
                this._collectionViewOperations.PersonsCollectionViewSource_PostalCodesFilter, ref this._allowAddPostalCodeFilter);
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ShapeTextButton currentButton = sender as ShapeTextButton;
            if (currentButton == null)
                return;

            currentButton.Visibility = Visibility.Collapsed;
            this.SearchAndGroupingBorder.Visibility = Visibility.Visible;
            this.CollapseButton.Visibility = Visibility.Visible;
        }


        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            ShapeTextButton currentButton = sender as ShapeTextButton;
            if (currentButton == null)
                return;

            currentButton.Visibility = Visibility.Collapsed;
            this.SearchAndGroupingBorder.Visibility = Visibility.Collapsed;
            this.SearchButton.Visibility = Visibility.Visible;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(this) {Owner = this};
            settingsWindow.ShowDialog();
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }


}
