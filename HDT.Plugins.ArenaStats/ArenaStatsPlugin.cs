using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;

using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;

namespace HDT.Plugins.ArenaStats
{
    public class ArenaStatsPlugin : IPlugin
    {
        // overlay of hero tier stats
        private HeroSelector _heroSelector = null;
        // overlay of hero contro/aggro status
        private HeroControlAggro _heroControlAggro = null;

        // Triggered upon startup and when the user ticks the plugin on
        public void OnLoad()
        {
            // Search Hearthstone_Deck_Tracker.API.GameEvents.* for all sorts of draw, play, game start/end events
            // Search Hearthstone_Deck_Tracker.API.DeckManagerEvents.* for events related to deck creation, deletion, etc.

            _heroSelector = new HeroSelector();
            Core.OverlayCanvas.Children.Add(_heroSelector);
            _heroSelector.UpdatePosition();
            LogEvents.OnArenaLogLine.Add(OnArenaLogLine);
            //LogEvents.OnAssetLogLine.Add(OnAssetLogLine);
            //LogEvents.OnPowerLogLine.Add(OnPowerLogLine);
            //LogEvents.OnRachelleLogLine.Add(OnRachelleLogLine);

            _heroControlAggro = new HeroControlAggro();
            Core.OverlayCanvas.Children.Add(_heroControlAggro);
            _heroControlAggro.UpdatePosition();
            //LogEvents.OnArenaLogLine.Add(OnArenaLogLine);
        }

        private void OnArenaLogLine(string obj)
        {
           Log.Info(obj);
            if (obj.Contains("DraftManager.OnChoicesAndContents - Draft Deck ID: 1002679268, Hero Card ="))
            {
                var heros = HeroTier.getHeroTiers();
                foreach (HeroInfo hero in heros)
                {
                    Log.Info($"{hero.name}: {hero.win_rate}");
                }
                _heroSelector.Update(heros);
                _heroSelector.Show();
                Log.Info($"Version: {Version.ToString()}");
            }
            // D 00:00:15.5451178 Client chooses: Garrosh Hellscream (HERO_01)
            // D 00:00:18.0581602 Client chooses: Thrall (HERO_02)
            // D 00:00:27.3227601 Client chooses: Rexxar (HERO_05)
            else if (obj.Contains("Client chooses:"))
            {
                Regex regex = new Regex(@"Client chooses: (?<heroName>[A-Za-z ]+) \((?<heroId>[A-Z0-9_]+)\)$");
                Match match = regex.Matches(obj)[0]; // TODO: Error checking
                var heroId = match.Groups["heroId"];
                // TODO: show new overlay control with more details hero info (popularity, control vs aggro, most common cards picked, etc)
                _heroControlAggro.Update();
                _heroControlAggro.Show();
                Log.Info($"Hero under consideration: {heroId}");
                Log.Info($"Version: {Version.ToString()}");
            }
        }

        private void OnAssetLogLine(string obj)
        {
            Log.Info(obj);
        }

        private void OnPowerLogLine(string obj)
        {
            Log.Info(obj);
        }

        private void OnRachelleLogLine(string obj)
        {
            Log.Info(obj);
        }

        // Triggered when the user unticks the plugin, however, HDT does not completely unload the plugin.
        // see https://git.io/vxEcH
        public void OnUnload()
        {
            //Core.OverlayCanvas.Children.Remove();
        }

        public void OnButtonPress()
        {
            // Triggered when the user clicks your button in the plugin list
        }

        // called every ~100ms
        public void OnUpdate()
        {
            if (Core.Game.IsRunning && Core.Game.CurrentMode == Mode.DRAFT)
            {
                // TODO: Need to hide after hero is chosen
                _heroSelector.Show();
                _heroControlAggro.Show();
            }
            else
            {
                _heroSelector.Hide();
                _heroControlAggro.Hide();
            }
        }

        public string Name => "Arena Stats";

        public string Description => "Provides useful information during Arena runs";

        // Text displayed on the button in "options > tracker > plugins" when your plugin is selected.
        public string ButtonText => "Settings";

        public string Author => "brootski";

        public Version Version => new Version(0, 0, 31);

        // The MenuItem added to the "Plugins" main menu. Return null to not add one.
        public MenuItem MenuItem => null;
    }
}