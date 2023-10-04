using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
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
using WCSharp.Shared.Data;
using WCSharp.Sync;
using static Constants;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.Dungeons
{
    public class CaveDungeon : Dungeon
    {
        private trigger _dungeonCreepDeathTrigger = null;

        private destructable _area1Gate = null;
        private group _area1UnitGroup = null;
        private List<unit> _area1ZoneUnits = null;
        private List<SpawnData> _area1ZoneSpawnDatas = null;
        private bool _isArea1Cleared = false;

        private group _area2UnitGroup = null;
        private List<unit> _area2ZoneUnits = null;
        private List<SpawnData> _area2ZoneSpawnDatas = null;
        private bool _isArea2Cleared = false;

        private unit _enumUnit = null;
        private unit _spawnedUnit = null;

        public override void Init()
        {
            DungeonName = "Ancient Lair";
            EntranceRegion = Regions.Dungeon1Entrance;
            StartRegion = Regions.Dungeon1Start;
            ExitRegion = Regions.Dungeon1Exit;
            EnumDestructablesInRect(Regions.Dungeon1Gate.Rect, Condition(null), AssignGate);
            InitializeDungeonCreeps();
            CreateDeathTrigger();
            DropTable = new List<ItemDropData>()
            {
                new ItemDropData() { ItemId = Constants.ITEM_CLAWS_OF_PASSIVE_BOP, DropAmount = 1, DropChance = 0.40f },
                new ItemDropData() { ItemId = Constants.ITEM_DUMMYITEM, DropAmount = 1, DropChance = 0.40f },
                new ItemDropData() { ItemId = Constants.ITEM_CLAWS_OF_BOP, DropAmount = 1, DropChance = 0.40f }
            };
            base.Init();
        }

        protected override void InitializeEntranceSystem()
        {
            base.InitializeEntranceSystem();
        }

        protected override void CalculateEntranceState()
        {
            base.CalculateEntranceState();
        }

        protected override void CalculateEntranceUnitState()
        {
            base.CalculateEntranceUnitState();
        }

        protected override void AddHeroUnitToDungeonInstance(unit entranceUnit)
        {
            base.AddHeroUnitToDungeonInstance(entranceUnit);
        }

        protected override void EnterDungeon()
        {
            base.EnterDungeon();
        }

        protected override void ActivateDungeon()
        {
            SpawnArea1Creeps();
            base.ActivateDungeon();
        }

        protected override void DeactivateDungeon()
        {
            _isArea1Cleared = false;
            _isArea2Cleared = false;
            ModifyGateBJ(bj_GATEOPERATION_CLOSE, _area1Gate);
            DespawnCreeps();
            base.DeactivateDungeon();
        }

        protected override void CompleteDungeon()
        {
            base.CompleteDungeon();
        }

        private void InitializeDungeonCreeps()
        {
            _area1ZoneUnits = new List<unit>();
            _area1ZoneSpawnDatas = new List<SpawnData>();
            _area1UnitGroup = GetUnitsInRectAll(Regions.Dungeon1Area1.Rect);
            //Console.WriteLine("Dungeon area 1 mob size: " + BlzGroupGetSize(_area1UnitGroup) + "!");
            ForGroup(_area1UnitGroup, IndexArea1Creeps);
            _area2ZoneUnits = new List<unit>();
            _area2ZoneSpawnDatas = new List<SpawnData>();
            _area2UnitGroup = GetUnitsInRectAll(Regions.Dungeon1Area2.Rect);
            //Console.WriteLine("Dungeon area 2 mob size: " + BlzGroupGetSize(_area2UnitGroup) + "!");
            ForGroup(_area2UnitGroup, IndexArea2Creeps);
        }

        private void IndexArea1Creeps()
        {
            _enumUnit = GetEnumUnit();
            //Console.WriteLine("Enum unit name: " + GetUnitName(_enumUnit));
            //_area1ZoneUnits.Add(_enumUnit);
            _area1ZoneSpawnDatas.Add(new SpawnData() { RespawnTime = 0.0f, SpawnPosX = GetUnitX(_enumUnit), SpawnPosY = GetUnitY(_enumUnit), SpawnRotation = GetUnitFacing(_enumUnit), UnitTypeId = GetUnitTypeId(_enumUnit) });
            UnitManager.UnitInstanceDatabase.Remove(_enumUnit);
            RemoveUnit(_enumUnit);
        }

        private void IndexArea2Creeps()
        {
            _enumUnit = GetEnumUnit();
            //Console.WriteLine("Enum unit name: " + GetUnitName(_enumUnit));
            //_area2ZoneUnits.Add(_enumUnit);
            _area2ZoneSpawnDatas.Add(new SpawnData() { RespawnTime = 0.0f, SpawnPosX = GetUnitX(_enumUnit), SpawnPosY = GetUnitY(_enumUnit), SpawnRotation = GetUnitFacing(_enumUnit), UnitTypeId = GetUnitTypeId(_enumUnit) });
            UnitManager.UnitInstanceDatabase.Remove(_enumUnit);
            RemoveUnit(_enumUnit);
        }

        private void DespawnCreeps()
        {
            for (int i = 0; i < _area1ZoneUnits.Count; i++)
            {
                UnitManager.UnitInstanceDatabase.Remove(_area1ZoneUnits[i]);
            }
            _area1ZoneUnits.Clear();
            _area1ZoneUnits.TrimExcess();
            for (int x = 0; x < _area2ZoneUnits.Count; x++)
            {
                UnitManager.UnitInstanceDatabase.Remove(_area2ZoneUnits[x]);
            }
            _area2ZoneUnits.Clear();
            _area2ZoneUnits.TrimExcess();
        }

        private void SpawnArea1Creeps()
        {
            for (int i = 0; i < _area1ZoneSpawnDatas.Count; i++)
            {
                _spawnedUnit = CreateUnit(Player(PLAYER_NEUTRAL_AGGRESSIVE), _area1ZoneSpawnDatas[i].UnitTypeId, _area1ZoneSpawnDatas[i].SpawnPosX, _area1ZoneSpawnDatas[i].SpawnPosY, _area1ZoneSpawnDatas[i].SpawnRotation);
                _area1ZoneUnits.Add(_spawnedUnit);
                UnitManager.AddUnit(_spawnedUnit);
            }
        }

        private void SpawnArea2Creeps()
        {
            for (int i = 0; i < _area2ZoneSpawnDatas.Count; i++)
            {
                _spawnedUnit = CreateUnit(Player(PLAYER_NEUTRAL_AGGRESSIVE), _area2ZoneSpawnDatas[i].UnitTypeId, _area2ZoneSpawnDatas[i].SpawnPosX, _area2ZoneSpawnDatas[i].SpawnPosY, _area2ZoneSpawnDatas[i].SpawnRotation);
                _area2ZoneUnits.Add(_spawnedUnit);
                UnitManager.AddUnit(_spawnedUnit);
            }
        }

        private void AssignGate()
        {
            _area1Gate = GetFilterDestructable();
            //Console.WriteLine("Area 1 gate name = " + GetDestructableName(_area1Gate));
        }

        private void CreateDeathTrigger()
        {
            _dungeonCreepDeathTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(_dungeonCreepDeathTrigger, EVENT_PLAYER_UNIT_DEATH);
            TriggerAddCondition(_dungeonCreepDeathTrigger, Condition(IsDungeonCreep));
            TriggerAddAction(_dungeonCreepDeathTrigger, HandleUnitDeath);
        }

        private void HandleUnitDeath()
        {
            if (IsArea1Unit())
            {
                _area1ZoneUnits.Remove(GetDyingUnit());
            }
            else if (IsArea2Unit())
            {
                _area2ZoneUnits.Remove(GetDyingUnit());
            }
            CheckAreaClearStates();
        }

        private void CheckAreaClearStates()
        {
            if (!_isArea1Cleared)
            {
                CheckArea1ClearState();
                return;
            }
            CheckArea2ClearState();
        }

        private void CheckArea1ClearState()
        {
            if (_area1ZoneUnits.Count == 0)
            {
                //zone clear
                _isArea1Cleared = true;
                //trigger
                OpenArea2();
            }
        }

        private void CheckArea2ClearState()
        {
            if (_area2ZoneUnits.Count == 0)
            {
                //zone clear
                _isArea2Cleared = true;
                //dungeon clear
                CompleteDungeon();
            }
        }

        private void OpenArea2()
        {
            ModifyGateBJ(bj_GATEOPERATION_OPEN, _area1Gate);
            //spawn boss
            SpawnArea2Creeps();
        }

        private bool IsDungeonCreep()
        {
            if (RectContainsUnit(Regions.Dungeon1Zone.Rect, GetDyingUnit()))
            {
                return true;
            }
            return false;
        }

        private bool IsArea1Unit()
        {
            if (_area1ZoneUnits.Contains(GetDyingUnit()))
            {
                return true;
            }
            return false;
        }

        private bool IsArea2Unit()
        {
            if (_area2ZoneUnits.Contains(GetDyingUnit()))
            {
                return true;
            }
            return false;
        }
    }
}