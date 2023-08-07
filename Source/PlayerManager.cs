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
    public static class PlayerManager
    {
        private static Dictionary<int, player> _players = null;
        private static Dictionary<int, unit> _playerUnits = null;

        public static Dictionary<int, player> Players { get => _players; }
        public static Dictionary<int, unit> PlayerUnits { get => _playerUnits; }

        public static void Init()
        {
            Console.WriteLine("Player unit manager initialized!");

            InitializePlayerData();
            GenerateStartingUnits();
        }

        private static void InitializePlayerData()
        {
            Console.WriteLine("Initializing player data!");
            force allPlayerForce = GetPlayersAll();
            _players = new Dictionary<int, player>();
            _playerUnits = new Dictionary<int, unit>();
            ForForce(allPlayerForce, AddPlayerData);
            Console.WriteLine("Player count: " + _players.Count);
        }

        private static void AddPlayerData()
        {
            player tmpEnumPlayer = GetEnumPlayer();
            int tmpPlayerId = GetPlayerId(tmpEnumPlayer);
            _players.Add(tmpPlayerId, tmpEnumPlayer);
            _playerUnits.Add(tmpPlayerId, null);
        }

        /// <summary>
        /// Create starting unit for each player
        /// </summary>
        private static void GenerateStartingUnits()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                GenerateStartingUnit(_players.ElementAt(i).Value);
            }
        }

        /// <summary>
        /// Create starting unit for player (wisp/etc)
        /// </summary>
        /// <param name="tgtPlayer"></param>
        private static void GenerateStartingUnit(player tgtPlayer)
        {
            int playerId = GetPlayerId(tgtPlayer);
            SwapPlayerUnit(playerId, Constants.UNIT_SOUL_OF_HERO, Location(Regions.GameStartRegion.Center.X, Regions.GameStartRegion.Center.Y), bj_UNIT_FACING);
        }

        private static bool IsActivePlayer(player tmpPlayer)
        {
            if (GetPlayerController(tmpPlayer) == MAP_CONTROL_USER && GetPlayerSlotState(tmpPlayer) == PLAYER_SLOT_STATE_PLAYING)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Use to swap to a new hero unit
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="unitType"></param>
        /// <param name="unitLocation"></param>
        /// <param name="unitAngle"></param>
        public static void SwapPlayerUnit(int playerId, int unitType, location unitLocation, float unitAngle)
        {
            Console.WriteLine("Swapping player unit with a new hero!");
            // Remove hero unit from unit manager
            UnitManager.RemoveHeroUnit(_playerUnits[playerId]);
            // Remove old unit from game
            RemoveUnit(_playerUnits[playerId]);
            // Create new unit and replace in array
            _playerUnits[playerId] = CreateUnitAtLoc(_players[playerId], unitType, unitLocation, unitAngle);

            // Add hero unit to unit manager
            // Assign custom unit data
            UnitManager.AddHeroUnit(_playerUnits[playerId]);

            // Select unit
            SelectUnitForPlayerSingle(_playerUnits[playerId], _players[playerId]);
            // Pan camera
            //PanCameraToLocForPlayer(_players[playerId], unitLocation);
            SetCameraPositionLocForPlayer(_players[playerId], unitLocation);
        }

        /// <summary>
        /// Use to swap to a loaded hero unit
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="unitType"></param>
        /// <param name="unitLocation"></param>
        /// <param name="unitAngle"></param>
        /// <param name="heroData"></param>
        public static void SwapPlayerUnit(int playerId, int unitType, location unitLocation, float unitAngle, HeroData heroData)
        {
            Console.WriteLine("Swapping player unit with a loaded hero!");
            // Remove hero unit from unit manager
            UnitManager.RemoveHeroUnit(_playerUnits[playerId]);
            // Remove old unit from game
            RemoveUnit(_playerUnits[playerId]);
            // Create new unit and replace in array
            _playerUnits[playerId] = CreateUnitAtLoc(_players[playerId], unitType, unitLocation, unitAngle);

            // Add hero unit to unit manager
            // Assign custom unit data
            UnitCombatData unitData = new UnitCombatData(heroData);
            UnitManager.AddHeroUnit(_playerUnits[playerId], unitData);

            // Select unit
            SelectUnitForPlayerSingle(_playerUnits[playerId], _players[playerId]);
            // Pan camera
            //PanCameraToLocForPlayer(_players[playerId], unitLocation);
            SetCameraPositionLocForPlayer(_players[playerId], unitLocation);
        }

        public static ResourceData GetResourceData(int playerId)
        {
            ResourceData tmpResourceData = new ResourceData
            {
                Gold = GetPlayerState(Player(playerId), PLAYER_STATE_RESOURCE_GOLD),
                Lumber = GetPlayerState(Player(playerId), PLAYER_STATE_RESOURCE_LUMBER)
            };
            return tmpResourceData;
        }

        public static HeroData GetHeroUnitData(int playerId)
        {
            unit heroUnit = _playerUnits[playerId];
            UnitCombatData unitData = UnitManager.HeroUnitInstanceDatabase[heroUnit].UnitData;
            HeroData heroData = new HeroData()
            {
                UnitTypeId = unitData.UnitTypeId,
                Level = unitData.Level,
                Experience = unitData.Experience,
                Health = unitData.BaseMaximumHealth,
                Mana = unitData.BaseMaximumMana,
                HealthRegeneration = unitData.BaseHealthRegeneration,
                ManaRegeneration = unitData.BaseManaRegeneration,
                PrimaryAttribute = unitData.PrimaryAttribute,
                Strength = unitData.BaseStrength,
                Agility = unitData.BaseAgility,
                Intelligence = unitData.BaseIntelligence,
                AttackCooldown = unitData.BaseAttackCooldown,
                PhysicalAttackDamage = unitData.BasePhysicalAttackDamage,
                MagicalAttackDamage = unitData.BaseMagicalAttackDamage,
                PhysicalDamageReduction = unitData.BasePhysicalDamageReduction,
                MagicalDamageReduction = unitData.BaseMagicalDamageReduction,
                SkillPoints = unitData.SkillPoints,
                AbilityDatas = unitData.AbilityDatas
            };
            Console.WriteLine("Getting hero unit data from PlayerManager!");
            return heroData;
        }

        public static InventoryData GetInventoryData(int playerId)
        {
            InventoryData tmpInventoryData = new InventoryData();
            tmpInventoryData.HeroInventoryIds = new int[6];
            List<ItemData> inventoryItems = ItemSystem.GetUnitInventoryItems(_playerUnits[playerId]);
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i] != null)
                {
                    tmpInventoryData.HeroInventoryIds[i] = inventoryItems[i].ItemId;
                }
            }
            return tmpInventoryData;
        }
    }
}