using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ylccProtocol;

namespace ylcWordCloudClient
{
    public class TargetValue
    {
        public string TargetLabel { get; set; }
        public Target Target { get; set; }
    }

    public class Setting
    {

        public string VideoId { get; set; }

        public TargetValue TargetValue { get; set; }

        public ObservableCollection<TargetValue> TargetValues { get; set; }

        public int FontMaxSize { get; set; }

        public int FontMinSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public uint BackgroundColorR { get; set; }
        public uint BackgroundColorG { get; set; }
        public uint BackgroundColorB { get; set; }
        public uint BackgroundColorA { get; set; }

        public ObservableCollection<Color> Colors { get; set; }

        public string URI { get; set; }

        public Setting()
        {
            TargetValues = new ObservableCollection<TargetValue>();
            TargetValue defaultTargetValue = new TargetValue { TargetLabel = "all user", Target = Target.AllUser };
            TargetValues.Add(defaultTargetValue);
            TargetValues.Add(new TargetValue { TargetLabel = "owner and moderator and sponsor", Target = Target.OwnerModeratorSponsor });
            TargetValues.Add(new TargetValue { TargetLabel = "owner and moderator", Target = Target.OwnerModerator });
            TargetValue = defaultTargetValue;
            FontMaxSize = 128;
            FontMinSize = 16;
            Width = 1024;
            Height = 512;
            BackgroundColorR = 255;
            BackgroundColorG = 255;
            BackgroundColorB = 255;
            BackgroundColorA = 200;
            Colors = new ObservableCollection<Color>();
            Colors.Add(new Color() { R = 255, G = 0, B = 0, A = 255});
            Colors.Add(new Color() { R = 0, G = 255, B = 0, A = 255});
            Colors.Add(new Color() { R = 0, G = 0, B = 255, A = 255});
            Colors.Add(new Color() { R = 255, G = 255, B = 0, A = 255 });
            Colors.Add(new Color() { R = 0, G = 255, B = 255, A = 255 });
            Colors.Add(new Color() { R = 255, G = 0, B = 255, A = 255 });
            URI = "https://127.0.0.1:12345";
        }

        public Color GetBackgroundColor()
        {
            return new Color()
            {
                R = BackgroundColorR,
                G = BackgroundColorG,
                B = BackgroundColorB,
                A = BackgroundColorA,
            };
        }

        public string Dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("VideoId: "+ VideoId + "\n");
            sb.Append("TargetValue: " + TargetValue.TargetLabel + "\n");
            sb.Append("FontMaxSize: " + FontMaxSize + "\n");
            sb.Append("FontMinSize: " + FontMinSize + "\n");
            sb.Append("Width: " + Width + "\n");
            sb.Append("Height: " + Height + "\n");
            sb.Append("BackgroundColor: " + BackgroundColorR +"," + BackgroundColorG + ","  + BackgroundColorB + ","  + BackgroundColorA + "," + "\n");
            foreach (var color in Colors)
            {
                sb.Append("Color" + Colors.IndexOf(color) + ": " + color.R + "," + color.G + "," + color.B + "," + color.A + "," + "\n");

            }
            return sb.ToString();
        }

    }
}
