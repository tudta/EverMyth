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
    public abstract class Dungeon
    {
        private string _dungeonName = string.Empty;

        private bool _isEntranceTimerRunning = false;
        private timer _entranceTimer = null;
        private float _enterTime = 5.0f;
        private Rectangle _entranceRegion = null;
        private trigger _entranceInTrigger = null;
        private trigger _entranceOutTrigger = null;
        private List<unit> _entranceUnits = null;

        private Rectangle _startRegion = null;
        private float _startX = 0.0f;
        private float _startY = 0.0f;

        private Rectangle _exitRegion = null;
        private float _exitX = 0.0f;
        private float _exitY = 0.0f;

        private trigger _heroUnitDeathTrigger = null;
        
        private bool _isActive = false;
        private List<unit> _dungeonPartyUnits = null;
        private List<ItemDropData> _dropTable = null;

        private player _owningPlayer = null;
        private unit _dyingUnit = null;
        private unit _triggerUnit = null;
        private unit _filterUnit = null;
        private float _dropRoll = 0.0f;

        protected string DungeonName
        {
            get
            {
                return _dungeonName;
            }

            set
            {
                _dungeonName = value;
            }
        }

        protected Rectangle EntranceRegion
        {
            get
            {
                return _entranceRegion;
            }

            set
            {
                _entranceRegion = value;
            }
        }

        protected Rectangle StartRegion
        {
            get
            {
                return _startRegion;
            }

            set
            {
                _startRegion = value;
            }
        }

        public Rectangle ExitRegion
        {
            get
            {
                return _exitRegion;
            }

            set
            {
                _exitRegion = value;
            }
        }

        public List<ItemDropData> DropTable
        {
            get
            {
                return _dropTable;
            }

            set
            {
                _dropTable = value;
            }
        }

        public virtual void Init()
        {
            Console.WriteLine("Initializing " + _dungeonName + " dungeon!");
            InitializeEntranceSystem();
            _dungeonPartyUnits = new List<unit>();
            _entranceUnits = new List<unit>();
            _startX = _startRegion.Center.X;
            _startY = _startRegion.Center.Y;
            _exitX = _exitRegion.Center.X;
            _exitY = _exitRegion.Center.Y;
            InitializeHeroDeathTrigger();
        }

        protected virtual void InitializeEntranceSystem()
        {
            _entranceTimer = CreateTimer();
            _entranceInTrigger = CreateTrigger();
            TriggerRegisterEnterRegionSimple(_entranceInTrigger, _entranceRegion.Region);
            TriggerAddCondition(_entranceInTrigger, Condition(IsTriggeringUnitHero));
            TriggerAddAction(_entranceInTrigger, CalculateEntranceState);
            _entranceOutTrigger = CreateTrigger();
            TriggerRegisterLeaveRegionSimple(_entranceOutTrigger, _entranceRegion.Region);
            TriggerAddCondition(_entranceOutTrigger, Condition(IsTriggeringUnitHero));
            TriggerAddAction(_entranceOutTrigger, CalculateEntranceState);
        }

        private void InitializeHeroDeathTrigger()
        {
            _heroUnitDeathTrigger = CreateTrigger();
            TriggerAddCondition(_heroUnitDeathTrigger, Condition(IsDyingUnitHero));
            TriggerAddAction(_heroUnitDeathTrigger, HandleHeroUnitDeath);
        }

        protected bool IsDyingUnitHero()
        {
            if (IsHeroUnitId(GetUnitTypeId(GetDyingUnit())))
            {
                return true;
            }
            return false;
        }

        protected bool IsTriggeringUnitHero()
        {
            _triggerUnit = GetTriggerUnit();
            //Console.WriteLine("Region unit is " + GetUnitName(_triggerUnit) + "!");
            if (IsHeroUnitId(GetUnitTypeId(_triggerUnit)))
            {
                //Console.WriteLine("Region unit is a hero!");
                return true;
            }
            return false;
        }

        protected bool IsFilterUnitHero()
        {
            _filterUnit = GetFilterUnit();
            //Console.WriteLine("Checking if unit inside of entrance is a hero!");
            //Console.WriteLine("Unit inside of entrance is " + GetUnitName(_filterUnit) + "!");
            if (IsHeroUnitId(GetUnitTypeId(_filterUnit)))
            {
                return true;
            }
            return false;
        }

        protected virtual void CalculateEntranceState()
        {
            //Console.WriteLine("Calculating entrance state!");
            CalculateEntranceUnitState();
            if (_entranceUnits.Count > 0)
            {
                //Console.WriteLine("Entrance units are present!");
                if (!_isEntranceTimerRunning)
                {
                    // Start timer.
                    Console.WriteLine("Starting entrance countdown for " + _dungeonName + " dungeon!");
                    _isEntranceTimerRunning = true;
                    TimerStart(_entranceTimer, _enterTime, false, EnterDungeon);
                }
                return;
            }
            //Console.WriteLine("No entrance units are present!");
            if (_isEntranceTimerRunning)
            {
                // Stop timer.
                Console.WriteLine("Stopping entrance countdown for " + _dungeonName + " dungeon!");
                _isEntranceTimerRunning = false;
                PauseTimer(_entranceTimer);
            }
        }

        protected virtual void CalculateEntranceUnitState()
        {
            _triggerUnit = GetTriggerUnit();
            //Console.WriteLine("Calculating entrance unit state!");
            if (RectContainsUnit(_entranceRegion.Rect, _triggerUnit))
            {
                //Console.WriteLine("Adding " + GetUnitName(_triggerUnit) + " to entrance unit list!");
                _entranceUnits.Add(_triggerUnit);
                return;
            }
            //Console.WriteLine("Removing " + GetUnitName(_triggerUnit) + " from entrance unit list!");
            _entranceUnits.Remove(_triggerUnit);
        }

        protected void AddHeroUnitsToDungeonInstance()
        {
            //Console.WriteLine("Adding " + _entranceUnits.Count + " hero units to dungeon party!");
            for (int i = 0; i < _entranceUnits.Count; i++)
            {
                AddHeroUnitToDungeonInstance(_entranceUnits[i]);
            }
        }

        protected virtual void AddHeroUnitToDungeonInstance(unit entranceUnit)
        {
            //Console.WriteLine("Teleporting " + GetUnitName(entranceUnit) + " to dungeon start!");
            _owningPlayer = GetOwningPlayer(entranceUnit);
            SetUnitPosition(entranceUnit, _startRegion.Center.X, _startRegion.Center.Y);
            PanCameraToTimedForPlayer(_owningPlayer, _startX, _startY, 0.0f);
            SelectUnitForPlayerSingle(entranceUnit, _owningPlayer);
            _dungeonPartyUnits.Add(entranceUnit);
        }

        protected void HandleHeroUnitDeath()
        {
            RemoveDyingHeroUnitFromDungeonInstance();
            if (_dungeonPartyUnits.Count == 0)
            {
                DeactivateDungeon();
            }
        }

        protected void RemoveDyingHeroUnitFromDungeonInstance()
        {
            _dungeonPartyUnits.Remove(GetDyingUnit());
        }

        protected virtual void EnterDungeon()
        {
            Console.WriteLine("Entered " + _dungeonName + " dungeon!");
            // Teleport players to start point.
            AddHeroUnitsToDungeonInstance();
            ActivateDungeon();
        }

        protected virtual void ActivateDungeon()
        {
            //Console.WriteLine(_dungeonName + " dungeon has been activated!");
            _isActive = true;
            // Spawn dungeon units.
        }

        protected virtual void DeactivateDungeon()
        {
            //Console.WriteLine(_dungeonName + " dungeon has been deactivated!");
            _isActive = false;
            _dungeonPartyUnits.Clear();
            _dungeonPartyUnits.TrimExcess();
        }

        protected virtual void CompleteDungeon()
        {
            Console.WriteLine("Completed " + _dungeonName + " dungeon!");
            // Remove players from dungeon.
            TeleportHeroUnitsOutOfDungeon();
            // Reward players with items from drop table.
            DistributeDrops();
            // Deactivate dungeon.
            DeactivateDungeon();
        }

        protected void TeleportHeroUnitsOutOfDungeon()
        {
            for (int i = 0; i < _dungeonPartyUnits.Count; i++)
            {
                _owningPlayer = GetOwningPlayer(_dungeonPartyUnits[i]);

                SetUnitPosition(_dungeonPartyUnits[i], _exitX, _exitY);
                // Move camera.
                PanCameraToTimedForPlayer(_owningPlayer, _exitX, _exitY, 0.0f);
                // Select hero.
                SelectUnitForPlayerSingle(_dungeonPartyUnits[i], _owningPlayer);
            }
        }

        protected void DistributeDrops()
        {
            for (int i = 0; i < _dungeonPartyUnits.Count; i++)
            {
                for (int x = 0; x < _dropTable.Count; x++)
                {
                    //roll for item
                    _dropRoll = GetRandomReal(0.000f, 0.999f);
                    Console.WriteLine("Drop roll is " + _dropRoll + " and needs to be lower than " + _dropTable[x].DropChance + "!");
                    //give item if roll was successful
                    if (_dropTable[x].DropChance > _dropRoll)
                    {
                        for (int y = 0; y < _dropTable[x].DropAmount; y++)
                        {
                            UnitAddItemById(_dungeonPartyUnits[i], _dropTable[x].ItemId);
                        }
                    }
                }
            }
        }
    }
}