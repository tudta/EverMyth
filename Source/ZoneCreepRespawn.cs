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
        private static List<SpawnData> _zoneCreepSpawnDatas = null;

        private static trigger _respawnTrigger = null;
        private static unit _enumUnit = null;
        private static unit _spawnedUnit = null;
        private static unit _dyingUnit = null;
        private static int _dyingUnitIndex = 0;

        public static void Init()
        {
            Console.WriteLine("Zone creep respawn initialized!");
            InitializeCreepList();
            CreateRespawnTrigger();
        }

        private static void CreateRespawnTrigger()
        {
            _respawnTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(_respawnTrigger, EVENT_PLAYER_UNIT_DEATH);
            TriggerAddCondition(_respawnTrigger, Condition(IsDyingUnitZoneCreep));
            TriggerAddAction(_respawnTrigger, RespawnCreep);
        }

        // Create unit groups and lists from existing creeps on map and index spawn points
        private static void InitializeCreepList()
        {
            _zoneCreepUnitGroup = GetUnitsOfPlayerMatching(Player(PLAYER_NEUTRAL_AGGRESSIVE), Condition(IsUnitZoneCreep));
            _zoneCreepUnits = new List<unit>();
            _zoneCreepSpawnDatas = new List<SpawnData>();
            ForGroupBJ(_zoneCreepUnitGroup, IndexCreep);
        }

        // Index spawn points of creeps
        private static void IndexCreep()
        {
            _enumUnit = GetEnumUnit();
            _zoneCreepSpawnDatas.Add(new SpawnData() { RespawnTime = _respawnTime, SpawnPosX = GetUnitX(_enumUnit), SpawnPosY = GetUnitY(_enumUnit), SpawnRotation = GetUnitFacing(_enumUnit), UnitTypeId = GetUnitTypeId(_enumUnit) });
            _zoneCreepUnits.Add(_enumUnit);
        }

        private static void RespawnCreep()
        {
            _dyingUnit = GetDyingUnit();
            _dyingUnitIndex = _zoneCreepUnits.IndexOf(_dyingUnit);
            UnitManager.UnitInstanceDatabase.Remove(_dyingUnit);
            PolledWait(_zoneCreepSpawnDatas[_dyingUnitIndex].RespawnTime);
            _spawnedUnit = CreateUnit(Player(PLAYER_NEUTRAL_AGGRESSIVE), GetUnitTypeId(_dyingUnit), _zoneCreepSpawnDatas[_dyingUnitIndex].SpawnPosX, _zoneCreepSpawnDatas[_dyingUnitIndex].SpawnPosY, _zoneCreepSpawnDatas[_dyingUnitIndex].SpawnRotation);
            _zoneCreepUnits[_dyingUnitIndex] = _spawnedUnit;
            UnitManager.AddUnit(_spawnedUnit);
        }

        private static bool IsUnitZoneCreep()
        {
            for (int i = 0; i < Dungeons.DungeonManager.DungeonRects.Count; i++)
            {
                if (RectContainsUnit(Dungeons.DungeonManager.DungeonRects[i], GetFilterUnit()))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsDyingUnitZoneCreep()
        {
            if (_zoneCreepUnits.Contains(GetDyingUnit()))
            {
                return true;
            }
            return false;
        }
    }
}