using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using ylccProtocol;

namespace ylcWordCloudClient
{
    /// <summary>
    /// ViewWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ViewWindow : Window
    {

        private volatile bool isClosed;

        public ViewWindow()
        {
            InitializeComponent();
            isClosed = false;
        }

        private void SetPngImage(byte[] pngImageData)
        {
            Debug.Print("Length:" + pngImageData.Length);
            using (var mem = new MemoryStream(pngImageData))
            {
                mem.Position = 0;
                BitmapSource bitmapSource = BitmapFrame.Create(mem, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                WordCloudImage.Source = bitmapSource;
            }
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            isClosed = true;
        }

        public async void ViewWordCloud(Setting setting)
        {
            this.Width = setting.Width + 40;
            this.Height = setting.Height + 50;
            WordCloudImage.Width = setting.Width + 20;
            WordCloudImage.Height = setting.Height + 20;
            try
            {
                if (setting.IsInsecure)
                {
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                }
                GrpcChannel channel = GrpcChannel.ForAddress(setting.URI);
                ylcc.ylccClient client = new ylcc.ylccClient(channel);
                YlccProtocol protocol = new YlccProtocol();
                StartCollectionWordCloudMessagesRequest startCollectionWordCloudMessagesRequest = protocol.BuildStartCollectionWordCloudMessagesRequest(setting.VideoId);
                StartCollectionWordCloudMessagesResponse startCollectionWordCloudMessagesResponse = await client.StartCollectionWordCloudMessagesAsync(startCollectionWordCloudMessagesRequest);
                if (startCollectionWordCloudMessagesResponse.Status.Code != Code.Success && startCollectionWordCloudMessagesResponse.Status.Code != Code.InProgress)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("通信エラー\n");
                    sb.Append("URI:" + setting.URI + "\n");
                    sb.Append("VideoId:" + setting.VideoId + "\n");
                    sb.Append("Reason:" + startCollectionWordCloudMessagesResponse.Status.Message + "\n");
                    MessageBox.Show(sb.ToString());
                    this.Close();
                    return;
                }
                while (!isClosed)
                {
                    GetWordCloudRequest getWordCloudRequest = protocol.BuildGetWordCloudRequest(
                        setting.VideoId,
                        setting.TargetValue.Target,
                        setting.Width,
                        setting.Height,
                        setting.FontMaxSize,
                        setting.FontMinSize,
                        setting.Colors,
                        setting.GetBackgroundColor());

                    Debug.Print("request:" + getWordCloudRequest.ToString());

                    GetWordCloudResponse getWordCloudResponse = await client.GetWordCloudAsync(getWordCloudRequest);

                    if (getWordCloudResponse.Status.Code != Code.Success && getWordCloudResponse.Status.Code != Code.InProgress)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("通信エラー\n");
                        sb.Append("URI:" + setting.URI + "\n");
                        sb.Append("VideoId:" + setting.VideoId + "\n");
                        sb.Append("Reason:" + getWordCloudResponse.Status.Message + "\n");
                        MessageBox.Show(sb.ToString());
                        this.Close();
                        break;
                    }
                    if  (getWordCloudResponse.Status.Code == Code.InProgress)
                    {
                        await Task.Delay(5000);
                        continue;
                    }
                    Debug.Print("MineType:" + getWordCloudResponse.MimeType);
                    if (getWordCloudResponse.MimeType == "image/png")
                    {
                        this.SetPngImage(getWordCloudResponse.Data.ToByteArray());
                    }
                    await Task.Delay(5000);
                }
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("通信エラー\n");
                sb.Append("URI:" + setting.URI + "\n");
                sb.Append("VideoId:" + setting.VideoId + "\n");
                sb.Append("Reason:" + e.Message + "\n");
                MessageBox.Show(sb.ToString());
                this.Close();
            }
        }
    }
}
