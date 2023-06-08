using Model.PhoneBookModule.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Interface
{
    public interface IProvince
    {
        IList<Province> Provinces { get; }


        Task LoadProvincesAsync();


        void LoadCitiesByProvinceId(int  provinceId);
    }
}
