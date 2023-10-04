using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp;
using WCSharp.Buffs;
using WCSharp.DateTime;
using WCSharp.Dummies;
using WCSharp.Effects;
using WCSharp.Events;
using WCSharp.Json;
using WCSharp.Knockbacks;
using WCSharp.Lightnings;
using WCSharp.Missiles;
using WCSharp.SaveLoad;
using WCSharp.Shared;
using WCSharp.Sync;
using static Constants;
using static Regions;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.DamageSystem
{
    public static class DamageEngine
    {
        public delegate void UnitAutoAttackedEventHandler(object sender, UnitAttackEventArgs e);
        public static event UnitAutoAttackedEventHandler UnitAutoAttacked;
        public delegate void UnitDamagedEventHandler(object sender, UnitDamagedEventArgs e);
        public static event UnitDamagedEventHandler UnitDamaged;

        public static void Init()
        {
            Console.WriteLine("Initializing damage engine!");
            CreateDamagedTrigger();
            CreateDamagingTrigger();
        }

        private static void CreateDamagedTrigger()
        {
            trigger damagedTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(damagedTrigger, EVENT_PLAYER_UNIT_DAMAGED);
            TriggerAddAction(damagedTrigger, DetectDamage);
        }

        private static void CreateDamagingTrigger()
        {
            trigger damagingTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(damagingTrigger, EVENT_PLAYER_UNIT_DAMAGING);
            TriggerAddAction(damagingTrigger, DetectDamaging);
        }

        private static void DetectDamage()
        {
            unit damagingUnit = GetEventDamageSource();
            UnitInstance damagingUnitInstance = UnitManager.GetUnitInstance(damagingUnit);
            unit damagedUnit = GetTriggerUnit();
            UnitInstance damagedUnitInstance = UnitManager.GetUnitInstance(damagedUnit);
            //float eventDamage = GetEventDamage();
            BlzSetEventDamage(0.0f);
            //Console.WriteLine("Unit " + GetUnitName(damagingUnit) + " damaged unit " + GetUnitName(damagedUnit) + " for " + eventDamage + " with an attack type of " + ConvertAttackTypeToString(BlzGetEventAttackType()) + " and a damage type of " +  ConvertDamageTypeToString(BlzGetEventDamageType()) + "!");
            if (BlzGetEventIsAttack())
            {
                //Console.WriteLine("Detected damage is an attack!");
                OnUnitAutoAttacked(null, new UnitAttackEventArgs() { AttackingUnit = damagingUnitInstance, TargetUnit = damagedUnitInstance });
                DamageUnit(new List<DamageInstance>() 
                { 
                    new DamageInstance(damagingUnitInstance, damagedUnitInstance, DamageType.PHYSICAL, damagingUnitInstance.UnitData.TotalPhysicalAttackDamage), 
                    new DamageInstance(damagingUnitInstance, damagedUnitInstance, DamageType.MAGICAL, damagingUnitInstance.UnitData.TotalMagicalAttackDamage)
                });
            }
        }

        private static void DetectDamaging()
        {
            //Console.WriteLine("Unit " + GetUnitName(GetTriggerUnit()) + " is damaging!");
        }

        //// Used to interact with default game systems.
        //private static string ConvertAttackTypeToString(attacktype atkType)
        //{
        //    if (atkType == ATTACK_TYPE_CHAOS)
        //    {
        //        return "Chaos";
        //    }
        //    if (atkType == ATTACK_TYPE_HERO)
        //    {
        //        return "Hero";
        //    }
        //    if (atkType == ATTACK_TYPE_MAGIC)
        //    {
        //        return "Magic";
        //    }
        //    if (atkType == ATTACK_TYPE_MELEE)
        //    {
        //        return "Melee";
        //    }
        //    if (atkType == ATTACK_TYPE_NORMAL)
        //    {
        //        return "Normal";
        //    }
        //    if (atkType == ATTACK_TYPE_PIERCE)
        //    {
        //        return "Pierce";
        //    }
        //    if (atkType == ATTACK_TYPE_SIEGE)
        //    {
        //        return "Siege";
        //    }
        //    return "Null";
        //}

        //// Used to interact with default game systems.
        //private static string ConvertDamageTypeToString(damagetype dmgType)
        //{
        //    if (dmgType == DAMAGE_TYPE_ACID)
        //    {
        //        return "Acid";
        //    }
        //    if (dmgType == DAMAGE_TYPE_COLD)
        //    {
        //        return "Cold";
        //    }
        //    if (dmgType == DAMAGE_TYPE_DEATH)
        //    {
        //        return "Death";
        //    }
        //    if (dmgType == DAMAGE_TYPE_DEFENSIVE)
        //    {
        //        return "Defensive";
        //    }
        //    if (dmgType == DAMAGE_TYPE_DEMOLITION)
        //    {
        //        return "Demolition";
        //    }
        //    if (dmgType == DAMAGE_TYPE_DISEASE)
        //    {
        //        return "Disease";
        //    }
        //    if (dmgType == DAMAGE_TYPE_DIVINE)
        //    {
        //        return "Divine";
        //    }
        //    if (dmgType == DAMAGE_TYPE_ENHANCED)
        //    {
        //        return "Enhanced";
        //    }
        //    if (dmgType == DAMAGE_TYPE_FIRE)
        //    {
        //        return "Fire";
        //    }
        //    if (dmgType == DAMAGE_TYPE_FORCE)
        //    {
        //        return "Force";
        //    }
        //    if (dmgType == DAMAGE_TYPE_LIGHTNING)
        //    {
        //        return "Lightning";
        //    }
        //    if (dmgType == DAMAGE_TYPE_MAGIC)
        //    {
        //        return "Magic";
        //    }
        //    if (dmgType == DAMAGE_TYPE_MIND)
        //    {
        //        return "Mind";
        //    }
        //    if (dmgType == DAMAGE_TYPE_NORMAL)
        //    {
        //        return "Normal";
        //    }
        //    if (dmgType == DAMAGE_TYPE_PLANT)
        //    {
        //        return "Plant";
        //    }
        //    if (dmgType == DAMAGE_TYPE_POISON)
        //    {
        //        return "Poison";
        //    }
        //    if (dmgType == DAMAGE_TYPE_SHADOW_STRIKE)
        //    {
        //        return "Shadow Strike";
        //    }
        //    if (dmgType == DAMAGE_TYPE_SLOW_POISON)
        //    {
        //        return "Slow Poison";
        //    }
        //    if (dmgType == DAMAGE_TYPE_SONIC)
        //    {
        //        return "Sonic";
        //    }
        //    if (dmgType == DAMAGE_TYPE_SPIRIT_LINK)
        //    {
        //        return "Spirit Link";
        //    }
        //    if (dmgType == DAMAGE_TYPE_UNIVERSAL)
        //    {
        //        return "Universal";
        //    }
        //    if (dmgType == DAMAGE_TYPE_UNKNOWN)
        //    {
        //        return "Unknown";
        //    }
        //    return "NULL";
        //}

        public static void DamageUnit(List<DamageInstance> damageInstances)
        {
            int damageTotal = 0;
            for (int i = 0; i < damageInstances.Count; i++)
            {
                damageTotal += DamageUnit(damageInstances[i]);
            }
            DisplayDamageText(damageTotal, damageInstances[0].DamageInstanceTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(damageInstances[0].DamageInstanceSource.LinkedUnit)));
        }

        public static int DamageUnit(DamageInstance damageInstance)
        {
            int roundedDamage = 0;
            if (damageInstance.DamageInstanceType != DamageType.PURE)
            {
                GetCriticalModifiedDamage(damageInstance.DamageInstanceSource, damageInstance.DamageInstanceAmount);
            }
            switch (damageInstance.DamageInstanceType)
            {
                case DamageType.PHYSICAL:
                    // Apply physical damage formula.
                    // Get starting physical reduction.
                    //Console.WriteLine("Incoming damage is starting at " + damageInstance.DamageInstanceAmount + "!");
                    float totalPhysicalReduction = damageInstance.DamageInstanceTarget.UnitData.TotalPhysicalDamageReduction;
                    //Console.WriteLine("Unit is starting with " + totalPhysicalReduction + " flat physical reduction!");
                    if (totalPhysicalReduction > 0.0f)
                    {
                        // Apply percent physical pen.
                        totalPhysicalReduction *= 1.0f - damageInstance.DamageInstanceSource.UnitData.AbsolutePercentPhysicalPenetration;
                        // Apply flat physical pen.
                        totalPhysicalReduction -= damageInstance.DamageInstanceSource.UnitData.TotalPhysicalPenetration;
                        // Clamp reduction.
                        Math.Clamp(totalPhysicalReduction, 0.0f, float.MaxValue);
                    }
                    //Console.WriteLine("Unit has " + totalPhysicalReduction + " flat physical reduction left!");
                    // Reduce damage by percent physical resistance.
                    damageInstance.DamageInstanceAmount *= 1.0f - damageInstance.DamageInstanceTarget.UnitData.AbsolutePhysicalDamageReductionPercent;
                    // Reduce damage by remaining flat physical resistance.
                    damageInstance.DamageInstanceAmount -= totalPhysicalReduction;
                    Math.Clamp(damageInstance.DamageInstanceAmount, 0.0f, float.MaxValue);
                    //Console.WriteLine("Damage has been reduced to " + damageInstance.DamageInstanceAmount + "!");
                    roundedDamage = MathRound(damageInstance.DamageInstanceAmount);
                    damageInstance.DamageInstanceTarget.UnitData.CurrentHealth -= roundedDamage;
                    OnUnitDamaged(null, new UnitDamagedEventArgs() { AttackingUnit = damageInstance.DamageInstanceSource, TargetUnit = damageInstance.DamageInstanceTarget, DamageDealtType = DamageType.PHYSICAL, DamageDealtAmount = roundedDamage });
                    //DisplayDamageText(roundedDamage, damageTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(damageSource.LinkedUnit)));
                    return roundedDamage;
                    //break;
                case DamageType.MAGICAL:
                    // Apply magical damage formula.
                    // Get starting magical reduction.
                    //Console.WriteLine("Incoming damage is starting at " + damageInstance.DamageInstanceAmount + "!");
                    float totalMagicalReduction = damageInstance.DamageInstanceTarget.UnitData.TotalMagicalDamageReduction;
                    //Console.WriteLine("Unit is starting with " + totalMagicalReduction + " flat magical reduction!");
                    if (totalMagicalReduction > 0.0f)
                    {
                        // Apply percent magical pen
                        totalMagicalReduction *= 1.0f - damageInstance.DamageInstanceSource.UnitData.AbsolutePercentMagicalPenetration;
                        // Apply flat magical pen
                        totalMagicalReduction -= damageInstance.DamageInstanceSource.UnitData.TotalMagicalPenetration;
                        // Clamp reduction
                        Math.Clamp(totalMagicalReduction, 0.0f, float.MaxValue);
                    }
                    //Console.WriteLine("Unit has " + totalMagicalReduction + " flat magical reduction left!");
                    // Reduce damage by percent magical resistance.
                    damageInstance.DamageInstanceAmount *= 1.0f - damageInstance.DamageInstanceTarget.UnitData.AbsoluteMagicalDamageReductionPercent;
                    // Reduce damage by remaining flat magical resistance.
                    damageInstance.DamageInstanceAmount -= totalMagicalReduction;
                    Math.Clamp(damageInstance.DamageInstanceAmount, 0.0f, float.MaxValue);
                    //Console.WriteLine("Damage has been reduced to " + damageInstance.DamageInstanceAmount + "!");
                    roundedDamage = MathRound(damageInstance.DamageInstanceAmount);
                    damageInstance.DamageInstanceTarget.UnitData.CurrentHealth -= roundedDamage;
                    OnUnitDamaged(null, new UnitDamagedEventArgs() { AttackingUnit = damageInstance.DamageInstanceSource, TargetUnit = damageInstance.DamageInstanceTarget, DamageDealtType = DamageType.MAGICAL, DamageDealtAmount = roundedDamage });
                    //DisplayDamageText(roundedDamage, damageTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(damageSource.LinkedUnit)));
                    return roundedDamage;
                    //break;
                case DamageType.PURE:
                    // Apply pure damage; no mitigation applied.
                    roundedDamage = MathRound(damageInstance.DamageInstanceAmount);
                    damageInstance.DamageInstanceTarget.UnitData.CurrentHealth -= roundedDamage;
                    OnUnitDamaged(null, new UnitDamagedEventArgs() { AttackingUnit = damageInstance.DamageInstanceSource, TargetUnit = damageInstance.DamageInstanceTarget, DamageDealtType = DamageType.PURE, DamageDealtAmount = roundedDamage });
                    //DisplayDamageText(roundedDamage, damageTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(damageSource.LinkedUnit)));
                    return roundedDamage;
                    //break;
                default:
                    return roundedDamage;
                    //break;
            }
        }

        //public static int DamageUnit(UnitInstance damageSource, UnitInstance damageTarget, DamageType damageType, float damageAmount)
        //{
        //    int roundedDamage = 0;
        //    if (damageType != DamageType.PURE)
        //    {
        //        GetCriticalModifiedDamage(damageSource, damageAmount);
        //    }
        //    switch (damageType)
        //    {
        //        case DamageType.PHYSICAL:
        //            // Apply physical damage formula
        //            // Get starting physical reduction
        //            Console.WriteLine("Incoming damage is starting at " + damageAmount + "!");
        //            float totalPhysicalReduction = damageTarget.UnitData.TotalPhysicalDamageReduction;
        //            Console.WriteLine("Unit is starting with " + totalPhysicalReduction + " flat physical reduction!");
        //            if (totalPhysicalReduction > 0.0f)
        //            {
        //                // Apply percent physical pen
        //                totalPhysicalReduction *= 1.0f - damageSource.UnitData.AbsolutePercentPhysicalPenetration;
        //                // Apply flat physical pen
        //                totalPhysicalReduction -= damageSource.UnitData.TotalPhysicalPenetration;
        //                // Clamp reduction
        //                Math.Clamp(totalPhysicalReduction, 0.0f, float.MaxValue);
        //            }
        //            Console.WriteLine("Unit has " + totalPhysicalReduction + " flat physical reduction left!");
        //            // Reduce damage by percent physical resistance
        //            damageAmount *= 1.0f - damageTarget.UnitData.AbsolutePhysicalDamageReductionPercent;
        //            // Reduce damage by remaining flat physical resistance
        //            damageAmount -= totalPhysicalReduction;
        //            Console.WriteLine("Damage has been reduced to " + damageAmount + "!");
        //            roundedDamage = MathRound(damageAmount);
        //            damageTarget.UnitData.CurrentHealth -= roundedDamage;
        //            //DisplayDamageText(roundedDamage, damageTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(damageSource.LinkedUnit)));
        //            return roundedDamage;
        //            //break;
        //        case DamageType.MAGICAL:
        //            // Apply magical damage formula
        //            // Get starting magical reduction
        //            Console.WriteLine("Incoming damage is starting at " + damageAmount + "!");
        //            float totalMagicalReduction = damageTarget.UnitData.TotalMagicalDamageReduction;
        //            Console.WriteLine("Unit is starting with " + totalMagicalReduction + " flat magical reduction!");
        //            if (totalMagicalReduction > 0.0f)
        //            {
        //                // Apply percent magical pen
        //                totalMagicalReduction *= 1.0f - damageSource.UnitData.AbsolutePercentMagicalPenetration;
        //                // Apply flat magical pen
        //                totalMagicalReduction -= damageSource.UnitData.TotalMagicalPenetration;
        //                // Clamp reduction
        //                Math.Clamp(totalMagicalReduction, 0.0f, float.MaxValue);
        //            }
        //            Console.WriteLine("Unit has " + totalMagicalReduction + " flat magical reduction left!");
        //            // Reduce damage by percent magical resistance
        //            damageAmount *= 1.0f - damageTarget.UnitData.AbsoluteMagicalDamageReductionPercent;
        //            // Reduce damage by remaining flat magical resistance
        //            damageAmount -= totalMagicalReduction;
        //            Console.WriteLine("Damage has been reduced to " + damageAmount + "!");
        //            roundedDamage = MathRound(damageAmount);
        //            damageTarget.UnitData.CurrentHealth -= roundedDamage;
        //            //DisplayDamageText(roundedDamage, damageTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(damageSource.LinkedUnit)));
        //            return roundedDamage;
        //            //break;
        //        case DamageType.PURE:
        //            // Apply pure damage; no mitigation applied
        //            roundedDamage = MathRound(damageAmount);
        //            damageTarget.UnitData.CurrentHealth -= roundedDamage;
        //            //DisplayDamageText(roundedDamage, damageTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(damageSource.LinkedUnit)));
        //            return roundedDamage;
        //            //break;
        //        default:
        //            return roundedDamage;
        //            //break;
        //    }
        //}

        public static void HealUnit(UnitInstance healingSource, UnitInstance healingTarget, float healAmount)
        {
            healAmount = GetCriticalModifiedDamage(healingSource, healAmount);
            int roundedHeal = MathRound(healAmount);
            healingTarget.UnitData.CurrentHealth += roundedHeal;
            DisplayHealingText(roundedHeal, healingTarget.LinkedUnit, GetPlayerId(GetOwningPlayer(healingSource.LinkedUnit)));
        }

        private static float GetCriticalModifiedDamage(UnitInstance attackingUnit, float damageAmount)
        {
            float criticalRoll = GetRandomReal(0.000f, 99.999f);
            float criticalDamageMultiplier = 0.0f;
            //Console.WriteLine("Crit roll is " + criticalRoll + " and needs to be less than " + attackingUnit.UnitData.TotalCriticalChance * 100 + "!");
            if (criticalRoll < attackingUnit.UnitData.TotalCriticalChance)
            {
                criticalDamageMultiplier = 1.0f + attackingUnit.UnitData.TotalCriticalDamage;
                //Console.WriteLine("Damage instance crit, changing value from " + damageAmount + " to " + damageAmount * criticalDamageMultiplier);
                damageAmount *= criticalDamageMultiplier;
            }
            return damageAmount;
        }

        private static void DisplayDamageText(int damageAmount, unit targetUnit, int playerNumber)
        {
            float[] rgbArray = ConvertPlayerNumberToColor(playerNumber);
            texttag damageText = CreateTextTagUnitBJ("-" + damageAmount + " hp", targetUnit, -57.0f, 9.0f, rgbArray[0], rgbArray[1], rgbArray[2], 0.0f);
            SetTextTagVelocity(damageText, 0.0f, 0.02f);
            SetTextTagPermanent(damageText, false);
            SetTextTagLifespan(damageText, 2.0f);
            SetTextTagFadepoint(damageText, 1.0f);
        }

        private static void DisplayHealingText(int healAmount, unit targetUnit, int playerNumber)
        {
            float[] rgbArray = ConvertPlayerNumberToColor(playerNumber);
            texttag damageText = CreateTextTagUnitBJ("+" + healAmount + " hp", targetUnit, -57.0f, 9.0f, rgbArray[0], rgbArray[1], rgbArray[2], 0.0f);
            SetTextTagVelocity(damageText, 0.0f, 0.02f);
            SetTextTagPermanent(damageText, false);
            SetTextTagLifespan(damageText, 2.0f);
            SetTextTagFadepoint(damageText, 1.0f);
        }

        private static float[] ConvertPlayerNumberToColor(int playerNumber)
        {
            float[] rgbArray = new float[3];
            switch (playerNumber)
            {
                case 0:
                    rgbArray[0] = 255;
                    rgbArray[1] = 3;
                    rgbArray[2] = 3;
                    break;
                case 1:
                    rgbArray[0] = 0;
                    rgbArray[1] = 66;
                    rgbArray[2] = 255;
                    break;
                case 2:
                    rgbArray[0] = 28;
                    rgbArray[1] = 230;
                    rgbArray[2] = 185;
                    break;
                case 3:
                    rgbArray[0] = 84;
                    rgbArray[1] = 0;
                    rgbArray[2] = 129;
                    break;
                case 4:
                    rgbArray[0] = 255;
                    rgbArray[1] = 252;
                    rgbArray[2] = 0;
                    break;
                case 5:
                    rgbArray[0] = 254;
                    rgbArray[1] = 138;
                    rgbArray[2] = 14;
                    break;
                case 6:
                    rgbArray[0] = 32;
                    rgbArray[1] = 192;
                    rgbArray[2] = 0;
                    break;
                case 7:
                    rgbArray[0] = 229;
                    rgbArray[1] = 91;
                    rgbArray[2] = 176;
                    break;
                case 8:
                    rgbArray[0] = 149;
                    rgbArray[1] = 150;
                    rgbArray[2] = 151;
                    break;
                case 9:
                    rgbArray[0] = 126;
                    rgbArray[1] = 191;
                    rgbArray[2] = 241;
                    break;
                case 10:
                    rgbArray[0] = 16;
                    rgbArray[1] = 98;
                    rgbArray[2] = 70;
                    break;
                case 11:
                    rgbArray[0] = 78;
                    rgbArray[1] = 42;
                    rgbArray[2] = 4;
                    break;
            }
            return rgbArray;
        }

        public static void OnUnitAutoAttacked(object s, UnitAttackEventArgs e)
        {
            if (UnitAutoAttacked != null)
            {
                UnitAutoAttacked(null, e);
            }
        }

        public static void OnUnitDamaged(object s, UnitDamagedEventArgs e)
        {
            if (UnitDamaged != null)
            {
                UnitDamaged(null, e);
            }
        }

        public static void OnUnitDamagedPhysical(object s, UnitAttackEventArgs e)
        {
            
        }

        public static void OnUnitDamagedMagical(object s, UnitAttackEventArgs e)
        {
            
        }

        public static void OnUnitDamagedPure(object s, UnitAttackEventArgs e)
        {

        }
    }
}