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

namespace Source
{
    public static class HeroExperience
    {
        private const float _experienceRange = 1200.0f;
        public static void Init()
        {
            Console.WriteLine("Hero experience initialized!");
            CreateExperienceTrigger();
        }

        private static void CreateExperienceTrigger()
        {
            trigger experienceTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(experienceTrigger, EVENT_PLAYER_UNIT_DEATH);
            TriggerAddCondition(experienceTrigger, Condition(CanAwardExperience));
            TriggerAddAction(experienceTrigger, GiveExperienceToUnitsInRange);
        }

        private static void GiveExperienceToUnitsInRange()
        {
            group unitsInRange = GetUnitsInRangeOfLocMatching(_experienceRange, GetUnitLoc(GetDyingUnit()), Condition(IsKillingUnitPlayerControlled));
            ForGroupBJ(unitsInRange, GiveExperienceToUnit);
        }

        private static void GiveExperienceToUnit()
        {
            unit enumUnit = GetEnumUnit();
            unit dyingUnit = GetDyingUnit();
            if (IsHeroUnitId(GetUnitTypeId(GetEnumUnit())))
            {
                int baseExperience = UnitManager.UnitInstanceDatabase[dyingUnit].UnitData.ExperienceAwarded;
                float experienceModifier = UnitManager.HeroUnitInstanceDatabase[enumUnit].UnitData.ExperienceGainModifier;
                UnitManager.HeroUnitInstanceDatabase[enumUnit].UnitData.Experience += MathRound(baseExperience + (baseExperience * experienceModifier));
                int baseGold = baseExperience;
                float goldModifier = 1.0f;
                int goldAddAmount = MathRound(baseGold * goldModifier);
                int currentGold = GetPlayerState(GetOwningPlayer(enumUnit), PLAYER_STATE_RESOURCE_GOLD);
                SetPlayerState(GetOwningPlayer(enumUnit), PLAYER_STATE_RESOURCE_GOLD, currentGold + goldAddAmount);
                texttag goldText = CreateTextTagUnitBJ("+" + goldAddAmount + " gold", enumUnit, -57.0f, 9.0f, 100.0f, 100.0f, 0.0f, 0.0f);
                SetTextTagVelocity(goldText, 0.0f, 0.02f);
                SetTextTagPermanent(goldText, false);
                SetTextTagLifespan(goldText, 2.0f);
                SetTextTagFadepoint(goldText, 1.0f);
                //SetPlayerState(targetPlayer, PLAYER_STATE_RESOURCE_LUMBER, save.ResourceSaveData.Lumber);
            }
        }

        private static bool CanAwardExperience()
        {
            if (GetOwningPlayer(GetDyingUnit()) == Player(PLAYER_NEUTRAL_AGGRESSIVE) && IsKillingUnitPlayerControlled())
            {
                return true;
            }
            return false;
        }

        private static bool IsKillingUnitPlayerControlled()
        {
            if (GetOwningPlayer(GetKillingUnit()) != Player(PLAYER_NEUTRAL_AGGRESSIVE) && GetOwningPlayer(GetKillingUnit()) != Player(PLAYER_NEUTRAL_PASSIVE))
            {
                return true;
            }
            return false;
        }
    }
}