//Leakthisyouprick@neegro.net
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FoxCore.Logs;
using FoxCore.WoWChecks;
using FoxCore.ControlMethod;

namespace FoxCore.Rotation
{
    public class Druid : CombatRoutine
    {
        public override string RoutineName { get { return "Boomkin"; } }
        public override string Premium { get { return "false"; } }
        public override string Recurring { get { return "false"; } }

        // SET AoE Range (range around you to detect aoe)
        public override int AoE_Range
        {
            get
            {
                return 40;
            }
        }

        // SET Interrupt Ability ID, self explanitory
        public override int Interrupt_Ability_Id
        {
            get
            {
                return 0;
            }
        }

        // Set single target rotation at number of targets around you
        public override int SINGLE
        {
            get
            {
                return 1;
            }
        }

        // Set aoe rotation at number of targets around you
        public override int AOE
        {
            get
            {
                return 3;
            }
        }

        // Set cleave rotation at number of targets around you
        public override int CLEAVE
        {
            get
            {
                return 99;
            }
        }

        public override string Name
        {
            get { return "BalanceDruid"; }
        }
        public override string Class
        {
            get { return "Druid"; }
        }

        public override Form SettingsForm { get; set; }

        // Intitialize
        public override void Initialize()
        {
            Log.Write("ST: 1 Target", Color.Red);
            Log.Write("Cleave:  2 Targets", Color.Red);
            Log.Write("AoE: >= 3 Targets", Color.Red);
        }

        public override void Stop()
        {
        }

        private float GCD
        {
            get
            {

                return (150f / (1f + (WoW.HastePercent / 100f)));

            }
        }

        // Pulse Rotation
        public override void Pulse()
        {
            if (WoW.CanCast("Moonkin Form") && !WoW.PlayerHasBuff("Moonkin Form") && WoW.HasTarget && WoW.TargetIsEnemy && !WoW.IsMounted && !WoW.PlayerHasBuff("Travel Form") && !WoW.PlayerHasBuff("Bear Form") && !WoW.PlayerHasBuff("Cat Form"))
            {
                WoW.CastSpell("Moonkin Form");
                return;
            }

            if (WoW.CanCast("Celestial Alignment") && WoW.HasBossTarget && WoW.IsInCombat || WoW.CanCast("Celestial Alignment") && WoW.IsInCombat && UseCooldowns)
            {
                    WoW.CastSpell("Celestial Alignment");
                    return;
            }

            if (combatRoutine.Type == RotationType.SingleTarget)  // Do Single Target Stuff here
            {
                Log.Write("Doing Singletarget", Color.Red);

                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat)
                {
                    if (WoW.CanCast("Solar Beam") && WoW.TargetIsCastingAndSpellIsInterruptible && WoW.TargetPercentCast > 35)
                    {
                        WoW.CastSpell("Solar Beam");
                        return;
                    }

                    if (WoW.CanCast("Starsurge") && WoW.CurrentAstralPower > 70)
                    {
                        WoW.CastSpell("Starsurge");
                        return;
                    }

                    if (WoW.CanCast("Moonfire") && !WoW.TargetHasDebuff("Moonfire") || WoW.TargetDebuffTimeRemaining("Moonfire") < 300)
                    {
                        WoW.CastSpell("Moonfire");
                        return;
                    }

                    if (WoW.CanCast("Sunfire") && !WoW.TargetHasDebuff("Sunfire") && WoW.LastSpell != "Sunfire" || WoW.TargetDebuffTimeRemaining("Sunfire") < 300)
                    {
                        WoW.CastSpell("Sunfire");
                        return;
                    }

                    if (WoW.CanCast("Lunar Strike") && WoW.LastSpell != "Lunar Strike" && WoW.PlayerBuffTimeRemaining("Lunar Empowerment") > GCD && !WoW.IsMoving || WoW.CanCast("Lunar Strike") && WoW.PlayerBuffTimeRemaining("Lunar Empowerment") > GCD && !WoW.IsMoving && WoW.PlayerBuffStacks("Lunar Empowerment") >= 2)
                    {
                        WoW.CastSpell("Lunar Strike");
                        return;
                    }

                    if (WoW.CanCast("Solar Wrath") && WoW.PlayerBuffTimeRemaining("Solar Empowerment") > GCD && !WoW.IsMoving || WoW.CanCast("Solar Wrath") && WoW.CurrentAstralPower < 85 && !WoW.IsMoving || WoW.CanCast("Solar Wrath") && WoW.PlayerBuffTimeRemaining("Solar Empowerment") > GCD && !WoW.IsMoving && WoW.PlayerBuffStacks("Solar Empowerment") >= 2)
                    {
                        WoW.CastSpell("Solar Wrath");
                        return;
                    }

                    if (WoW.CanCast("Rejuv") && WoW.HealthPercent < 85 && !WoW.PlayerHasBuff("Rejuv") && !WoW.IsMounted && !WoW.PlayerHasBuff("Travel Form"))
                    {
                        WoW.CastSpell("Rejuv");
                        return;
                    }

                    if (WoW.CanCast("Regrowth") && WoW.HealthPercent < 50 && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Regrowth");
                        return;
                    }

                }
            }

