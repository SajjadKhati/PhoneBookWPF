using DataAccess.EntityModule.Class.Entity;
using DataAccess.ExceptionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    internal static class ProvinceMapper
    {
        internal static async Task<IList<Province>> MapScalarPropertiesToProvinceListAsync(IList<ProvinceEntity> provinceEntities)
        {
            return await Task.Factory.StartNew<IList<Province>>((object taskProvinceEntities) =>
            {
                IList<ProvinceEntity> taskProvinceEntitiesConverter = (taskProvinceEntities as IList<ProvinceEntity>) ?? 
                    throw new ArgumentNullException(nameof(taskProvinceEntities), ExceptionMessage.argumentNullExceptionMessage);

                return MapScalarPropertiesToProvinceList(taskProvinceEntitiesConverter);
            }, provinceEntities);
        }


        internal static IList<Province> MapScalarPropertiesToProvinceList(IList<ProvinceEntity> provinceEntities)
        {
            IList<Province> provinceList = new List<Province>();

            foreach (ProvinceEntity provinceEntity in provinceEntities)
            {
                Province province = MapScalarPropertiesToProvince(provinceEntity);
                if (province != null)
                    provinceList.Add(province);
            }

            return provinceList;
        }


        internal static Province MapScalarPropertiesToProvince(ProvinceEntity provinceEntity)
        {
            return new Province(provinceEntity.Id, provinceEntity.ProvinceName);
        }


        internal static void AddCitiesToProvince(ICollection<CityEntity> cityEntities, IList<City> provinceCities)
        {
            cityEntities = cityEntities ?? throw new ArgumentNullException(nameof(cityEntities),
                ExceptionMessage.argumentNullExceptionMessage);
            provinceCities = provinceCities ?? throw new ArgumentNullException(nameof(provinceCities),
                ExceptionMessage.argumentNullExceptionMessage);

            provinceCities.Clear();
            foreach (CityEntity cityEntity in cityEntities)
            {
                provinceCities.Add(new City(cityEntity.Id, cityEntity.CityName));
            }

        }


    }
}
