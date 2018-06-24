using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.Utility.Logging;
using APICore = Hearthstone_Deck_Tracker.API.Core;

namespace HDT.Plugins.ArenaStats
{
    public partial class HeroControlAggro : StackPanel
    {
        public HeroControlAggro()
        {
            // InitializeComponent();
            this.Orientation = Orientation.Horizontal;
        }

        internal string GetHeroControlAggroScale()
        {
            return "Priest > Druid > Warlock > Mage > Shaman > Paladin > Warrior > Rogue > Hunter";
        }

        public void Update()
        {
            if (Children.Count > 0)
            {
                this.UpdatePosition();
                this.Show();
                return;
            }

            var label = new HearthstoneTextBlock();
            label.FontSize = 18;
            label.Text = GetHeroControlAggroScale();
            label.Visibility = Visibility.Visible;
            Children.Add(label);
            this.UpdatePosition();
            this.Show();
        }

        public void UpdatePosition()
        {
            Canvas.SetBottom(this, 50);
            Canvas.SetLeft(this, (APICore.OverlayWindow.Width - this.ActualWidth) / 2);
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