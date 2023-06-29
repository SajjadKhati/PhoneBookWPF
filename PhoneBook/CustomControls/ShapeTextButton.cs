using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Input;
using System.ComponentModel;

namespace PoshtibangirTolo.View.CustomControl
{
    /// <summary>
    /// اتریباتس ای هست که نوع Source و نوع Target را در پیاده سازی اینترفیس IValueConverter تعیین میکنه .
    /// نوع Source ، در اولین پارامتر متد Convert _ پارامترِ value _ و همچنین نوع Target ، در خروجیِ این متد Convert ، مشخص میشه .
    /// این اتریباتس ، اجباری نیست .
    /// </summary>
    [ValueConversion(typeof(Path), typeof(bool))]

    /// <summary>
    /// این کلاس ، کلاسِ دکمه ای هست که هم قابلیت رسم شکل و یا رسم متن و یا هر دو را دارد .
    /// رسم توسط اطلاعات پروپرتیِ ShapeData و رسم متن ، توسط اطلاعات پروپرتیِ Content انجام میشود . بنابراین برای این کلاس ، پروپرتیِ Content را فقط باید مقدار رشته بدید .
    /// رسم ، توسط برنامه نویس و توسط Template باید انجام شود .
    /// 
    /// برای اینکه درون Template ها ، مشخص کنیم که اگه پروپرتی هایی که نام شون به ButtonShape ختم میشه _ و نوع Path دارن_ مقدار شون null هست یا نه _ و در صورت null بودن ، رسم ای انجام نشه _ نیاز به تبدیل کننده یا همون نیاز به پیاده سازی اینترفیس IValueConverter داریم .
    /// </summary>
    public class ShapeTextButton : Button
    {

        #region فیلدهای استاتیکِ DependencyProperty 

        /// <summary>
        /// dependency property مربوط به ShapeTextButton.DefaultButtonShape را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.DefaultButtonShape ، برمیگردونه .
        ///  توضیحات اضافی :
        ///  اگه بخوایم برای یه پروپرتی ای که بصورت DependencyProperty تعریف کردیم ، مقدار پیش فرض مشخص کنیم ، نباید این کار را درون متد سازنده انجام بدیم بلکه باید توسط آرگومانِ typeMetaData در متد DependencyProperty.Register این کار را انجام بدیم .
        public static readonly DependencyProperty DefaultButtonShapeProperty = DependencyProperty.Register(
            "DefaultButtonShape", typeof(Path), typeof(ShapeTextButton),
            new PropertyMetadata(ButtonShape_PropertyChanged));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.MouseEnterButtonShape را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.MouseEnterButtonShape ، برمیگردونه .
        ///  توضیحات اضافی :
        ///  اگه بخوایم برای یه پروپرتی ای که بصورت DependencyProperty تعریف کردیم ، مقدار پیش فرض مشخص کنیم ، نباید این کار را درون متد سازنده انجام بدیم بلکه باید توسط آرگومانِ typeMetaData در متد DependencyProperty.Register این کار را انجام بدیم .
        public static readonly DependencyProperty MouseEnterButtonShapeProperty = DependencyProperty.Register(
            "MouseEnterButtonShape", typeof(Path), typeof(ShapeTextButton),
            new PropertyMetadata(ButtonShape_PropertyChanged));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.MouseDownButtonShape را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.MouseDownButtonShape ، برمیگردونه .
        ///  توضیحات اضافی :
        ///  اگه بخوایم برای یه پروپرتی ای که بصورت DependencyProperty تعریف کردیم ، مقدار پیش فرض مشخص کنیم ، نباید این کار را درون متد سازنده انجام بدیم بلکه باید توسط آرگومانِ typeMetaData در متد DependencyProperty.Register این کار را انجام بدیم .
        public static readonly DependencyProperty MouseDownButtonShapeProperty = DependencyProperty.Register(
            "MouseDownButtonShape", typeof(Path), typeof(ShapeTextButton),
            new PropertyMetadata(ButtonShape_PropertyChanged));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.ControlDisabledButtonShape را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.ControlDisabledButtonShape ، برمیگردونه .
        ///  توضیحات اضافی :
        ///  اگه بخوایم برای یه پروپرتی ای که بصورت DependencyProperty تعریف کردیم ، مقدار پیش فرض مشخص کنیم ، نباید این کار را درون متد سازنده انجام بدیم بلکه باید توسط آرگومانِ typeMetaData در متد DependencyProperty.Register این کار را انجام بدیم .
        public static readonly DependencyProperty ControlDisabledButtonShapeProperty = DependencyProperty.Register(
            "ControlDisabledButtonShape", typeof(Path), typeof(ShapeTextButton),
            new PropertyMetadata(ButtonShape_PropertyChanged));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.MouseEnterContentBrush را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.MouseEnterContentBrush ، برمیگردونه .
        public static readonly DependencyProperty MouseEnterContentBrushProperty = DependencyProperty.Register(
            "MouseEnterContentBrush", typeof(Brush), typeof(ShapeTextButton));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.MouseDownContentBrush را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.MouseDownContentBrush ، برمیگردونه .
        public static readonly DependencyProperty MouseDownContentBrushProperty = DependencyProperty.Register(
            "MouseDownContentBrush", typeof(Brush), typeof(ShapeTextButton));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.ControlDisabledContentBrush را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.ControlDisabledContentBrush ، برمیگردونه .
        public static readonly DependencyProperty ControlDisabledContentBrushProperty = DependencyProperty.Register(
            "ControlDisabledContentBrush", typeof(Brush), typeof(ShapeTextButton));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.IsMouseLeftButtonDown را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.IsMouseLeftButtonDown ، برمیگردونه .
        public static readonly DependencyProperty IsMouseLeftButtonDownProperty = DependencyProperty.Register(
            "IsMouseLeftButtonDown", typeof(bool), typeof(ShapeTextButton));


