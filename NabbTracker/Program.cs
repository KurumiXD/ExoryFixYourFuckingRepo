﻿using System;

using LeagueSharp;
using LeagueSharp.Common;

namespace NabbTracker
{
    class Program
    {
        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Tracker.OnLoad();
            Game.PrintChat("Nabb<font color=\"#228B22\">Tracker</font>: Ultima - Loaded!");
        }
    }
}