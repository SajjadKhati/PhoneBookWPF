using Model.ValidationModule.Class;
using Model.ValidationModule.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Model.PhoneBookModule.Class;
using ViewModel.PhoneBookViewModelModule.Interface;
using System.Windows.Data;

namespace ViewModel.PhoneBookValidationRuleModule
{
    public class PhoneBookValidationRule : ValidationRule
    {
        public IPhoneBookViewModelAggregator PhoneBookViewModel { get; set; }


        public bool AllowCallAddOrUpdate{ get; set; }


        public bool ShouldCallAdd{ get; set; }




        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (this.AllowCallAddOrUpdate == false)
                    return ValidationResult.ValidResult;

                Person person = (value as BindingGroup)?.Items?.OfType<Person>().FirstOrDefault();
                if (person == null)
                    return new ValidationResult(false, "شیِ Person بدست نیامد");

                IValidator personValidator = new PersonValidator(person);
                /// دقت شود که چون ماژول IValidator ، اگر ارزیابی ای که برای اعتبارسنجی انجام میدهد ، معتبر نباشد ، یک استثنا پرتاب میکند ،
                /// پس لازم نیست که بعد از فراخوانی متد IsValid ، نتیجه اش را بررسی کنیم که درست است یا نه چون اگر ارزیابی ، نامعتبر باشد ، قطعا در بخش catch میرود .
                /// 
                /// پس ، بعد از این متد ، قطعا اعتبارسنجی ، درست است و نیاز به بررسی برای نامعتبر بودن ، نیست .
                personValidator.IsValid();

                bool result;
                if (this.ShouldCallAdd == true)
                    result = this.PhoneBookViewModel.AddPerson(person);
                else
                    result = this.PhoneBookViewModel.EditPerson(person);

                if (result == false)
                    return new ValidationResult(false, "ذخیره سازی ، با شکست مواجه شد .");
                else
                {
                    /// فقط و فقط در صورت true بودن متغییر result ، متغییرهای AllowCallAddOrUpdate و ShouldCallAdd را
                    /// ریست کنید و مقدارشان را false کنید .
                    this.ResetFlagProperties();
                    return ValidationResult.ValidResult;
                }
            }
            catch (Exception exception)
            {
                return new ValidationResult(false, "خطای اعتبار سنجی : \n" + exception.Message);
            }
        }


        private void ResetFlagProperties()
        {
            this.AllowCallAddOrUpdate = false;
            this.ShouldCallAdd = false;
        }


    }
}
