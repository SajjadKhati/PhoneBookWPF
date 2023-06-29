using CommunityToolkit.Mvvm.Input;
using Model.PhoneBookModule.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModel.PresentationLogicModule.Enum;
using ViewModel.PresentationLogicModule.Interface;

namespace ViewModel.PresentationLogicModule
{
    public class PresentationLogicModule : ISettingsPresentationLogic
    {
        public static (double centerX, double centerY) GetWindowCenterPoint(double windowWidth, double windowHeight)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double centerX = (screenWidth - windowWidth) / 2;
            double centerY = (screenHeight - windowHeight) / 2;
            return (centerX, centerY);
        }


        public static bool HasValuePostalCodeOrAddressDetails(object person)
        {
            Person personConverted = person as Person;
            if (personConverted == null)
                return false;

            foreach (Address address in personConverted.Addresses)
            {
                if (HasValuePostalCodeOrAddressDetailsForSingleAddress(address) == true)
                    return true;
            }
            return false;
        }


        private static bool HasValuePostalCodeOrAddressDetailsForSingleAddress(Address address)
        {
            if ((address.PostalCode.HasValue == true && address.PostalCode > 0) || string.IsNullOrEmpty(address.AddressDetail) == false)
                return true;

            return false;
        }


        public static string GetPersonDeleteMessage(object person)
        {
            Person personConverted = person as Person;
            if (personConverted == null)
                return "";

            string message = "آیا مطمئن هستید که قصد دارید تا سطر انتخاب شده با اطلاعات زیر را حذف کنید؟\n\n";
            message = (string.IsNullOrEmpty(personConverted.FirstName) == false) ? "نام  :  " + personConverted.FirstName : "";
            message += (string.IsNullOrEmpty(personConverted.LastName) == false) ? "\nنام خانوادگی  :  " + personConverted.LastName : "";
            message += (personConverted.Mobiles.Count > 0 && personConverted.Mobiles[0].MobileNumber != null) ?
                "\nاولین شماره همراه  :  " + personConverted.Mobiles[0].MobileNumber : "";

            return message;
        }




        public ICommand ChangeApplicationStyleCommand { get; }



        public PresentationLogicModule()
        {
            this.ChangeApplicationStyleCommand = new RelayCommand<ValueTuple<ResourceDictionary, SelectedStyle>>(
                this.ChangeApplicationStyle);
        }




        public void ChangeApplicationStyle((ResourceDictionary ApplicationResource, SelectedStyle SelectedStyle) changeStyleInfo)
        {
            if(changeStyleInfo.ApplicationResource == null)
                return;

            string styleFileName = changeStyleInfo.SelectedStyle.ToString();
            /// PhoneBook در ابتدای متن زیر ، همان نام اسمبلی در لایه ی View هست .
            string fullUri = "Resource/XamlResource/Style_Template/" + styleFileName + "Resource.xaml";
            changeStyleInfo.ApplicationResource.MergedDictionaries[1].Source = new Uri(fullUri, UriKind.Relative);
        }


    }
}
