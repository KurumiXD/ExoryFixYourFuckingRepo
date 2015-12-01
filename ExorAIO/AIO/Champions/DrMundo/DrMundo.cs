namespace ExorAIO.Champions.DrMundo
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ExorAIO.Utilities;

    /// <summary>
    /// The main class.
    /// </summary>
    public class DrMundo
    {
        public void OnLoad()
        {
            Settings.SetSpells();
            Settings.SetMenu();
            Settings.SetMethods();
            Drawings.Load();
        }

        /// <summary>
        /// Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void Game_OnGameUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead &&
                Targets.Target != null &&
                Targets.Target.IsValid)
            {
                Logics.ExecuteAuto(args);
            }

            if (Targets.Minions != null &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                Logics.ExecuteFarm(args);
            }
        }

        /// <summary>
        /// Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe &&
                Orbwalking.IsAutoAttack(args.SData.Name) &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (args.Target.IsValid<Obj_AI_Hero>())
                {
                    Logics.ExecuteModes(sender, args);
                }
            }
        }
    }
}
