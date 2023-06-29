using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoshtibangirTolo.View.UserControls
{
    /// <summary>
    /// Interaction logic for TitleBarUserControl.xaml
    /// </summary>
    public partial class TitleBarUserControl : UserControl
    {
        #region اعضای استاتیک

        #region DependencyProperty

        public static readonly DependencyProperty TitleBarMainIconGeometryProperty = DependencyProperty.Register("TitleBarMainIconGeometry", typeof(Geometry), typeof(TitleBarUserControl));
        
        public static readonly DependencyProperty TitleBarMainIconImageSourceProperty = DependencyProperty.Register("TitleBarMainIconImageSource", typeof(ImageSource), typeof(TitleBarUserControl));
        
        #endregion

        #endregion




        #region پروپرتی ها

        /// <summary>
        /// شکلِ Geometry ای که تعیین کردیم را در دکمه ی TitleBarMainIconImage یا همون دکمه ی آیکونِ برنامه که در سمت چپ قرار داره ، رسم میکنه .
        /// اگه از شکل های vector میخواین برای رسم آیکون استفاده کنید ، از این پروپرتی استفاده کنید .
        /// اما اگه از شکل های بیت مپ یا icon میخواید برای رسم آیکون استفاده کنید ، از پروپرتیِ TitleBarMainIconImageSource استفاده کنید 
        /// </summary>
        public Geometry TitleBarMainIconGeometry
        {
            get
            {
                return this.GetValue(TitleBarMainIconGeometryProperty) as Geometry;
            }
            set
            {
                this.SetValue(TitleBarMainIconGeometryProperty, value);
            }
        }


        /// <summary>
        /// شکلِ ImageSource یا شکل بیت مپ یا شکل آیکون ای که تعیین کردیم را در دکمه ی TitleBarMainIconImage یا همون دکمه ی آیکونِ برنامه که در سمت چپ قرار داره ، رسم میکنه .
        /// اگه از شکل های بیت مپ یا icon میخواید برای رسم آیکون استفاده کنید ، از این پروپرتی استفاده کنید 
        /// اما اگه از شکل های vector میخواین برای رسم آیکون استفاده کنید ، از  پروپرتیِ TitleBarMainIconGeometry استفاده کنید .
        /// </summary>
        public ImageSource TitleBarMainIconImageSource
        {
            get
            {
                return this.GetValue(TitleBarMainIconImageSourceProperty) as ImageSource;
            }
            set
            {
                this.SetValue(TitleBarMainIconImageSourceProperty, value);
            }
        }


        /// <summary>
        ///لیستی از کنترل هایی که به عنوان کنترل های TitleBar در سمت راست پنل قرار میگیرند .
        ///اعضای این پروپرتی ، در کد xaml در ویندوزها مشخص میشن .
        ///
        ///تذکر مهم اینکه این پروپرتی ، فقط یکبار مصرف هست و جهت استفاده در xaml مناسب هست .
        ///چون بعد از یکبار مقداردهی ، اعضاش پاک میشه . 
        ///بنابراین وقتی در کدهای سی شارپ اگه خواستین از این پروپرتی استفاده کنین ، بجاش از پروپرتیِ Child ئه کنترلِ TitleBarAddControlsDockPanel استفاده کنید .
        /// </summary>
        public UIElementCollection TitleBarAddControls { get; set; }

        #endregion




        #region متدها

        #region متد سازنده

        public TitleBarUserControl()
        {
            InitializeComponent();

            this.InitializeUserControlOperation();
        }

        #endregion




        #region متدهای مقداردهی

        private void InitializeUserControlOperation()
        {
            this.TitleBarAddControls = new UIElementCollection(this, this);
        }

        #endregion




        #region متدها

        /// <summary>
        /// این متد ، کنترل و المنت های TitleBarAddControls را به پنلِ TitleBarAddControlsDockPanel اضافه میکنه .
        /// </summary>
        private void AddControlsToDockPanel()
        {
            /// چون درون حلقه ی Foreach ، میخوایم اعضای پروپرتیِ TitleBarAddControls را ویرایش کنیم و در واقع اعضاش را کم کنیم و
            /// همچنین چون در حلقه ی Foreach ، نمیتونیم روی همون کالکشن و آرایه ای که به عنوان ایتریتور در Foreach استفاده میکنیم ، تغییری ایجاد کنیم 
            /// پس بجای ایتریتور زدن روی پروپرتیِ TitleBarAddControls ، یه کپی از این کالکشن را که توی متغییر زیر یعنی متغییر iterateAddControls ریختیم ،
            /// روی این متغییر ، ایتریتور مون را میسازیم
            List<UIElement> iterateAddControls = this.TitleBarAddControls.Cast<UIElement>().ToList<UIElement>();

            foreach (UIElement addcontrol in iterateAddControls)
            {
                /// در متد سازنده ی UIElementCollection ، مجبور بودیم که حداقل ، پارامتر اول که visualParent باشه را مشخص کنیم . 
                /// در اون متد سازنده که در متد InitializeUserControlOperation ، مقداردهی اش کرده بودیم ، شیِ this که 
                /// شیِ جاریِ TitleBarUserControl بود را به عنوان visualParent براش در نظر گرفتیم .
                /// 
                /// مقداری که در پارامترِ visualParent مشخص کرده بودیم ، به عنوانِ والدِ بصری برای UIElement هایی که در کالکشنِ TitleBarAddControls اضافه میکنیم ، در نظر گرفته میشه
                /// در اینجا ، همه ی شی های UIElement ای که در کدهای xaml در ویندوزها برای پروپرتیِ TitleBarAddControls تعیین میکنیم ، والد بصریِ همه شون ، شیِ جاریِ TitleBarUserControl هست .
                /// 
                /// حالا اگه بخوایم اون شی های UIElement ای که والدِ بصری شون ، شیِ جاریِ TitleBarUserControl هست را به عنوانِ فرزند در کنترلِ TitleBarAddControlsDockPanel اضافه کنیم ، -
                /// چون یک کنترل ، فقط یک والدِ بصری _ یا حتی یک والد منطقی_ میتونه داشته باشه ، و در اینجا هم ، قبلا ، کنترلِ شیِ جاریِ TitleBarUserControl را به عنوان هم والد بصری و هم والد منطقی -
                /// برای اعضا و آیتم های پروپرتیِ TitleBarAddControls در نظر گرفته بودیم ، 
                /// بنابراین نمیشه یک عضو از این پروپرتی ، در یک زمان ، والد بصری اش هم شیِ جاری از این کلاس و هم شیِ TitleBarAddControlsDockPanel باشه . 
                /// 
                /// برای حل این مشکل هم اول باید والدِ اون UIElement ای که درونِ پروپرتیِ TitleBarAddControls وجود داره _ که والدش ، شیِ جاری از این کلاس که شیِ TitleBarUserControl باشه ، بود ، 
                /// قطع ارتباط کنیم و والدش را ازش پس بگیریم .
                /// 
                /// برای این کار ، تنها راه ، به نظر میرسه این هست که اون کنترل را از کالکشنِ TitleBarAddControls ، حذف کنیم تا والدِ بصری اش ، دیگه کنترلِ شیِ جاری از این کلاس که قبلا بود ، نباشه .
                /// بعد از اون ، اون کنترل را فرزندِ پنلِ دیگه مثلا فرزندِ کنترلِ TitleBarAddControlsDockPanel کنیم .
                this.TitleBarAddControls.Remove(addcontrol);
                this.TitleBarAddControlsDockPanel.Children.Add(addcontrol);
            }
        }


        /// <summary>
        /// این متد ، وقتی که روی این شی از یوزر کنترل ، کلیک چپ و بعد درگ انجام شد ، ویندوزی که ریشه ی شیِ جاری از یوزر کنترل باشه را میگیره و جا به جا اش میکنه .
        /// </summary>
        private void MoveParentWindow()
        {
            /// کنترلِ Window ای که والدِ شی جاری ، یعنی والدِ شیِ TitleBarUserControl هست را میگیره تا جا به جاش کنه .
            Window userControlParentWindow = Window.GetWindow(this);
            if (userControlParentWindow != null)
                userControlParentWindow.DragMove();
        }


        #endregion

        #endregion




        #region رویدادها

        private void TitleBarAddControlsDockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.AddControlsToDockPanel();
        }


        private void TitleBarDockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.MoveParentWindow();
        }

        #endregion


    }
}
