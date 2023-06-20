using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshtibangirTolo.View.CustomControl
{
    /// <summary>
    /// تعیین میکند که کدام یک از پروپرتیِ های Stroke یا Fill یا هر دو یا هیچ کدام از انها ، در مقدار بازگشتیِ پروپرتی هایی که با ButtonShape درون کلاس ShapeTextButton ختم میشوند ، میتوانند مقدارِ بِراش شان را از بِراش پیش فرضی که پروپرتی هایی که با نامِ ContentBrush ختم میشوند ، یعنی از پروپرتی های Foreground و MouseEnterContentBrush و ... دریافت کنند 
    /// این enum ، توسط پروپرتیِ ShapeTextButton.GetContentBrush_ForWhichButtonShapeProperty استفاده میشود .
    /// برای این enum ، عملگر بیتی را بکار نبرید .
    /// </summary>
    public enum ButtonShapeProperty
    {
        //
        // Summary:
        // پروپرتی های Stroke و Fill مربوط به اعضای بازگشتیِ پروپرتی هایی که با نامِ ButtonShape درون کلاس ShapeTextButton ختم میشوند ، از مقادیری که در همان پروپرتی ها تعیین شده ، استفاده میکنند . 
        None = -1,
        //
        // Summary:
        //     پروپرتی Stroke مربوط به اعضای بازگشتیِ پروپرتی هایی که با نامِ ButtonShape درون کلاس ShapeTextButton ختم میشوند ، از بِراش های پیش فرض مربوط به پروپرتی هایی که با نام ContentBrush ختم میشوند ، استفاده میکند .
        Stroke = 2,
        //
        // Summary:
        //     پروپرتی Fill مربوط به اعضای بازگشتیِ پروپرتی هایی که با نامِ ButtonShape درون کلاس ShapeTextButton ختم میشوند ، از بِراش های پیش فرض مربوط به پروپرتی هایی که با نام ContentBrush ختم میشوند ، استفاده میکند .
        Fill = 4,
        //
        // Summary:
        //     هم پروپرتی  Stroke و هم Fill که مربوط به اعضای بازگشتیِ پروپرتی هایی که با نامِ ButtonShape درون کلاس ShapeTextButton ختم میشوند ، از بِراش های پیش فرض مربوط به پروپرتی هایی که با نام ContentBrush ختم میشوند ، استفاده میکنند .
        Both = 8
    }
}
