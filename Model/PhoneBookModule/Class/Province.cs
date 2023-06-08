using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    public class Province
    {
        public int Id { get;}


        public string ProvinceName { get;}


        /// <summary>
        ///
        /// ارتباط این کلاسِ جاری ، با کلاسی که در این پروپرتی مشخص شد ، ارتباط Composition هست . <br/>
        /// چون شی این کلاسِ جاری ، مستقل از شیِ کلاسی که در نوعِ این پروپرتی مشخص شد ، نیست .
        /// </summary>
        public IList<City> Cities { get; }




        public Province(int id, string provinceName)
        {
            this.Id = id;
            this.ProvinceName = provinceName;
            this.Cities = new ObservableCollection<City>();
        }


    }
}
