﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.PhoneBookViewModelModule.Interface
{
    public interface IBindingRelatedOperations
    {
        Tuple<IEnumerable, object> GetPhoneBookBindingOperationsInfo();

    }
}