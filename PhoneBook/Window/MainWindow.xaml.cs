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
using ViewModel.PhoneBookValidationRuleModule;
using ViewModel.PhoneBookViewModelModule.Class;
using ViewModel.PhoneBookViewModelModule.Interface;
using ViewModel.SortingModule;
using XceedNS = Xceed.Wpf.Toolkit;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly PersonValidationRule _personValidationRule;




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

            this._personValidationRule = this.Resources["PersonValidationRuleKey"] as PersonValidationRule;
            if (this._personValidationRule != null)
                this._personValidationRule.PhoneBookViewModel = this.PhoneBookViewModel;
        }




        private async void PhoneBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await this.LoadPhoneBookAsync();
        }


        private void PhoneBookDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            this.OtherContentStackPanel.IsEnabled = false;
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


        private void MobileAddButton_Click(object sender, RoutedEventArgs e)
        {
            IEditableCollectionViewAddNewItem editingMobileCollectionViewSource = (((sender as Button)?.Parent as StackPanel)?.
                Resources["EditingMobileCollectionViewSourceKey"] as CollectionViewSource)?.View as IEditableCollectionViewAddNewItem;

            this.AddItemToCollectionView(editingMobileCollectionViewSource);
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
                (this.GetVisualParent<StackPanel>(mobileDeleteButton, "EditMobileCollectionColumnCellStackPanel")
                    ?.Resources["EditingMobileCollectionViewSourceKey"] as CollectionViewSource)?.View as IEditableCollectionViewAddNewItem;
            if (editingMobileCollectionViewSource == null)
                return;

            this.DeleteItemFromCollectionView(editingMobileCollectionViewSource, currentMobileCollectionIndex);
        }


        private void AddressAddButton_Click(object sender, RoutedEventArgs e)
        {
            IEditableCollectionViewAddNewItem editingAddressCollectionViewSource = (((sender as Button)?.Parent as StackPanel)?.
                Resources["EditingAddressCollectionViewSourceKey"] as CollectionViewSource)?.View as IEditableCollectionViewAddNewItem;

            this.AddItemToCollectionView(editingAddressCollectionViewSource);
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
                (this.GetVisualParent<StackPanel>(addressDeleteButton, "EditAddressCollectionColumnCellStackPanel")
                    ?.Resources["EditingAddressCollectionViewSourceKey"] as CollectionViewSource)?.View as IEditableCollectionViewAddNewItem;
            if (editingAddressCollectionViewSource == null)
                return;

            this.DeleteItemFromCollectionView(editingAddressCollectionViewSource, currentAddressCollectionIndex);
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

            this.LoadCitiesByProvince(editProvinceComboBox.SelectedItem);
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


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void InitializeViewModelsModules()
        {
            this.InitializePhoneBookViewModel();
        }


        private void InitializePhoneBookViewModel()
        {
            this.PhoneBookViewModel = new PhoneBookViewModel(this.PersonDeleteStatus, this.LoadExceptionOccured, this.SaveSuccessed);
            this.PhoneBookViewModel.AnyPersonOperationCanceled += PhoneBookViewModel_AnyPersonOperationCanceled;
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
            this.OtherContentStackPanel.IsEnabled = true;
        }


        private void PhoneBookViewModel_AnyPersonOperationCanceled()
        {
            XceedNS.MessageBox.Show("عملیات ، لغو شد");
            this.OtherContentStackPanel.IsEnabled = true;
        }


        private bool ShowDeleteMessage()
        {
            string deleteMessage = "آیا مطمئن هستید که قصد دارید تا سطر انتخاب شده با اطلاعات زیر را حذف کنید؟\n\n" +
                this.PhoneBookViewModel.GetPersonInfo(this.PhoneBookDataGrid.CurrentItem);
            string deleteCaption = "حذف سطر";

            MessageBoxResult deletionQuestionResult = XceedNS.MessageBox.Show(deleteMessage, deleteCaption, 
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if(deletionQuestionResult == MessageBoxResult.Yes)
                return true;
            else if(deletionQuestionResult == MessageBoxResult.No)
                return false;
            return false;
        }


        private void AddItemToCollectionView(IEditableCollectionViewAddNewItem editingCollectionView)
        {
            if (editingCollectionView == null || editingCollectionView.CanAddNew == false)
                return;

            /// اول از اینکه وقتی میخواهید یه آیتمی را اضافه کنید ، اصولی تر این هست که توسط CollectionView ها این کار را کنید .
            /// دوما با فراخوانی متد AddNew ، یک شیِ پیش فرض مربوط به آیتم های آن CollectionView که در اینجا ، شیِ Address هست ، ایجاد و به کالکشن اصلی مان اضافه میشود .
            /// 
            /// برای مدیریت ذخیره کردن در دیتابیس ، در لایه ی Model ، موقع اضافه شدن آیتم ها به شیِ آن آیتم ، چک کنید که
            /// که اگه مقادیرِ هر پروپرتی از آیتم های آن شی ، مقادیر پیش فرض هست ،
            /// یعنی مثلا برای اشیاء از نوع کلاس ، مقدار null و یا اشیاء از نوع اعداد ، مقدار صفر هست ، در دیتابیس ذخیره نشود .
            /// از این بابت برای هر پروپرتی ، مجزا این کار را انجام دهید چون معمولا به ازای هر پروپرتی ، ستونی مجزا در DataGrid ساخته میشود .
            ///
            /// یعنی مثلا در هندلرِ رویدادی که در کلاس PhoneBook ، به رویداد ObservableCollection.CollectionChanged متصل کردید ،
            /// بررسی کنید که آیا مقدارِ پروپرتیِ مربوط به آن رویداد که اضافه شد (فرضا پروپرتیِ Province به کالکشن اضافه شد ، آیا مقدار این پروپرتی ، مقداری پیش فرض هست یا نه .
            /// اگر مقدار پیش فرض نبود ، فقط در آن صورت در دیتابیس ذخیره کنید .
            ///
            /// 
            /// بعد از متد AddNew ، حتما باید متد CommitNew را برای ثبت مقدار ، فراخوانی کنید .
            /// هر چند بعد از متد AddNew در کد زیر ، هر وقت متد CancelNew را فراخوانی کنید ، مقدار مورد نظر که اضافه شده بود ، حذف میشود
            /// 
            /// اما برای بررسی اعتبار سنجی ، لازم به این کار نیست . چون بعد از فشردنِ دکمه ی اضافه کردن ، کاربر مجبور است که نهایتا دکمه ی enter در روی کیبرد
            /// یا اینکه به هر حال فوکوس را از روی آن سطری که در DataGrid هست ، خارج کند که در این صورت و به هر حال ، رویداد CellEditEnding اجرا میشود
            /// که در نتیجه ی آن ، متد اعتبارسنجی ، اجرا میشود که اگر مقداری که با متد AddNew اضافه کرده بود ، ویرایش نشد ، برای آن سطر ، اخطار نامعتبر میدهد
            /// و کاربر مجبور میشود که مقدار آنرا تصحیح کند وگرنه قابلیت ویرایش سطرهای دیگر را نخواهد داشت .
            editingCollectionView.AddNew();
            /// برای ذخیره شدن ، حتما باید بعد از متد AddNew ، متد CommitNew را فراخوانی کنیم .
            editingCollectionView.CommitNew();
        }


        private void DeleteItemFromCollectionView(IEditableCollectionViewAddNewItem editingCollectionView, int collectionViewIndex)
        {
            if (editingCollectionView == null || editingCollectionView.CanRemove == false)
                return;

            editingCollectionView.RemoveAt(collectionViewIndex);
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private ResultParentType GetVisualParent<ResultParentType>(FrameworkElement searchStartChild, string parentControlName)
            where ResultParentType : FrameworkElement
        {
            FrameworkElement foundedParent = VisualTreeHelper.GetParent(searchStartChild) as FrameworkElement;
            if (foundedParent == null)
                return null;

            /// برای اینکه اگر نوعِ foundedParent از نوعِ FrameworkElement بود ، مقدار null در آن متغییر ذخیره نشود ، مستقیما به نوعِ ResultParentType تبدیل نمیکنیم .
            /// بلکه مجددا مقدار آنرا تبدیل میکنیم .
            ResultParentType convertedFoundedParent = foundedParent as ResultParentType;
            if (convertedFoundedParent != null && convertedFoundedParent.Name == parentControlName)
                return convertedFoundedParent;
            else
            {
                ResultParentType foundedAncestor = this.GetVisualParent<ResultParentType>(foundedParent, parentControlName);
                if (foundedAncestor != null && foundedAncestor.Name == parentControlName)
                    return foundedAncestor;
            }

            return null;
        }


        private void LoadCitiesByProvince(object province)
        {
            this.PhoneBookViewModel.LoadCitiesByProvinceIdCommand.Execute(province);
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
            if(newSelectedDataGridRow == null)
                return;

            if (this.PhoneBookViewModel.HasValuePostalCodeOrAddressDetails(newSelectedItemPerson))
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


        private void PhoneBookDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.Header.ToString() != "ردیف" || e.Column.SortDirection != ListSortDirection.Ascending)
                return;

            e.Handled = true;
            e.Column.SortDirection = ListSortDirection.Descending;
            this.SetCustomSortInDataGridSource();
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


    }


}
