using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            using (var mem = new MemoryStream(pngImageData))
            {
                mem.Position = 0;
                PngBitmapDecoder decoder = new PngBitmapDecoder(mem, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource = decoder.Frames[0];
                WordCloudImage.Source = bitmapSource;
                WordCloudImage.Stretch = Stretch.None;
            }
        }

        public void ViewWordCloud(Setting setting)
        {
            try
            {

                this.Width = setting.Width;
                this.Height = setting.Height + 16;
                WordCloudImage.Width = setting.Width;
                WordCloudImage.Height = setting.Height;
                using GrpcChannel channel = GrpcChannel.ForAddress(setting.URI);
                ylcc.ylccClient client = new ylcc.ylccClient(channel);
                YlccProtocol protocol = new YlccProtocol();
                StartCollectionWordCloudMessagesRequest startCollectionWordCloudMessagesRequest = protocol.BuildStartCollectionWordCloudMessagesRequest(setting.VideoId);
                StartCollectionWordCloudMessagesResponse startCollectionWordCloudMessagesResponse = client.StartCollectionWordCloudMessages(startCollectionWordCloudMessagesRequest);
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
                    GetWordCloudResponse getWordCloudResponse = client.GetWordCloud(getWordCloudRequest);
                    if (getWordCloudResponse.Status.Code != Code.Success)
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
                    if (getWordCloudResponse.MimeType == "image/png")
                    {
                        this.SetPngImage(getWordCloudResponse.Data.ToByteArray());
                    }
                }
            }
            catch (Grpc.Core.RpcException e)
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
