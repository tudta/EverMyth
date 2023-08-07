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
using System.Collections;
using System.Timers;

namespace Source
{
    public static class SaveManager
    {
        public static Dictionary<player, List<SaveData>> SavesByPlayer { get; } = new Dictionary<player, List<SaveData>>();
        private static SaveSystem<SaveData> _saveSystem;
        private const int _maxSaveSlotCount = 100;

        public static void Init()
        {
            Console.WriteLine("Save manager initialized!");

            InitializeSaveSystem();
            LoadAllPlayerSavesToMemory();
        }

        private static void InitializeSaveSystem()
        {
            // Do not just copy/paste these options, you should pick your own hash and salt values.
            // You can use IntelliSense to get more information about the options.
            // Just know that Hash1, Hash2, Salt and SaveFolder are required.
            _saveSystem = new SaveSystem<SaveData>(new SaveSystemOptions
            {
                // These values have been presonally set.
                Hash1 = 760619,
                Hash2 = 318337,
                Salt = "kpBtkmxnxNx57UeO",
                BindSavesToPlayerName = true,
                SaveFolder = "TudtaRPG"
            });
            _saveSystem.OnSaveLoaded += SaveManager_OnSaveLoaded;
        }

        private static void LoadAllPlayerSavesToMemory()
        {
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                LoadPlayerSavesToMemory(PlayerManager.Players.ElementAt(i).Value);
            }
        }

        private static void LoadPlayerSavesToMemory(player targetPlayer)
        {
            int tmpIndex = 0;
            SavesByPlayer.Add(targetPlayer, new List<SaveData>());
            for (int i = 0; i < _maxSaveSlotCount; i++)
            {
                tmpIndex = i + 1;
                _saveSystem.Load(targetPlayer, tmpIndex);
            }
        }

        public static void SaveManager_OnSaveLoaded(SaveData save, LoadResult loadResult)
        {
            SavesByPlayer[save.GetPlayer()].Add(save);

            if (loadResult == LoadResult.Success)
            {
                //Console.WriteLine("Save slot " + save.GetSaveSlot() + ": populated save detected!");
            }

            // If the load result is anything except success, the save will be a newly created object.
            if (loadResult != LoadResult.Success)
            {
                // You can also just set the default value of the property to this.
                // This is just to illustrate why you may want to know when it is an empty save,
                // as then things like the heroes dictionary will not be created or filled.
                //Console.WriteLine("Save slot " + save.GetSaveSlot() + ": empty save detected!");
            }
            // Extension method for determining whether the load result is any of the failed states.
            if (loadResult.Failed())
            {
                //Console.WriteLine("Save slot " + save.GetSaveSlot() + ": an existing save failed to load correctly!");
            }
        }

        public static void SavePlayerData(player targetPlayer, int playerId, int saveSlot)
        {
            // Create save data.
            SaveData save = new SaveData();
            save.SetPlayer(targetPlayer);
            save.SetSaveSlot(saveSlot);
            save.IsPopulated = true;
            save.ResourceSaveData = PlayerManager.GetResourceData(playerId);
            save.HeroUnitData = PlayerManager.GetHeroUnitData(playerId);
            save.HeroInventoryData = PlayerManager.GetInventoryData(playerId);
            // Save data to dictionary.
            SavesByPlayer[targetPlayer][saveSlot - 1] = save;
            // Save data to file.
            SaveToFile(save);
        }

        private static void SaveToFile(SaveData save)
        {
            Console.WriteLine("Save is populated: " + save.IsPopulated);
            Console.WriteLine("Hero data:");
            Console.WriteLine("Unit type ID: " + save.HeroUnitData.UnitTypeId);
            Console.WriteLine("Level: " + save.HeroUnitData.Level);
            Console.WriteLine("Experience: " + save.HeroUnitData.Experience);
            Console.WriteLine("Health: " + save.HeroUnitData.Health);
            Console.WriteLine("Mana: " + save.HeroUnitData.Mana);
            Console.WriteLine("Strength: " + save.HeroUnitData.Strength);
            Console.WriteLine("Agility: " + save.HeroUnitData.Agility);
            Console.WriteLine("Intelligence: " + save.HeroUnitData.Intelligence);
            Console.WriteLine("Gold: " + save.ResourceSaveData.Gold);
            Console.WriteLine("Lumber: " + save.ResourceSaveData.Lumber);
            _saveSystem.Save(save);
        }

        public static void LoadPlayerData(player targetPlayer, int playerId, int saveSlot)
        {
            // Load data from dictionary.
            SaveData save = SavesByPlayer[targetPlayer][saveSlot - 1];
            // Swap player unit with hero id from loaded data.
            PlayerManager.SwapPlayerUnit(playerId, save.HeroUnitData.UnitTypeId, Location(Regions.HeroSpawnRegion.Center.X, Regions.HeroSpawnRegion.Center.Y), bj_UNIT_FACING, save.HeroUnitData);
            //unit playerUnit = PlayerManager.PlayerUnits[playerId];
            //// Assign appropriate stats, abilities, gold, lumber, and items.
            ////if (save.HeroSaveData.Level > 1)
            ////{
            ////    SetHeroLevel(playerUnit, save.HeroSaveData.Level, false);
            ////}
            //SetHeroXP(playerUnit, save.HeroUnitData.Experience, false);
            //SetHeroAgi(playerUnit, save.HeroUnitData.Agility, true);
            //SetHeroInt(playerUnit, save.HeroUnitData.Intelligence, true);
            //SetHeroStr(playerUnit, save.HeroUnitData.Strength, true);
            //UnitModifySkillPoints(playerUnit, (-1) * GetHeroSkillPoints(playerUnit));
            //UnitModifySkillPoints(playerUnit, save.HeroUnitData.SkillPoints);
            SetPlayerState(targetPlayer, PLAYER_STATE_RESOURCE_GOLD, save.ResourceSaveData.Gold);
            SetPlayerState(targetPlayer, PLAYER_STATE_RESOURCE_LUMBER, save.ResourceSaveData.Lumber);
            ItemSystem.PopulateInventory(PlayerManager.PlayerUnits[playerId], save.HeroInventoryData);
            ////set ability 
            //for (int i = 0; i < save.HeroUnitData.AbilityDatas.Count; i++)
            //{
            //    Console.WriteLine("Adding ability " + i + " with an id of " + save.HeroUnitData.AbilityDatas[i].AbilityId + " and a level of " + save.HeroUnitData.AbilityDatas[i].AbilityLevel + ".");
            //    UnitAddAbility(playerUnit, save.HeroUnitData.AbilityDatas[i].AbilityId);
            //    SetUnitAbilityLevel(playerUnit, save.HeroUnitData.AbilityDatas[i].AbilityId, save.HeroUnitData.AbilityDatas[i].AbilityLevel);
            //    //UnitModifySkillPoints(playerUnit, save.HeroSaveData.AbilityDatas[i].AbilityLevel * (-1));
            //}
        }
    }
}