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
using System.Windows.Shapes;
using ViewModel.PresentationLogicModule.Enum;
using ViewModel.PresentationLogicModule.Interface;

namespace PhoneBook.Window
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : System.Windows.Window
    {
        private readonly ISettingsPresentationLogic _settingsPresentationLogic;




        public SettingsWindow(MainWindow mainWindowForSelectedStyle)
        {
            InitializeComponent();

            this.DataContext = mainWindowForSelectedStyle;
            this._settingsPresentationLogic = Application.Current.Resources["PresentationLogicModuleKey"] as ISettingsPresentationLogic;
        }




        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void SelectStyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox selectStyleComboBox = sender as ComboBox;
            /// اگر مقدار _settingsPresentationLogic برابر null بود ، یعنی قبل از اینکه اجرای متد InitializeComponent که در بدنه ی متد سازنده فراخوانی شده بود ، تمام شود ،
            /// این رویداد فراخوانی شد . یعنی زمانی که کاربر ، خودش مقداری را انتخاب نکرد . پس در این صورت نباید کاری انجام شود .
            if (this._settingsPresentationLogic == null || selectStyleComboBox == null || 
                selectStyleComboBox.SelectedIndex < 0 || Application.Current.Resources == null)
                return;

            var changeStyleInfo = (ApplicationResource : Application.Current.Resources, 
                SelectedStyle : (selectStyleComboBox.SelectedIndex == 0)? SelectedStyle.DarkStyle : SelectedStyle.LightStyle);
            this._settingsPresentationLogic.ChangeApplicationStyleCommand.Execute(changeStyleInfo);
        }


    }
}
