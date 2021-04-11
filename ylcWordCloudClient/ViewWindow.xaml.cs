using Grpc.Net.Client;
using System;
using System.Collections.Generic;
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
        public ViewWindow()
        {
            InitializeComponent();
        }

        private async void CallGrpc ()
        {
            using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
            ylcc.ylccClient client = new ylccProtocol.ylcc.ylccClient(channel);
            StartCollectionWordCloudMessagesRequest request = new StartCollectionWordCloudMessagesRequest
            {
                VideoId = "ssss",
            };
            StartCollectionWordCloudMessagesResponse response = await client.StartCollectionWordCloudMessagesAsync(request);
        }
    }
}
