using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ylcWordCloudClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Setting setting = new Setting();

        public MainWindow()
        {
            InitializeComponent();
            VideoIdTextBox.DataContext = setting;
            TargetComboBox.DataContext = setting;
            MessageLimitTextBox.DataContext = setting;
            FontMaxSizeTextBox.DataContext = setting;
            FontMinSizeTextBox.DataContext = setting;
            WidthTextBox.DataContext = setting;
            HeightTextBox.DataContext = setting;
            BackgroundColorTextBox.DataContext = setting;
            BackgroundColorBorder.DataContext = setting;
            FontColorsDataGrid.DataContext = setting;
            URITextBOX.DataContext = setting;
            InsecureCheckBox.DataContext = setting;
        }

        private void RemoveColorClick(object sender, EventArgs e)
        {
            if (FontColorsDataGrid.SelectedIndex == -1)
            {
                return;
            }
            setting.FontColors.Remove(setting.FontColors[FontColorsDataGrid.SelectedIndex]);
            FontColorsDataGrid.SelectedIndex = -1;
        }

        private void ConnectButtonClick(object sender, EventArgs e)
        {
            if (setting.VideoId == null || setting.VideoId == "")  {
                MessageBox.Show("VideoIdが入力されていません");
                return;
            }
            ViewWindow viewWindow = new ViewWindow();
            viewWindow.Show();
            viewWindow.ViewWordCloud(setting);
        }

    }
}
