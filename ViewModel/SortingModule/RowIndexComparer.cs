using Model.PhoneBookModule.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.SortingModule
{
    public class RowIndexComparer : IComparer
    {
        public IDictionary<object, int> RowIndexPersons { get; set; }




        public int Compare(object x, object y)
        {
            Person xPerson = x as Person;
            Person yPerson = y as Person;
            if(xPerson == null || yPerson == null)
                return 0;
            int xPersonRowIndex = this.GetRowIndexFromPerson(xPerson);
            int yPersonRowIndex = this.GetRowIndexFromPerson(yPerson);
            if (xPersonRowIndex == -1 || yPersonRowIndex == -1)
                return 0;

            if (xPersonRowIndex > yPersonRowIndex)
                return -1;
            else if (xPersonRowIndex < yPersonRowIndex)
                return 1;
            return 0;
        }


        private int GetRowIndexFromPerson(Person person)
        {
            if (this.RowIndexPersons == null)
                return -1;

            if (this.RowIndexPersons.TryGetValue(person, out int rowIndex) == true)
                return rowIndex;
            else
                return -1;
        }

    }
}
