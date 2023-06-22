using Model.NotifyPropertyChangedModule;
using Model.PhoneBookModule.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    public class Person : NotifyPropertyChanged, IEditableObject
    {
        private bool onEditFlag;


        private Person copyPersonForCancelEdit;




        public event EventHandler<CancelOperationType> OperationCanceled;




        private int _id;


        private string _firstName;


        private string _lastName;




        public int Id
        {
            get
            {
                return _id;
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


        public string FirstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                if (this._firstName != value)
                {
                    this._firstName = value;
                    this.OnPropertyChanged();
                }
            }
        }


        public string LastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                if (this._lastName != value)
                {
                    this._lastName = value;
                    this.OnPropertyChanged();
                }
            }
        }


        /// <summary>
        ///
        /// ارتباط این کلاسِ جاری ، با کلاسی که در این پروپرتی مشخص شد ، ارتباط Aggregation هست . <br/>
        /// چون شی این کلاسِ جاری ، مستقل از شیِ کلاسی که در نوعِ این پروپرتی مشخص شد ، هست .
        /// </summary>
        public IList<Mobile> Mobiles { get; }


        /// <summary>
        ///
        /// ارتباط این کلاسِ جاری ، با کلاسی که در این پروپرتی مشخص شد ، ارتباط Accociation هست . <br/>
        /// چون در این برنامه ، یک شی آدرس جوری در نظر گرفته شد که یک آدرس میتواند مالک خاصی نداشته باشد و یا در واقع چندین مالک داشته باشد یا بصورت اشتراکی باشد .
        /// مثلا یک آدرس منزل ، میتواند هم برای یک زن ، شوهر ، فرزند و ... باشد .<br/>
        /// بنابراین در این صورت ، شخص و آدرس ، فقط با هم رابطه دارند و مالک چیزی نیستند و همچنین عدم وجود یکی از آنها ، بر شی کلاسِ دیگری تاثیری ندارد .
        /// </summary>
        public IList<Address> Addresses { get; }




        public Person()
        {
            /// برای اینکه مشکلی زمان اضافه کردن آیتم ها به این کالکشن های Mobiles و Addresses در ui ئه برنامه پیش نیاید ، کالکشن های اینها را فقط خواندنی کردیم و
            /// فقط یکبار در سازنده ی این کلاس ، مقدار دادیم . برای بقیه ی کلاس های زیر مجموعه ی این کلاس مثل کلاس Address هم همینطور عمل کردیم .
            this.Mobiles = new ObservableCollection<Mobile>();
            this.Addresses = new ObservableCollection<Address>();
        }


        ////////////////////////////////////////////////////////////


        public void BeginEdit()
        {
            if (this.onEditFlag == true)
                return;

            this.onEditFlag = true;
            this.copyPersonForCancelEdit = PersonCloner.DeepClonePerson(this);
        }


        public void EndEdit()
        {
            if(this.onEditFlag == false)
                return;

            this.onEditFlag = false;
            this.copyPersonForCancelEdit = null;
        }


        public void CancelEdit()
        {
            if (this.onEditFlag == false)
                return;

            this.onEditFlag = false;
            if (this.copyPersonForCancelEdit == null)
                return;

            PersonCloner.UpdatePerson(this.copyPersonForCancelEdit, this);
            CancelOperationType cancelOperationType;
            if (string.IsNullOrEmpty(this.copyPersonForCancelEdit.FirstName) == true)
                cancelOperationType = CancelOperationType.AddCanceled;
            else
                cancelOperationType = CancelOperationType.EditCanceled;
            this.OperationCanceled?.Invoke(this, cancelOperationType);
        }


    }
}