        /// <summary>
        /// dependency property مربوط به ShapeTextButton.GetContentBrush_ForWhichButtonShapeProperty را مشخص میکند .
        /// </summary>
        /// Returns:
        ///  شناسه را برای پروپرتیِ dependency property ئه ShapeTextButton.GetContentBrush_ForWhichButtonShapeProperty ، برمیگردونه .
        public static readonly DependencyProperty GetContentBrush_ForWhichButtonShapePropertyProperty = DependencyProperty.Register("GetContentBrush_ForWhichButtonShapeProperty", typeof(ButtonShapeProperty), typeof(ShapeTextButton), new PropertyMetadata(ButtonShapeProperty.None));


        #endregion




        #region  پروپرتی ها


        /// <summary>
        /// شکل مربوط به این دکمه برای رسم پیش فرض توسط Style و Template ای که مینویسیم .
        /// مقدار دادن به این پروپرتی ، الزامی هست .
        /// رسم متن ، در پروپرتیِ Content توسط Style و Template  ای که مینویسیم ، ارائه میشه و برنامه نویس باید مقدار این پروپرتیِ Content را ، مقدار string بده .
        /// </summary>
        [Description("شکل پیش فرض را برای رسم در Template مربوطه مشخص میکند .\nمقداردهی این پروپرتی ، برای رسم شکل ، الزامی هست")]
        public Path DefaultButtonShape
        {
            get
            {
                return (Path)this.GetValue(ShapeTextButton.DefaultButtonShapeProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.DefaultButtonShapeProperty, value);
            }
        }


        /// <summary>
        ///  شکل مربوط به این دکمه برای رسم ، زمانی که موس روی این المنت قرار میگیره ، توسط Style و Template ای که مینویسیم .
        /// </summary>
        [Description("شکل برای زمانی که موسِ کاربر ، روی این کنترل قرار میگیرد را در Template مربوطه مشخص میکند .")]
        public Path MouseEnterButtonShape
        {
            get
            {
                return (Path)this.GetValue(ShapeTextButton.MouseEnterButtonShapeProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.MouseEnterButtonShapeProperty, value);
            }
        }


        /// <summary>
        ///  شکل مربوط به این دکمه برای رسم ، زمانی که موس روی این المنت کلیک میکنه ، توسط Style و Template ای که مینویسیم .
        /// </summary>
        [Description("شکل برای زمانی که کاربر روی این کنترل کلیک کرد را در Template مربوطه مشخص میکند .")]
        public Path MouseDownButtonShape
        {
            get
            {
                return (Path)this.GetValue(ShapeTextButton.MouseDownButtonShapeProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.MouseDownButtonShapeProperty, value);
            }
        }


        /// <summary>
        ///  شکل مربوط به این دکمه برای رسم ، زمانی که این المنت ، غیر فعال شد ، توسط Style و Template ای که مینویسیم .
        /// </summary>
        [Description("شکل برای زمانی که کنترل غیر فعال شد را در Template مربوطه مشخص میکند .")]
        public Path ControlDisabledButtonShape
        {
            get
            {
                return (Path)this.GetValue(ShapeTextButton.ControlDisabledButtonShapeProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.ControlDisabledButtonShapeProperty, value);
            }
        }


        /// <summary>
        /// بِراش و رنگ متن ای که وقتی موس روی این المنت میاد
        /// </summary>
        [Description("بِراش و قلمو ی Content برای زمانی که موس کاربر ، روی این کنترل قرار میگیرد را در Template مربوطه مشخص میکند .\nدر صورت مقداردهی پروپرتی Content ، حتما مقداری از نوع String و رشته باید داده شود .")]
        public Brush MouseEnterContentBrush
        {
            get
            {
                return (Brush)this.GetValue(ShapeTextButton.MouseEnterContentBrushProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.MouseEnterContentBrushProperty, value);
            }
        }


        /// <summary>
        /// بِراش و رنگ متن ای که وقتی موس روی این المنت کلیک میکنه
        /// </summary>
        [Description("بِراش و قلمو ی Content برای زمانی که کاربر روی این کنترل کلیک کرد را در Template مربوطه مشخص میکند .\nدر صورت مقداردهی پروپرتی Content ، حتما مقداری از نوع String و رشته باید داده شود .")]
        public Brush MouseDownContentBrush
        {
            get
            {
                return (Brush)this.GetValue(ShapeTextButton.MouseDownContentBrushProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.MouseDownContentBrushProperty, value);
            }
        }


