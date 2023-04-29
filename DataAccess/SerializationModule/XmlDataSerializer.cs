using DataAccess.EntityModule.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataAccess.SerializationModule
{
    /// <summary>
    /// سطح دسترسی این کلاس را چون در T4TextTemplate در دسترس باشد ، بصورت public گرفته شد .
    /// </summary>
    public class XmlDataSerializer
    {
        public static void WriteDataToXmlFile<TData>(TData data, string outputFileName)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(outputFileName))
            using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(xmlWriter))
            {
                /// از کلاس XmlSerializer برای ذخیره و سریالایز کردن اطلاعات در فایل xml استفاده نکردیم چون آن کلاس ، نمیتواند پروپرتی هایی که از نوع اینترفیس هستند یا آنها
                /// را پیاده سازی میکنند را استفاده کند . چون نوع Dictionary ، اینترفیسِ IDictionary را پیاده سازی میکند .
                /// اما کلاس DataContractSerializer به همراه نویسنده ی XmlDictionaryWriter ، برای نوشتن و سریالایز کردنِ نوع دیکشنری ، مشکلی ندارد .
                DataContractSerializer xmlDictionarySerializer = new DataContractSerializer(typeof(TData));
                xmlDictionarySerializer.WriteObject(xmlDictionaryWriter, data);
            }
        }



        public static TData ReadDataFromXmlFile<TData>(string inputFileUri)
        {
            TData tData;
            using (XmlReader xmlReader = XmlReader.Create(inputFileUri))
            using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader))
            {
                /// از کلاس XmlSerializer برای خواندن و سریالایز کردن اطلاعات از فایل xml استفاده نکردیم چون آن کلاس ، نمیتواند پروپرتی هایی که از نوع اینترفیس هستند یا آنها
                /// را پیاده سازی میکنند را استفاده کند . چون نوع Dictionary ، اینترفیسِ IDictionary را پیاده سازی میکند .
                /// اما کلاس DataContractSerializer به همراه نویسنده ی XmlDictionaryReader ، برای خواندن و سریالایز کردنِ نوع دیکشنری ، مشکلی ندارد .
                DataContractSerializer xmlDictionarySerializer = new DataContractSerializer(typeof(TData));
                tData = (TData)xmlDictionarySerializer.ReadObject(xmlDictionaryReader);
            }
            return tData;
        }


    }
}
