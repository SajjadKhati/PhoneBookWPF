using Model.NotifyPropertyChangedModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    public class Address : NotifyPropertyChanged
    {
        private int _id;


        private string _province;


        private string _city;


        private string _addressDetail;


        private long? _postalCode;




        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                if (this._id != value)
                {
                    this._id = value;
                    this.OnPropertyChanged();
                }
            }
        }


        public string Province
        {
            get
            {
                return this._province;
            }
            set
            {
                if (this._province != value)
                {
                    this._province = value;
                    this.OnPropertyChanged();
                }
            }
        }


        public string City
        {
            get
            {
                return this._city;
            }
            set
            {
                if (this._city != value)
                {
                    this._city = value;
                    this.OnPropertyChanged();
                }
            }
        }


        public string AddressDetail
        {
            get
            {
                return this._addressDetail;
            }
            set
            {
                if (this._addressDetail != value)
                {
                    this._addressDetail = value;
                    this.OnPropertyChanged();
                }
            }
        }


        public long? PostalCode
        {
            get
            {
                return this._postalCode;
            }
            set
            {
                if (this._postalCode != value)
                {
                    this._postalCode = value;
                    this.OnPropertyChanged();
                }
            }
        }


        /// <summary>
        ///
        /// ارتباط این کلاسِ جاری ، با کلاسی که در این پروپرتی مشخص شد ، ارتباط Accociation هست . <br/>
        /// چون در این برنامه ، یک شی آدرس جوری در نظر گرفته شد که یک آدرس میتواند مالک خاصی نداشته باشد و یا در واقع چندین مالک داشته باشد یا بصورت اشتراکی باشد .
        /// مثلا یک آدرس منزل ، میتواند هم برای یک زن ، شوهر ، فرزند و ... باشد .<br/>
        /// بنابراین در این صورت ، شخص و آدرس ، فقط با هم رابطه دارند و مالک چیزی نیستند و همچنین عدم وجود یکی از آنها ، بر شی کلاسِ دیگری تاثیری ندارد .
        /// </summary>
        public IList<Person> Persons { get; }




        public Address()
        {
            /// برای اینکه مشکلی زمان اضافه کردن آیتم ها به این کالکشن های Mobiles و Addresses در ui ئه برنامه پیش نیاید ، کالکشن های اینها را فقط خواندنی کردیم و
            /// فقط یکبار در سازنده ی این کلاس ، مقدار دادیم . برای بقیه ی کلاس های زیر مجموعه ی این کلاس مثل کلاس Address هم همینطور عمل کردیم .
            this.Persons = new ObservableCollection<Person>();
        }


    }
}
