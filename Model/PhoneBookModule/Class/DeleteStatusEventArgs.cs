using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    public class DeleteStatusEventArgs : EventArgs
    {
        public bool IsDeletionSuccessed { get; }


        public Person Person{ get; }


        public Exception DeletionException { get; }




        public DeleteStatusEventArgs(bool isDeletionSuccessed, Person person, Exception deletionException)
        {
            this.IsDeletionSuccessed = isDeletionSuccessed;
            this.Person = person;
            this.DeletionException = deletionException;
        }


    }
}
