using System;
using System.Collections.Generic;

using LeagueSharp;
using LeagueSharp.Common;

namespace ExorAIO.Utilities
{
    /// <summary>
    ///    The Mana manager class.
    /// </summary>
    class ManaManager
    {
        public static int NeededQMana => Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.qmana") != null ? Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.qmana").GetValue<Slider>().Value : 0;
        public static int NeededWMana => Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.wmana") != null ? Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.wmana").GetValue<Slider>().Value : 0;
        public static int NeededEMana => Variables.Menu.Item($"{Variables.MainMenuName}.esettings.emana") != null ? Variables.Menu.Item($"{Variables.MainMenuName}.esettings.emana").GetValue<Slider>().Value : 0;
        public static int NeededRMana => Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.rmana") != null ? Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.rmana").GetValue<Slider>().Value : 0;
    }

    /// <summary>
    ///    The drawings class.
    /// </summary>
    public class Drawings
    {
        public static void Load()
        {
            Drawing.OnDraw += delegate
            {
                if (Variables.Q.IsReady() &&
                    Variables.Q.Range != 0 &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.q") != null &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.q").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.Q.Range, System.Drawing.Color.Green);
                }

                if (Variables.W.IsReady() &&
                    Variables.W.Range != 0 &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.w") != null &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.w").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.W.Range, System.Drawing.Color.Purple);
                }

                if (Variables.E.IsReady() &&
                    Variables.E.Range != 0 &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.e") != null &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.e").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.E.Range, System.Drawing.Color.Cyan);
                }

                if (Variables.R.IsReady() &&
                    Variables.R.Range != 0 &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.r") != null &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.r").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.R.Range, System.Drawing.Color.Red);
                }
            };
        }
    }
}
