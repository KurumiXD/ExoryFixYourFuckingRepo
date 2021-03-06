namespace ExorAIO.Champions.Nasus
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Orbwalking = SFXTargetSelector.Orbwalking;
    using TargetSelector = SFXTargetSelector.TargetSelector;

    using ExorAIO.Utilities;

    /// <summary>
    ///    The settings class.
    /// </summary>
    public class Settings
    {
        public static void SetSpells()
        {
            Variables.Q = new Spell(SpellSlot.Q, ObjectManager.Player.BoundingRadius*2 + 150f);
            Variables.W = new Spell(SpellSlot.W, 600f);
            Variables.E = new Spell(SpellSlot.E, 500f);
            Variables.R = new Spell(SpellSlot.R, ObjectManager.Player.BoundingRadius + 175f);
            
            Variables.E.SetSkillshot(
                Variables.E.Instance.SData.SpellCastTime,
                Variables.E.Instance.SData.LineWidth,
                Variables.E.Instance.SData.MissileSpeed,
                false,
                SkillshotType.SkillshotCircle
            );
        }

        public static void SetMenu()
        {
            //Settings Menu
            Variables.SettingsMenu = new Menu("Spell Menu", $"{Variables.MainMenuName}.settingsmenu");
            {
                // Q Options
                Variables.QMenu = new Menu("Q Settings", $"{Variables.MainMenuName}.qsettingsmenu");
                {
                    Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqcombo", "Use Q in Combo")).SetValue(true);
                    Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqks", "Use Q to Automatically KillSteal")).SetValue(true);
                    Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqfarm", "Use Q to Farm")).SetValue(true);
                }
                Variables.SettingsMenu.AddSubMenu(Variables.QMenu);

                // W Options
                Variables.WMenu = new Menu("W Settings", $"{Variables.MainMenuName}.wsettingsmenu");
                {
                    Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewcombo", "Use Smart W in Combo")).SetValue(true);
                }
                Variables.SettingsMenu.AddSubMenu(Variables.WMenu);

                // E Options
                Variables.EMenu = new Menu("E Settings", $"{Variables.MainMenuName}.esettingsmenu");
                {
                    Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useecombo", "Use E in Combo")).SetValue(true);
                }
                Variables.SettingsMenu.AddSubMenu(Variables.EMenu);

                // R Options
                Variables.RMenu = new Menu("R Settings", $"{Variables.MainMenuName}.rsettingsmenu");
                {
                    Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.userlifesaver", "Use R LifeSaver")).SetValue(true);
                }
                Variables.SettingsMenu.AddSubMenu(Variables.RMenu);

                // Miscellaneous Options
                Variables.MiscMenu = new Menu("Misc. Settings", $"{Variables.MainMenuName}.miscsettingsmenu");
                {
                    Variables.MiscMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.miscsettings.useresetters", "Use Smart Tiamat/Ravenous/Titanic")).SetValue(true);
                }
                Variables.SettingsMenu.AddSubMenu(Variables.MiscMenu);
            }
            Variables.Menu.AddSubMenu(Variables.SettingsMenu);

            //Drawings Menu
            Variables.DrawingsMenu = new Menu("Drawings Menu", $"{Variables.MainMenuName}.drawingsmenu");
            {
                Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.q", "Show Q Range")).SetValue(true);
                Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.w", "Show W Range")).SetValue(true);
                Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.e", "Show E Range")).SetValue(true);
                Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.r", "Show R Range")).SetValue(true);
            }
            Variables.Menu.AddSubMenu(Variables.DrawingsMenu);
        }

        public static void SetMethods()
        {
            Game.OnUpdate += Nasus.Game_OnGameUpdate;
            Obj_AI_Base.OnDoCast += Nasus.Obj_AI_Base_OnDoCast;
        }
    }

    /// <summary>
    ///    The targets class.
    /// </summary>
    public class Targets
    {
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(Variables.W.Range, LeagueSharp.DamageType.Physical);
        public static IEnumerable<Obj_AI_Base> Units
        => 
            ObjectManager.Get<Obj_AI_Base>()
                .Where(
                    a =>
                        a.IsValidTarget(Variables.Q.Range));

        public static Obj_AI_Base Unit
        =>
            Targets.Units.FirstOrDefault();
    }
}
