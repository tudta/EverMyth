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
        private static trigger _heroRespawnTrigger = null;
        private static unit _dyingUnit = null;
        private static player _owningPlayer = null;
        private static float _spawnPosX = 0;
        private static float _spawnPosY = 0;

        public static void Init()
        {
            Console.WriteLine("Hero respawn initialized!");
            CreateRespawnTrigger();
            _spawnPosX = HeroSpawnRegion.Center.X;
            _spawnPosY = HeroSpawnRegion.Center.Y;
        }

        private static void CreateRespawnTrigger()
        {
            _heroRespawnTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(_heroRespawnTrigger, EVENT_PLAYER_UNIT_DEATH);
            TriggerAddCondition(_heroRespawnTrigger, Condition(IsAHero));
            TriggerAddAction(_heroRespawnTrigger, RespawnHero);
        }

        private static void RespawnHero()
        {
            _dyingUnit = GetDyingUnit();
            _owningPlayer = GetOwningPlayer(_dyingUnit);

            // Wait respawn time.
            PolledWait(_respawnTime);
            // Revive hero.
            ReviveHero(_dyingUnit, _spawnPosX, _spawnPosY, true);
            BuffManager.ApplyAttributePassives(UnitManager.HeroUnitInstanceDatabase[_dyingUnit]);
            // Restore health and mana.
            UnitManager.HeroUnitInstanceDatabase[_dyingUnit].UnitData.CurrentHealth = UnitManager.HeroUnitInstanceDatabase[_dyingUnit].UnitData.TotalMaximumHealth;
            UnitManager.HeroUnitInstanceDatabase[_dyingUnit].UnitData.CurrentMana = UnitManager.HeroUnitInstanceDatabase[_dyingUnit].UnitData.TotalMaximumMana;
            // Move camera.
            PanCameraToForPlayer(_owningPlayer, _spawnPosX, _spawnPosY);
            // Select hero.
            SelectUnitForPlayerSingle(_dyingUnit, _owningPlayer);
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
