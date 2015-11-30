﻿using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorKogMaw
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "KogMaw")
            {
                return;
            }

            KogMaw.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Kog'Maw</b></font> Loaded!");
        }
    }
}