        /// <summary>
        /// بِراش و رنگ متن ای که وقتی این المنت ، غیر فعال میشه
        /// </summary>
        [Description("بِراش و قلمو ی Content برای زمانی که این کنترل غیر فعال شد را در Template مربوطه مشخص میکند .\nدر صورت مقداردهی پروپرتی Content ، حتما مقداری از نوع String و رشته باید داده شود .")]
        public Brush ControlDisabledContentBrush
        {
            get
            {
                return (Brush)this.GetValue(ShapeTextButton.ControlDisabledContentBrushProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.ControlDisabledContentBrushProperty, value);
            }
        }


        /// <summary>
        /// مشخص میکند که آیا روی این المنت ، کلیک چپ انجام شد یا نه .
        /// برای استفاده در عملیات binding در template ، به این پروپرتی نیاز هست .
        /// </summary>
        [Description("عملکردی شبیه پروپرتی IsPressed دارد اما زمانی که موس از روی کنترل خارج میشود هم عمل میکند . ")]
        public bool IsMouseLeftButtonDown
        {
            get
            {
                return (bool)this.GetValue(ShapeTextButton.IsMouseLeftButtonDownProperty);
            }
            private set
            {
                this.SetValue(ShapeTextButton.IsMouseLeftButtonDownProperty, value);
            }
        }


        /// <summary>
        /// این پروپرتی ، در template و style مورد نظر ، استفاده میشود .
        /// </summary>
        [Description("کاربرد این پروپرتی برای زمانی هست که با تغییر نوع استایل ، رنگ و بِراش شکل هایی که با پروپرتی هایی که با نام ButtonShape ختم میشوند ، بصورت پیش فرض تغییر کند .\n\nبه این صورت که همان بِراش هایی که بِراش های Content و متن را تعیین میکند _ مثل پروپرتیِ Foreground و MouseEnterContentBrush و ..._، با مشخص کردن مقدار این پروپرتی ، از همان بِراش ها برای پروپرتی های Fill و Stroke یا هر دوی این پروپرتی ، بِراشِ مربوط به شکل های تعیین شده در پروپرتی های ButtonShape از این بِراش ها استفاده میکند .")]
        public ButtonShapeProperty GetContentBrush_ForWhichButtonShapeProperty
        {
            get
            {
                return (ButtonShapeProperty)this.GetValue(ShapeTextButton.GetContentBrush_ForWhichButtonShapePropertyProperty);
            }
            set
            {
                this.SetValue(ShapeTextButton.GetContentBrush_ForWhichButtonShapePropertyProperty, value);
            }
        }


        #endregion




        #region متدها


        /// <summary>
        /// این متد ، بعد از تغییر مقدار Dependency Property ، اجرا میشود .
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonShape_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnPropertyChanged(d as ShapeTextButton, e.NewValue as Path, e.OldValue as Path);
        }


        /// <summary>
        /// بعد از تغییر مقدار Dependency Property ، این متد اجرا میشود و Parent ئه شی Path ئه مربوط به Dependency Property را به شیِ جاریِ ShapeTextButton
        /// تغییر میدهد تا وقتی Binding ای در شیِ Path بصورت FindAnsestor به شیِ ShapeTextButton انجام شد ، آن Binding ، درست کار کند وگرنه کار نمیکند .
        ///
        /// چون وقتی Binding ای بصورت FindAnsestor برای شی والد انجام میشود ، در درخت منطقی یا همان پروپرتی Parent در آن شی ، شروع به جستجو میکند .
        /// </summary>
        /// <param name="shapeTextButton"></param>
        /// <param name="newButtonShape"></param>
        /// <param name="oldButtonShape"></param>
        private static void OnPropertyChanged(ShapeTextButton shapeTextButton, Path newButtonShape,
            Path oldButtonShape)
        {
            try
            {
                if (oldButtonShape != null && shapeTextButton != null && oldButtonShape.Parent == shapeTextButton)
                {
                    shapeTextButton.RemoveLogicalChild(oldButtonShape);
                }

                if (newButtonShape != null && shapeTextButton != null && newButtonShape.Parent == null)
                {
                    /// وقتی فرزند منطقی ای را به شیِ shapeTextButton اضافه کنیم ،
                    /// باعث میشود تا پروپرتیِ Parent ئه newButtonShape ، به شیِ shapeTextButton تغییر کند .
                    shapeTextButton.AddLogicalChild(newButtonShape);
                }
            }
            catch (Exception exception)
            {
            }
        }




        /// <summary>
        /// برای مقداردهی پروپرتی IsMouseLeftButtonDown ، از این متد استفاده میکنیم .
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.IsMouseLeftButtonDown = true;
            base.OnMouseLeftButtonDown(e);
        }

        /// <summary>
        /// برای مقداردهی پروپرتی IsMouseLeftButtonDown ، از این متد استفاده میکنیم .
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.IsMouseLeftButtonDown = false;
            base.OnMouseLeftButtonUp(e);
        }


        #endregion


    }
}
