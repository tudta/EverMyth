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
    public static class HeroRespawn
    {
        private static float _respawnTime = 2.0f;

        public static void Init()
        {
            Console.WriteLine("Hero respawn initialized!");
            CreateRespawnTrigger();
        }

        private static void CreateRespawnTrigger()
        {
            trigger respawnTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(respawnTrigger, EVENT_PLAYER_UNIT_DEATH);
            TriggerAddCondition(respawnTrigger, Condition(IsAHero));
            TriggerAddAction(respawnTrigger, RespawnHero);
        }

        private static void RespawnHero()
        {
            unit dyingUnit = GetDyingUnit();
            player owningPlayer = GetOwningPlayer(dyingUnit);
            location spawnLocation = Location(HeroSpawnRegion.Center.X, HeroSpawnRegion.Center.Y);

            // Wait respawn time.
            PolledWait(_respawnTime);
            // Revive hero.
            ReviveHeroLoc(dyingUnit, spawnLocation, true);
            BuffManager.ApplyAttributePassives(UnitManager.HeroUnitInstanceDatabase[dyingUnit]);
            // Restore health and mana.
            UnitManager.HeroUnitInstanceDatabase[dyingUnit].UnitData.CurrentHealth = UnitManager.HeroUnitInstanceDatabase[dyingUnit].UnitData.TotalMaximumHealth;
            UnitManager.HeroUnitInstanceDatabase[dyingUnit].UnitData.CurrentMana = UnitManager.HeroUnitInstanceDatabase[dyingUnit].UnitData.TotalMaximumMana;
            // Move camera.
            PanCameraToTimedLocForPlayer(owningPlayer, spawnLocation, 0.0f);
            // Select hero.
            SelectUnitForPlayerSingle(dyingUnit, owningPlayer);
        }

        private static bool IsAHero()
        {
            if (IsUnitType(GetDyingUnit(), UNIT_TYPE_HERO))
            {
                return true;
            }
            return false;
        }
    }
}
