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
    public static class ZoneCreepRespawn
    {
        private static float _respawnTime = 2.0f;
        private static group _zoneCreepUnitGroup = null;
        private static List<unit> _zoneCreepUnits = null;
        private static List<location> _zoneCreepPoints = null;

        public static void Init()
        {
            Console.WriteLine("Zone creep respawn initialized!");
            InitializeCreepList();
            CreateRespawnTrigger();
        }

        private static void CreateRespawnTrigger()
        {
            trigger respawnTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(respawnTrigger, EVENT_PLAYER_UNIT_DEATH);
            TriggerAddCondition(respawnTrigger, Condition(IsAZoneCreep));
            TriggerAddAction(respawnTrigger, RespawnCreep);
        }

        // Create unit groups and lists from existing creeps on map and index spawn points
        private static void InitializeCreepList()
        {
            _zoneCreepUnitGroup = GetUnitsInRectOfPlayer(GetPlayableMapRect(), Player(PLAYER_NEUTRAL_AGGRESSIVE));
            _zoneCreepUnits = new List<unit>();
            _zoneCreepPoints = new List<location>();
            ForGroupBJ(_zoneCreepUnitGroup, IndexCreep);
        }

        // Index spawn points of creeps
        private static void IndexCreep()
        {
            unit enumUnit = GetEnumUnit();
            _zoneCreepPoints.Add(GetUnitLoc(enumUnit));
            _zoneCreepUnits.Add(enumUnit);
        }

        private static void RespawnCreep()
        {
            unit dyingUnit = GetDyingUnit();
            int dyingUnitIndex = _zoneCreepUnits.IndexOf(dyingUnit);
            UnitManager.UnitInstanceDatabase.Remove(dyingUnit);
            PolledWait(_respawnTime);
            unit spawnedUnit = CreateUnitAtLoc(Player(PLAYER_NEUTRAL_AGGRESSIVE), GetUnitTypeId(dyingUnit), _zoneCreepPoints[dyingUnitIndex], bj_UNIT_FACING);
            _zoneCreepUnits[dyingUnitIndex] = spawnedUnit;
            UnitManager.AddUnit(spawnedUnit);
        }

        private static bool IsAZoneCreep()
        {
            if (_zoneCreepUnits.Contains(GetDyingUnit()))
            {
                return true;
            }
            return false;
        }
    }
}
