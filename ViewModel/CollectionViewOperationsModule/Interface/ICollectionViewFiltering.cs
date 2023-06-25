using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ViewModel.CollectionViewOperationsModule.Interface
{
    public interface ICollectionViewFiltering
    {
        string FilterFirstName{ get; set; }


        string FilterLastName { get; set; }


        string FilterMobileNumber{ get; set; }


        string FilterProvince{ get; set; }


        string FilterCity { get; set; }


        string FilterAddressDetail { get; set; }


        string FilterPostalCode { get; set; }




        void PersonsCollectionViewSource_FirstNameFilter(object sender, FilterEventArgs e);


        void PersonsCollectionViewSource_LastNameFilter(object sender, FilterEventArgs e);


        void PersonsCollectionViewSource_MobileNumbersFilter(object sender, FilterEventArgs e);


        void PersonsCollectionViewSource_ProvincesFilter(object sender, FilterEventArgs e);


        void PersonsCollectionViewSource_CitiesFilter(object sender, FilterEventArgs e);


        void PersonsCollectionViewSource_AddressDetailsFilter(object sender, FilterEventArgs e);


        void PersonsCollectionViewSource_PostalCodesFilter(object sender, FilterEventArgs e);


    }
}
