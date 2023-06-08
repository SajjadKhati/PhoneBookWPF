using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    public class City
    {
        public int Id { get; }


        public string CityName { get; }




        public City(int id, string cityName)
        {
            this.Id = id;
            this.CityName = cityName;
        }


    }
}
