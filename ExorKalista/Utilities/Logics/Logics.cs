namespace ExorKalista
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Orbwalking = SFXTargetSelector.Orbwalking;
    using TargetSelector = SFXTargetSelector.TargetSelector;

    public class Logics
    {
        public static void ExecuteAuto(EventArgs args)
        {
            /// <summary>
            /// The Soulbound declaration.
            /// </summary>
            if (Variables.SoulBound == null)
            {
                Variables.SoulBound = HeroManager.Allies
                    .Find(h => 
                        h.Buffs
                    .Any(b =>
                        b.Caster.IsMe &&
                        b.Name.Contains("kalistacoopstrikeally")));
            }

            /// <summary>
            /// The Target preference.
            /// </summary>
            if (TargetSelector.Weights.GetItem("low-health") != null)
            {
                TargetSelector.Weights.GetItem("low-health").ValueFunction = hero => hero.Health - Variables.GetPerfectRendDamage(hero);
                TargetSelector.Weights.GetItem("low-health").Tooltip = "Low Health (Health - Rend Damage) = Higher Weight";
                TargetSelector.Weights.Register(
                    new TargetSelector.Weights.Item(
                        "w-stack", "W Stack", 10, false, hero => hero.HasBuff("kalistacoopstrikemarkally") ? 1 : 0,
                        "Has W Debuff = Higher Weight"));
            }

            /// <summary>
            /// The Q KillSteal Logic,
            /// The Q Immobile Harass Logic.
            /// The Q Combo Part.1 Logic
            /// </summary>
            if (Variables.Q.IsReady() &&
                Targets.Target != null &&
                Targets.Target.IsValidTarget(Variables.Q.Range) &&
                Variables.Q.GetPrediction(Targets.Target).Hitchance >= HitChance.VeryHigh &&
                ((Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqks").GetValue<bool>() && Targets.Target.Health <= ObjectManager.Player.GetSpellDamage(Targets.Target, SpellSlot.Q)) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqimmobile").GetValue<bool>() && Bools.IsImmobile(Targets.Target)) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)))
            {
                //Orbwalking.ResetAutoAttackTimer();
                Variables.Q.Cast(Variables.Q.GetPrediction(Targets.Target).UnitPosition);
            }

            /// <summary>
            /// The Q Farm Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                !ObjectManager.Player.IsWindingUp &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqfarm").GetValue<bool>() &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededQMana)
            {
                if (Targets.Minions
                    .Count(
                        m => 
                            m != null &&
                            m.IsValidTarget(Variables.Q.Range) &&
                            (m.Health < Variables.Q.GetDamage(m))) > 2 &&
                    Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).MinionsHit > 2)
                {
                    //Orbwalking.ResetAutoAttackTimer();
                    Variables.Q.Cast(Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).Position);
                }
            }

            /// <summary>
            /// The W Logic.
            /// </summary>
            if (Variables.W.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewauto").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededWMana &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None &&
                !ObjectManager.Player.IsRecalling() &&
                ((ObjectManager.Player.Distance(new Vector2(5064f, 10568f)) < Variables.W.Range) ||
                (ObjectManager.Player.Distance(new Vector2(9796f, 4432f)) < Variables.W.Range)))
            {
                Variables.W.Cast(
                    ObjectManager.Player.Distance(new Vector2(5064f, 10568f)) < ObjectManager.Player.Distance(new Vector2(9796f, 4432f)) ?
                        new Vector3(5064f, 10568f, -71f) :
                        new Vector3(9796f, 4432f, -71f)
                );
            }

            /// <summary>
            /// The E before Dying Logic.
            /// </summary>
            if (Variables.E.IsReady() &&
                Targets.Target != null &&
                !Targets.Target.IsDead &&
                Targets.Target.IsVisible &&
                Variables.E.CanCast(Targets.Target) &&
                Targets.Target.IsValidTarget(Variables.E.Range) &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useedie").GetValue<bool>() &&
                HealthPrediction.GetHealthPrediction(ObjectManager.Player, (int)(250 + Game.Ping / 2f)) <= 0)
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.E.Cast();
                return;
            }

            /// <summary>
            /// The E Combo Logic.
            /// </summary>
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useecombo").GetValue<bool>())
            {
                foreach (var unit in ObjectManager.Get<Obj_AI_Hero>()
                    .Where(
                        h =>
                            h != null &&
                            !h.IsDead &&
                            h.IsVisible &&
                            Variables.E.CanCast(h) &&
                            h.IsValidTarget(Variables.E.Range) &&
                            h.Health < Variables.GetPerfectRendDamage(h)))
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Variables.E.Cast();
                    return;
                }
            }

            /// <summary>
            /// The E against Monsters Logic.
            /// </summary>
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useemonsters").GetValue<bool>())
            {
                foreach (var miniontarget in MinionManager.GetMinions(Variables.E.Range, MinionTypes.All, MinionTeam.Neutral)
                    .Where(
                        y => 
                            y != null &&
                            !y.IsDead &&
                            y.IsVisible &&
                            Variables.E.CanCast(y) &&
                            y.IsValidTarget(Variables.E.Range) &&
                            (y.Health < Variables.GetPerfectRendDamage(y))))
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Variables.E.Cast();
                    return;
                }
            }

            /// <summary>
            /// The R Lifesaver Logic.
            /// </summary>
            if (Variables.R.IsReady() &&
                ObjectManager.Player.Distance(Variables.SoulBound) <= Variables.R.Range &&
                HealthPrediction.GetHealthPrediction(Variables.SoulBound, (int)(250 + Game.Ping / 2f)) <= 0 &&
                Variables.SoulBound.CountEnemiesInRange(800) > 0)
            {
                Variables.R.Cast();
            }
        }

        public static void ExecuteFarm(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The E Farm Logic.
            /// </summary>
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useefarm").GetValue<bool>())
            {
                if (Targets.Minions.Any() &&
                    Targets.Minions.Count(
                        x =>
                            !x.IsDead &&
                            x.IsVisible &&
                            Variables.E.CanCast(x) &&
                            x.IsValidTarget(Variables.E.Range) &&
                            x.Health < (float)(ObjectManager.Player.CalcDamage(x, LeagueSharp.Common.Damage.DamageType.Physical, Variables.E.GetDamage(x)))) >= 2)
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Variables.E.Cast();
                    return;
                }
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The Q Combo Part2 Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                ((Obj_AI_Hero)args.Target).IsValidTarget(Variables.Q.Range) &&
                Variables.Q.GetPrediction(((Obj_AI_Hero)args.Target)).Hitchance >= HitChance.VeryHigh &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>() &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                //Orbwalking.ResetAutoAttackTimer();
                Variables.Q.Cast(Variables.Q.GetPrediction((Obj_AI_Hero)args.Target).UnitPosition);
            }
        }
    }
}
