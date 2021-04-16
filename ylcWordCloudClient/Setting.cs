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

    public class FontColor
    {
        public string Color { get; set; }
    }


    public class Setting
    {

        public string VideoId { get; set; }

        public TargetValue TargetValue { get; set; }

        public ObservableCollection<TargetValue> TargetValues { get; set; }

        public int MessageLimit { get; set; }

        public int FontMaxSize { get; set; }

        public int FontMinSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string BackgroundColor { get; set; }

        public ObservableCollection<FontColor> FontColors { get; set; }

        public string URI { get; set; }

        public bool IsInsecure { get; set; }

        public Setting()
        {
            TargetValues = new ObservableCollection<TargetValue>();
            TargetValue defaultTargetValue = new TargetValue { TargetLabel = "all user", Target = Target.AllUser };
            TargetValues.Add(defaultTargetValue);
            TargetValues.Add(new TargetValue { TargetLabel = "owner and moderator and sponsor", Target = Target.OwnerModeratorSponsor });
            TargetValues.Add(new TargetValue { TargetLabel = "owner and moderator", Target = Target.OwnerModerator });
            TargetValue = defaultTargetValue;
            MessageLimit = 25;
            FontMaxSize = 128;
            FontMinSize = 16;
            Width = 1024;
            Height = 512;
            BackgroundColor = "#FFFFFF";
            FontColors = new ObservableCollection<FontColor>();
            FontColors.Add(new FontColor() { Color = "#D00000" });
            FontColors.Add(new FontColor() { Color = "#00D000" });
            FontColors.Add(new FontColor() { Color = "#0000D0" });
            FontColors.Add(new FontColor() { Color = "#D0D000" });
            FontColors.Add(new FontColor() { Color = "#00D0D0" });
            FontColors.Add(new FontColor() { Color = "#D000D0" });
            URI = "http://127.0.0.1:12345";
            IsInsecure = true;
        }

        public Collection<Color> GetFontColors()
        {
            Collection<Color> colors = new Collection<Color>();
            foreach (FontColor fontColor in FontColors)
            {
                System.Drawing.Color dColor = System.Drawing.ColorTranslator.FromHtml(fontColor.Color);
                colors.Add(new Color()
                {
                    R = dColor.R,
                    G = dColor.G,
                    B = dColor.B,
                    A = dColor.A,
                });
            }
            return colors;

        }

        public Color GetBackgroundColor()
        {
            System.Drawing.Color dColor = System.Drawing.ColorTranslator.FromHtml(BackgroundColor);           
            return new Color()
            {
                R = dColor.R,
                G = dColor.G,
                B = dColor.B,
                A = dColor.A,
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
            sb.Append("BackgroundColor: "  + BackgroundColor + "\n");
            foreach (var fontColor in FontColors)
            {
                sb.Append("Color" + FontColors.IndexOf(fontColor) + ": " + fontColor.Color + "\n");
            }
            return sb.ToString();
        }

    }
}