            if (combatRoutine.Type == RotationType.SingleTargetCleave)
            {

            }

            if (combatRoutine.Type == RotationType.AOE)
            {
                Log.Write("Doing AOE", Color.Red);

                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat)
                {
                    if (WoW.CanCast("Starfall") && !WoW.PlayerHasBuff("Starfall") && WoW.CurrentAstralPower > 85)
                    {
                        //WoW.CastSpell("Starfall");
                        WoW.CastSpellOnMe("Starfall");
                        return;
                    }

                    if (WoW.CanCast("Starsurge") && WoW.CurrentAstralPower > 40)
                    {
                        WoW.CastSpell("Starsurge");
                        return;
                    }

                    if (WoW.CanCast("Moonfire") && !WoW.TargetHasDebuff("Moonfire") || WoW.TargetDebuffTimeRemaining("Moonfire") < 300)
                    {
                        WoW.CastSpell("Moonfire");
                        return;
                    }

                    if (WoW.CanCast("Sunfire") && !WoW.TargetHasDebuff("Sunfire") || WoW.TargetDebuffTimeRemaining("Sunfire") < 300)
                    {
                        WoW.CastSpell("Sunfire");
                        return;
                    }

                    if (WoW.CanCast("Lunar Strike") && WoW.LastSpell != "Lunar Strike" && WoW.PlayerBuffTimeRemaining("Lunar Empowerment") > GCD && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Lunar Strike");
                        return;
                    }

                    if (WoW.CanCast("Solar Wrath") && WoW.PlayerBuffTimeRemaining("Solar Empowerment") > GCD && !WoW.IsMoving || WoW.CanCast("Solar Wrath") && WoW.CurrentAstralPower < 85 && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Solar Wrath");
                        return;
                    }

                    if (WoW.CanCast("Rejuv") && WoW.HealthPercent < 85 && !WoW.PlayerHasBuff("Rejuv") && WoW.IsInCombat)
                    {
                        WoW.CastSpell("Rejuv");
                        return;
                    }

                    if (WoW.CanCast("Regrowth") && WoW.HealthPercent < 50 && !WoW.IsMoving && WoW.IsInCombat)
                    {
                        WoW.CastSpell("Regrowth");
                        return;
                    }
                }
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Whatever
AddonName=JerkOff
WoWVersion=BFA - 80000
[SpellBook.db]
Spell,8921,Moonfire,D1
Spell,93402,Sunfire,D2
Spell,78674,Starsurge,D3
Spell,194153,Lunar Strike,D4
Spell,190984,Solar Wrath,D5
Spell,24858,Moonkin Form,NumPad1
Spell,774,Rejuv,F3
Spell,8936,Regrowth,F1
Spell,78675,Solar Beam,D7
Spell,194223,Celestial Alignment,D9
Buff,164547,Lunar Empowerment
Buff,164545,Solar Empowerment
Buff,194223,Celestial Alignment
Buff,24858,Moonkin Form
Buff,783,Travel Form
Buff,768,Cat Form
Buff,5487,Bear Form
Buff,191034,Starfall
Buff,774,Rejuv
Buff,8936,Regrowth
Debuff,164812,Moonfire
Debuff,164815,Sunfire
*/
