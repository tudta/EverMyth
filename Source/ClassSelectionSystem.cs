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
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source
{
    public static class ClassSelectionSystem
    {
        private static float _regionCenterX = 0;
        private static float _regionCenterY = 0;

        public static void Init()
        {
            Console.WriteLine("Class selection system initialized!");
            SetHeroSpawnCoordinates();
            CreateSelectionTriggers();
        }

        private static void SetHeroSpawnCoordinates()
        {
            _regionCenterX = Regions.HeroSpawnRegion.Center.X;
            _regionCenterY = Regions.HeroSpawnRegion.Center.Y;
        }

        private static void CreateSelectionTriggers()
        {
            trigger archerSelectTrigger = CreateTrigger();
            TriggerRegisterEnterRegionSimple(archerSelectTrigger, Regions.ArcherSelectRegion.Region);
            TriggerAddCondition(archerSelectTrigger, Condition(IsBaseClass));
            TriggerAddAction(archerSelectTrigger, ChooseArcher);

            trigger mageSelectTrigger = CreateTrigger();
            TriggerRegisterEnterRegionSimple(mageSelectTrigger, Regions.MageSelectRegion.Region);
            TriggerAddCondition(mageSelectTrigger, Condition(IsBaseClass));
            TriggerAddAction(mageSelectTrigger, ChooseMage);

            trigger warriorSelectTrigger = CreateTrigger();
            TriggerRegisterEnterRegionSimple(warriorSelectTrigger, Regions.WarriorSelectRegion.Region);
            TriggerAddCondition(warriorSelectTrigger, Condition(IsBaseClass));
            TriggerAddAction(warriorSelectTrigger, ChooseWarrior);
        }

        public static void ChooseArcher()
        {
            Console.WriteLine("You have chosen the Archer hero!");

            unit enteringUnit = GetEnteringUnit();
            player owningPlayer = GetOwningPlayer(enteringUnit);

            PlayerManager.SwapPlayerUnit(GetPlayerId(owningPlayer), Constants.UNIT_ARCHER, Location(_regionCenterX, _regionCenterY), bj_UNIT_FACING);
        }

        public static void ChooseMage()
        {
            Console.WriteLine("You have chosen the Mage hero!");

            unit enteringUnit = GetEnteringUnit();
            player owningPlayer = GetOwningPlayer(enteringUnit);

            PlayerManager.SwapPlayerUnit(GetPlayerId(owningPlayer), Constants.UNIT_MAGE, Location(_regionCenterX, _regionCenterY), bj_UNIT_FACING);
        }

        public static void ChooseWarrior()
        {
            Console.WriteLine("You have chosen the Warrior hero!");

            unit enteringUnit = GetEnteringUnit();
            player owningPlayer = GetOwningPlayer(enteringUnit);

            PlayerManager.SwapPlayerUnit(GetPlayerId(owningPlayer), Constants.UNIT_WARRIOR, Location(_regionCenterX, _regionCenterY), bj_UNIT_FACING);
        }

        public static bool IsBaseClass()
        {
            Console.WriteLine("Checking for base class!");
            if (GetUnitTypeId(GetEnteringUnit()) == Constants.UNIT_SOUL_OF_HERO)
            {
                return true;
            }
            return false;
        }
    }
}
