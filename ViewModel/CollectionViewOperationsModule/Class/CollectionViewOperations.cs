using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using ViewModel.CollectionViewOperationsModule.Interface;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Data;
using Model.PhoneBookModule.Class;
using ViewModel.CollectionViewOperationsModule.Enum;

namespace ViewModel.CollectionViewOperationsModule.Class
{
    public class CollectionViewOperations : ICollectionViewOperationsAggregator
    {
        public ICommand AddItemToCollectionViewCommand { get; }


        public ICommand DeleteItemFromCollectionViewCommand { get; }


        public ICommand PersonGroupingCommand { get; }




        public PersonGroupingProperties GroupingProperty { get; set; }


        public string FilterFirstName { get; set; }


        public string FilterLastName { get; set; }


        public string FilterMobileNumber { get; set; }


        public string FilterProvince { get; set; }


        public string FilterCity { get; set; }


        public string FilterAddressDetail { get; set; }


        public string FilterPostalCode { get; set; }




        public CollectionViewOperations()
        {
            this.AddItemToCollectionViewCommand = new RelayCommand<IEditableCollectionViewAddNewItem>(this.AddItemToCollectionView);
            this.DeleteItemFromCollectionViewCommand = 
                new RelayCommand<CollectionViewItemDeleteInfo>(this.DeleteItemFromCollectionView);
            this.PersonGroupingCommand = new RelayCommand<ICollectionView>(this.PersonGrouping);
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


        private void DeleteItemFromCollectionView(CollectionViewItemDeleteInfo collectionViewItemDeleteInfo)
        {
            if (collectionViewItemDeleteInfo == null || collectionViewItemDeleteInfo.EditingCollectionView == null || 
                collectionViewItemDeleteInfo.CollectionViewIndex < 0 || 
                collectionViewItemDeleteInfo.EditingCollectionView.CanRemove == false)
                return;

            collectionViewItemDeleteInfo.EditingCollectionView.RemoveAt(collectionViewItemDeleteInfo.CollectionViewIndex);
        }


        private void PersonGrouping(ICollectionView groupingCollectionView)
        {
            if (groupingCollectionView == null)
                return;

            groupingCollectionView.GroupDescriptions.Clear();
            string propertyName = this.GroupingProperty.ToString();

            switch (this.GroupingProperty)
            {
                case PersonGroupingProperties.FirstName :
                {
                    groupingCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(propertyName));
                    break;
                }
                case PersonGroupingProperties.LastName :
                {
                    groupingCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(propertyName));
                    break;
                }
                case PersonGroupingProperties.FirstProvince :
                {
                    propertyName = "Addresses";
                    groupingCollectionView.GroupDescriptions.Add(
                        new PropertyGroupDescription(propertyName, new FirstProvinceGroupingConverter()));
                    break;
                }
                case PersonGroupingProperties.FirstCity:
                {
                    propertyName = "Addresses";
                    groupingCollectionView.GroupDescriptions.Add(
                        new PropertyGroupDescription(propertyName, new FirstCityGroupingConverter()));
                    break;
                }
            }
        }


        public void PersonsCollectionViewSource_FirstNameFilter(object sender, FilterEventArgs e)
        {
            Person currentPersonToFilter = e.Item as Person;
            if (currentPersonToFilter == null || currentPersonToFilter.FirstName == null) 
                return;

            if (currentPersonToFilter.FirstName.Contains(this.FilterFirstName) == false)
                /// برای اینکه چندین شرط را روی چندین فیلد یا پروپرتی اعمال کنیم و هر کدوم توی رویداد متفاوتی هستند ،
                /// پس فقط مقدار روپرتیِ Accepted را باید روی شرطِ false تنظیم کنیم تا با اجرا در رویدادهای دیگه ، تداخل پیدا نکنه .
                e.Accepted = false;
        }


        public void PersonsCollectionViewSource_LastNameFilter(object sender, FilterEventArgs e)
        {
            Person currentPersonToFilter = e.Item as Person;
            if (currentPersonToFilter == null || currentPersonToFilter.LastName == null)
                return;

            if (currentPersonToFilter.LastName.Contains(this.FilterLastName) == false)
                e.Accepted = false;
        }


        public void PersonsCollectionViewSource_MobileNumbersFilter(object sender, FilterEventArgs e)
        {
            Person currentPersonToFilter = e.Item as Person;
            if (currentPersonToFilter == null)
                return;

            IList<string> mobileNumbers = new List<string>(currentPersonToFilter.Mobiles.Count);
            foreach (Mobile mobile in currentPersonToFilter.Mobiles)
            {
                mobileNumbers.Add(mobile.MobileNumber);
            }

            if (this.AllItemsContain(mobileNumbers, this.FilterMobileNumber) == false)
                e.Accepted = false;
        }


        public void PersonsCollectionViewSource_ProvincesFilter(object sender, FilterEventArgs e)
        {
            Person currentPersonToFilter = e.Item as Person;
            if (currentPersonToFilter == null)
                return;

            IList<string> provinces = new List<string>(currentPersonToFilter.Addresses.Count);
            foreach (Address address in currentPersonToFilter.Addresses)
            {
                if (address.Province != null)
                    provinces.Add(address.Province);
            }

            if (this.AllItemsContain(provinces, this.FilterProvince) == false || provinces.Count < 1)
                e.Accepted = false;
        }


        public void PersonsCollectionViewSource_CitiesFilter(object sender, FilterEventArgs e)
        {
            Person currentPersonToFilter = e.Item as Person;
            if (currentPersonToFilter == null)
                return;

            IList<string> cities = new List<string>(currentPersonToFilter.Addresses.Count);
            foreach (Address address in currentPersonToFilter.Addresses)
            {
                if (address.City != null)
                    cities.Add(address.City);
            }

            if (this.AllItemsContain(cities, this.FilterCity) == false || cities.Count < 1)
                e.Accepted = false;
        }


        public void PersonsCollectionViewSource_AddressDetailsFilter(object sender, FilterEventArgs e)
        {
            Person currentPersonToFilter = e.Item as Person;
            if (currentPersonToFilter == null)
                return;

            IList<string> addressDetails = new List<string>(currentPersonToFilter.Addresses.Count);
            foreach (Address address in currentPersonToFilter.Addresses)
            {
                if (address.AddressDetail != null)
                    addressDetails.Add(address.AddressDetail);
            }

            if (this.AllItemsContain(addressDetails, this.FilterAddressDetail) == false || addressDetails.Count < 1)
                e.Accepted = false;
        }


        public void PersonsCollectionViewSource_PostalCodesFilter(object sender, FilterEventArgs e)
        {
            Person currentPersonToFilter = e.Item as Person;
            if (currentPersonToFilter == null)
                return;

            IList<string> postalCodes = new List<string>(currentPersonToFilter.Addresses.Count);
            foreach (Address address in currentPersonToFilter.Addresses)
            {
                if (address.PostalCode != null && address.PostalCode.HasValue == true)
                    postalCodes.Add(address.PostalCode.Value.ToString());
            }

            if (this.AllItemsContain(postalCodes, this.FilterPostalCode) == false || postalCodes.Count < 1)
                e.Accepted = false;
        }


        private bool AllItemsContain(IList<string> items, string containText)
        {
            if(items == null)
                return false;

            foreach (string item in items)
            {
                bool result = item.Contains(containText);
                if (result == true) 
                    return true;
            }
            return false;
        }


    }
}
