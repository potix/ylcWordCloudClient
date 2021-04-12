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
            FontMaxSizeTextBox.DataContext = setting;
            FontMinSizeTextBox.DataContext = setting;
            WidthTextBox.DataContext = setting;
            HeightTextBox.DataContext = setting;
            BackgroundColorRTextBox.DataContext = setting;
            BackgroundColorBTextBox.DataContext = setting;
            BackgroundColorGTextBox.DataContext = setting;
            BackgroundColorATextBox.DataContext = setting;
            ColorsDataGrid.DataContext = setting;
            URITextBOX.DataContext = setting;
        }

        private void ConnectButtonClick(object sender, EventArgs e)
        {
            if (setting.VideoId == null || setting.VideoId == "")  {
                MessageBox.Show("VideoIdが入力されていません");
                return;
            }
            Debug.Print(setting.Dump());
            ViewWindow viewWindow = new ViewWindow();
            viewWindow.Show();
            viewWindow.ViewWordCloud(setting);
        }


    }
}
