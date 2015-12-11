namespace ExorAIO.Champions.Tristana
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Orbwalking = SFXTargetSelector.Orbwalking;

    using ExorAIO.Utilities;

    public class Logics
    {
        public static void ExecuteBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            /// <summary>
            /// The Q Combo Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                (Bools.IsCharged((Obj_AI_Base)args.Target) || !Variables.E.IsReady()) &&
                ((Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqauto").GetValue<bool>()) ||

                (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && 
                    Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqfarm").GetValue<bool>())))
            {
                Variables.Q.Cast();
            }

            /// <summary>
            /// The E Logic.
            /// </summary>
            if (Variables.E.IsReady() &&
                ((Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeauto").GetValue<bool>() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.esettings.ewhitelist.{((Obj_AI_Hero)args.Target).ChampionName.ToLower()}").GetValue<bool>()) ||

                (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear &&
                    ObjectManager.Player.ManaPercent > ManaManager.NeededEMana &&
                    ((Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useebuildings").GetValue<bool>() && args.Target.IsValid<Obj_AI_Turret>()) ||
                    (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useefarm").GetValue<bool>() && GameObjects.EnemyMinions.Count(minions => minions.Distance(Targets.EMinions.FirstOrDefault()) < 150f) > 2)))))
            {
                Variables.E.CastOnUnit((Obj_AI_Base)args.Target);
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The R KillSteal Logic.
            /// </summary>
            if (Variables.R.IsReady() &&
                (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userks").GetValue<bool>() &&
                KillSteal.Damage((Obj_AI_Hero)args.Target) > ((Obj_AI_Hero)args.Target).Health &&
                ObjectManager.Player.GetAutoAttackDamage((Obj_AI_Hero)args.Target)*2 < ((Obj_AI_Hero)args.Target).Health))
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.R.Cast((Obj_AI_Hero)args.Target);
                return;
            }
        }
    }
}
