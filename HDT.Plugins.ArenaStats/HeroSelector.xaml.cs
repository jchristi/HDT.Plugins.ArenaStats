using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.Utility.Logging;
using APICore = Hearthstone_Deck_Tracker.API.Core;
using Convert = System.Convert;

namespace HDT.Plugins.ArenaStats
{
    internal partial class HeroSelector : StackPanel
    {
        public HeroSelector()
        {
            //InitializeComponent();
            Orientation = Orientation.Vertical;
        }

        internal Color GetPercentageColor(double pct, double max = 100, double min = 0)
        {
            pct = pct / 100;
            max = max / 100;
            min = min / 100;

            double red = 0;
            double green = 0;
            double blue = 0;
            
            if (pct > max)
                return Color.FromRgb(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            if (pct < min)
                return Color.FromRgb(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));

            double median = min + ((max - min) / 2);
            double modifier = (pct - min) / (max - min);

            if (pct >= median)
            {
                red = (1 - modifier) * 2 * 255;
                green = 255;
            }
            else
            {
                red = 255;
                green = modifier * 2 * 255;
            }

            return Color.FromRgb(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
        }

        public void Update(List<HeroInfo> heros)
        {
            Children.Clear();

            var Label = new HearthstoneTextBlock();
            Label.FontSize = 16;
            Label.Text = "Winrate %";
            Label.Visibility = Visibility.Visible;
            Children.Add(Label);

            for (int i=0; i<heros.Count; i++)
            {
                // TODO: Create a custom View for this that takes a list of HeroInfo
                var hero = heros[i];
                Label = new HearthstoneTextBlock();
                Label.FontSize = 18;
                var text = $"#{i+1} {hero.name} {hero.win_rate}%"; // TODO: May want to just use a table/cols instead
                Label.Text = text;
                Label.Fill = new SolidColorBrush(GetPercentageColor(hero.win_rate, 55, 45));
                //var margin = Label.Margin;
                //margin.Top = 0;
                //Label.Margin = margin;
                Children.Add(Label);
                Label.Visibility = Visibility.Visible;
            }
            this.UpdatePosition();
        }

        public void UpdatePosition()
        {
            Log.Info($"OverlayWindow.Width={APICore.OverlayWindow.Width}");
            Log.Info($"OverlayWindow.Height={APICore.OverlayWindow.Height}");
            Canvas.SetTop(this, 50); // APICore.OverlayWindow.Height * 5 / 100);
            Canvas.SetLeft(this, 10); // APICore.OverlayWindow.Width * 20 / 100);
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }

        /* https://hearthstonejson.com/
            https://api.hearthstonejson.com/v1/
            https://art.hearthstonejson.com/v1/256x/" + e + ".jpg" 
        */
    }
}