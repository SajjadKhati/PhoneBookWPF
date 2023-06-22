using Model.NotifyPropertyChangedModule;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    public class Mobile : NotifyPropertyChanged
    {
        private int _id;


        private string _mobileNumber;




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


        public string MobileNumber
        {
            get
            {
                return this._mobileNumber;
            }
            set
            {
                if (this._mobileNumber != value)
                {
                    this._mobileNumber = value;
                    this.OnPropertyChanged();
                }
            }
        }


        internal Mobile ShallowCopy()
        {
            return this.MemberwiseClone() as Mobile;
        }


    }
}
