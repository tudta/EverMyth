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
using WCSharp.Shared;
using WCSharp.Sync;
using static Constants;
using static Regions;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source
{
    public static class UnitManager
    {
        // KV pair = constant ID, UnitData
        private static Dictionary<int, UnitCombatData> _unitReferenceDatabase;

        // KV pair = unit instance, UnitData
        private static Dictionary<unit, UnitInstance> _unitInstanceDatabase;

        private static Dictionary<int, UnitCombatData> _heroUnitReferenceDatabase;

        private static Dictionary<unit, UnitInstance> _heroUnitInstanceDatabase;

        public static Dictionary<unit, UnitInstance> UnitInstanceDatabase { get => _unitInstanceDatabase; set => _unitInstanceDatabase = value; }

        public static Dictionary<unit, UnitInstance> HeroUnitInstanceDatabase { get => _heroUnitInstanceDatabase; set => _heroUnitInstanceDatabase = value; }

        public static void Init()
        {
            Console.WriteLine("UnitManager initialized!");
            InitializeUnitReferenceDatabase();
            InitializeUnitInstanceDatabase();
            InitializeHeroUnitReferenceDatabase();
            InitializeHeroUnitInstanceDatabase();
        }

        public static void AddUnit(unit newUnit)
        {
            _unitInstanceDatabase.Add(newUnit, null);
            AssignReferenceDataToUnitInstance(newUnit);
        }

        /// <summary>
        /// Use to create new hero unit
        /// </summary>
        /// <param name="newUnit"></param>
        public static void AddHeroUnit(unit newUnit)
        {
            _heroUnitInstanceDatabase.Add(newUnit, null);
            AssignReferenceDataToHeroUnitInstance(newUnit);
            BuffManager.ApplyAttributePassives(_heroUnitInstanceDatabase[newUnit]);
        }

        /// <summary>
        /// Use to load existing hero unit
        /// </summary>
        /// <param name="newUnit"></param>
        /// <param name="unitData"></param>
        public static void AddHeroUnit(unit newUnit, UnitCombatData unitData)
        {
            _heroUnitInstanceDatabase.Add(newUnit, new UnitInstance(newUnit, unitData));
            UpdateUnitInGameData(_heroUnitInstanceDatabase[newUnit]);
            _heroUnitInstanceDatabase[newUnit].UnitModified += UnitInstance_UnitModified;
            BuffManager.ApplyAttributePassives(_heroUnitInstanceDatabase[newUnit]);
        }

        public static void RemoveUnit(unit oldUnit)
        {
            if (oldUnit != null)
            {
                if (_unitInstanceDatabase.TryGetValue(oldUnit, out UnitInstance tmpUnitInstance))
                {
                    _unitInstanceDatabase.Remove(oldUnit);
                }
            }
        }

        public static void RemoveHeroUnit(unit oldUnit)
        {
            Console.WriteLine("Attempting to remove old hero unit");
            if (oldUnit != null)
            {
                if (_heroUnitInstanceDatabase.TryGetValue(oldUnit, out UnitInstance tmpUnitInstance))
                {
                    _heroUnitInstanceDatabase.Remove(oldUnit);
                }
            }
        }

        private static void AssignReferenceDataToUnitInstance(unit newUnit)
        {
            Console.WriteLine("Assigning reference data to unit instance!");
            int unitTypeId = GetUnitTypeId(newUnit);
            if (_unitReferenceDatabase.TryGetValue(unitTypeId, out UnitCombatData tmpData))
            {
                _unitInstanceDatabase[newUnit] = new UnitInstance(newUnit, new UnitCombatData(_unitReferenceDatabase[unitTypeId]));
                UpdateUnitInGameData(_unitInstanceDatabase[newUnit]);
                _unitInstanceDatabase[newUnit].UnitModified += UnitInstance_UnitModified;
            }
        }

        private static void AssignReferenceDataToHeroUnitInstance(unit newUnit)
        {
            Console.WriteLine("Assigning reference data to hero unit instance!");
            int unitTypeId = GetUnitTypeId(newUnit);
            if (_heroUnitReferenceDatabase.TryGetValue(unitTypeId, out UnitCombatData tmpData))
            {
                _heroUnitInstanceDatabase[newUnit] = new UnitInstance(newUnit, new UnitCombatData(_heroUnitReferenceDatabase[unitTypeId]));
                UpdateUnitInGameData(_heroUnitInstanceDatabase[newUnit]);
                _heroUnitInstanceDatabase[newUnit].UnitData.BaseHealthRegeneration = 5.0f;
                _heroUnitInstanceDatabase[newUnit].UnitModified += UnitInstance_UnitModified;
            }
        }

        private static void AddNeutralUnitToDatabase()
        {
            Console.WriteLine("Adding single unit to instance database!");
            unit enumUnit = GetEnumUnit();
            Console.WriteLine("Unit: " + GetUnitName(enumUnit) + ", ID: " + GetUnitTypeId(enumUnit));
            AddUnit(enumUnit);
        }

        private static void AddAllNeutralUnitsToDatabase()
        {
            Console.WriteLine("Adding all units to instance database!");
            group neutralUnits = GetUnitsInRectOfPlayer(GetPlayableMapRect(), Player(PLAYER_NEUTRAL_AGGRESSIVE));
            ForGroup(neutralUnits, AddNeutralUnitToDatabase);
        }

        public static UnitInstance GetUnitInstance(unit targetUnit)
        {
            UnitInstance tmpUnitInstance = null;
            if (_unitInstanceDatabase.TryGetValue(targetUnit, out tmpUnitInstance))
            {
                return tmpUnitInstance;
            }
            if (_heroUnitInstanceDatabase.TryGetValue(targetUnit, out tmpUnitInstance))
            {
                return tmpUnitInstance;
            }
            return null;
        }

        public static List<UnitInstance> GetUnitInstancesOfType(int unitTypeId)
        {
            List<UnitInstance> units = new List<UnitInstance>();
            for (int i = 0; i < _unitInstanceDatabase.Count; i++)
            {
                if (GetUnitTypeId(_unitInstanceDatabase.ElementAt(i).Key) == unitTypeId)
                {
                    units.Add(_unitInstanceDatabase.ElementAt(i).Value);
                }
                 
            }
            for (int i = 0; i < _heroUnitInstanceDatabase.Count; i++)
            {
                if (GetUnitTypeId(_heroUnitInstanceDatabase.ElementAt(i).Key) == unitTypeId)
                {
                    units.Add(_heroUnitInstanceDatabase.ElementAt(i).Value);
                }
            }
            if (units.Count > 0)
            {
                return units;
            }
            return null;
        }

        public static bool IsHeroUnit(int unitTypeId)
        {
            return _heroUnitReferenceDatabase.ContainsKey(unitTypeId);
        }

        /// <summary>
        /// Used to respond to newly created unit
        /// </summary>
        /// <param name="unitInstanceData"></param>
        private static void UpdateUnitInGameData(UnitInstance unitInstanceData)
        {
            SetHeroXP(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.Experience, true);
            BlzSetUnitMaxHP(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalMaximumHealth);
            SetUnitLifeBJ(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalMaximumHealth);
            BlzSetUnitMaxMana(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalMaximumMana);
            SetUnitManaBJ(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalMaximumMana);
            BlzSetUnitAttackCooldown(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalAttackCooldown, 0);
            BlzSetUnitBaseDamage(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalPhysicalAttackDamage - 1, 0);
            BlzSetUnitArmor(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalPhysicalDamageReduction);
            SetHeroAgi(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.BaseAgility, true);
            SetHeroInt(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.BaseIntelligence, true);
            SetHeroStr(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.BaseStrength, true);
            if (unitInstanceData.UnitData.AbilityDatas != null)
            {
                for (int i = 0; i < unitInstanceData.UnitData.AbilityDatas.Count; i++)
                {
                    Console.WriteLine("Adding ability " + i + " with an id of " + unitInstanceData.UnitData.AbilityDatas[i].AbilityId + " and a level of " + unitInstanceData.UnitData.AbilityDatas[i].AbilityLevel + ".");
                    UnitAddAbility(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.AbilityDatas[i].AbilityId);
                    SetUnitAbilityLevel(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.AbilityDatas[i].AbilityId, unitInstanceData.UnitData.AbilityDatas[i].AbilityLevel);
                }
                int currentSkillPoints = GetHeroSkillPoints(unitInstanceData.LinkedUnit);
                UnitModifySkillPoints(unitInstanceData.LinkedUnit, -(currentSkillPoints - unitInstanceData.UnitData.SkillPoints));
            }
        }

        /// <summary>
        /// Used to respond to stat change event
        /// </summary>
        /// <param name="unitInstanceData"></param>
        /// <param name="stat"></param>
        private static void UpdateUnitInGameData(UnitInstance unitInstanceData, UnitCombatStat stat)
        {
            switch (stat)
            {
                case UnitCombatStat.SKILL_POINTS:
                    break;
                case UnitCombatStat.LEVEL:
                    BuffManager.ApplyAttributePassives(unitInstanceData);
                    break;
                case UnitCombatStat.EXPERIENCE:
                    SetHeroXP(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.Experience, true);
                    break;
                case UnitCombatStat.CURRENT_HEALTH:
                    SetUnitLifeBJ(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.CurrentHealth);
                    break;
                case UnitCombatStat.MAXIMUM_HEALTH:
                    BlzSetUnitMaxHP(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalMaximumHealth);
                    break;
                case UnitCombatStat.CURRENT_MANA:
                    SetUnitManaBJ(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.CurrentMana);
                    break;
                case UnitCombatStat.MAXIMUM_MANA:
                    BlzSetUnitMaxMana(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalMaximumMana);
                    break;
                case UnitCombatStat.HEALTH_REGENERATION:
                    break;
                case UnitCombatStat.MANA_REGENERATION:
                    break;
                case UnitCombatStat.ATTACK_SPEED:
                    BlzSetUnitAttackCooldown(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalAttackCooldown, 0);
                    break;
                case UnitCombatStat.PHYSICAL_ATTACK_DAMAGE:
                    //BlzSetUnitBaseDamage(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalPhysicalAttackDamage - 1, 0);
                    break;
                case UnitCombatStat.MAGICAL_ATTACK_DAMAGE:
                    //BlzSetUnitBaseDamage(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalMagicalAttackDamage - 1, 0);
                    break;
                case UnitCombatStat.FLAT_PHYSICAL_PENETRATION:
                    break;
                case UnitCombatStat.PERCENT_PHYSICAL_PENETRATION:
                    break;
                case UnitCombatStat.FLAT_MAGICAL_PENETRATION:
                    break;
                case UnitCombatStat.PERCENT_MAGICAL_PENETRATION:
                    break;
                case UnitCombatStat.FLAT_PHYSICAL_DAMAGE_REDUCTION:
                    //BlzSetUnitArmor(unitInstanceData.LinkedUnit, unitInstanceData.UnitData.TotalPhysicalDamageReduction);
                    break;
                case UnitCombatStat.PERCENT_PHYSICAL_DAMAGE_REDUCTION:
                    break;
                case UnitCombatStat.FLAT_MAGICAL_DAMAGE_REDUCTION:
                    break;
                case UnitCombatStat.PERCENT_MAGICAL_DAMAGE_REDUCTION:
                    break;
                case UnitCombatStat.SKILL_DAMAGE:
                    break;
                case UnitCombatStat.ABILITY_DAMAGE:
                    break;
                case UnitCombatStat.SPELL_DAMAGE:
                    break;
                case UnitCombatStat.CRITICAL_CHANCE:
                    break;
                case UnitCombatStat.CRITICAL_DAMAGE:
                    break;
                case UnitCombatStat.AGILITY:
                    BuffManager.ApplyAgilityAttributePassive(unitInstanceData);
                    break;
                case UnitCombatStat.INTELLIGENCE:
                    BuffManager.ApplyIntelligenceAttributePassive(unitInstanceData);
                    break;
                case UnitCombatStat.STRENGTH:
                    BuffManager.ApplyStrengthAttributePassive(unitInstanceData);
                    break;
                default:
                    break;
            }
        }

        private static void UnitInstance_UnitModified(object s, StatChangeEventArgs e)
        {
            //Console.WriteLine("Stat changed detected in UnitManager: " + e.CombatStat + "!");
            //Console.WriteLine("Updating unit data from event!");
            UpdateUnitInGameData((UnitInstance)s, e.CombatStat);
        }

        private static void InitializeUnitReferenceDatabase()
        {
            _unitReferenceDatabase = new Dictionary<int, UnitCombatData>();
            _unitReferenceDatabase.Add(Constants.UNIT_TRAINING_DUMMY, new UnitCombatData(Constants.UNIT_TRAINING_DUMMY, 0, 0, int.MaxValue, 0, 0.0f, 0.0f, 0.0f, 0, 0, 0, 0, 0.0f, 0.0f, 0, 0));
            _unitReferenceDatabase.Add(Constants.UNIT_SPOODER, new UnitCombatData(Constants.UNIT_SPOODER, 1, 100, 50, 20, 1.0f, 0.0f, 1.0f, 5, 1, 0, 0, 0.0f, 0.0f, 0, 0));
            _unitReferenceDatabase.Add(Constants.UNIT_SPOODER_KING, new UnitCombatData(Constants.UNIT_SPOODER_KING, 5, 1000, 150, 50, 2.0f, 0.0f, 1.5f, 25, 5, 0, 0, 0.0f, 0.0f, 5, 0));
            _unitReferenceDatabase.Add(Constants.UNIT_BOAR_RUNT, new UnitCombatData(Constants.UNIT_BOAR_RUNT, 1, 10, 10, 5, 0.0f, 0.0f, 1.0f, 2, 0, 0, 0, 0.0f, 0.0f, 0, 0));
            _unitReferenceDatabase.Add(Constants.UNIT_BOAR, new UnitCombatData(Constants.UNIT_BOAR, 2, 20, 15, 8, 0.0f, 0.0f, 1.0f, 3, 0, 0, 0, 0.0f, 0.0f, 2, 1));
            _unitReferenceDatabase.Add(Constants.UNIT_BOAR_ALPHA, new UnitCombatData(Constants.UNIT_BOAR_ALPHA, 3, 30, 20, 10, 0.0f, 0.0f, 1.0f, 5, 0, 0, 0, 0.0f, 0.0f, 3, 2));
            _unitReferenceDatabase.Add(Constants.UNIT_HOGZILLA, new UnitCombatData(Constants.UNIT_HOGZILLA, 7, 100, 100, 20, 0.0f, 0.0f, 1.0f, 8, 0, 0, 0, 0.0f, 0.0f, 5, 3));
            _unitReferenceDatabase.Add(Constants.UNIT_BANDIT_THUG, new UnitCombatData(Constants.UNIT_BANDIT_THUG, 2, 20, 20, 10, 0.0f, 0.0f, 1.0f, 4, 0, 0, 0, 0.0f, 0.0f, 2, 0));
            _unitReferenceDatabase.Add(Constants.UNIT_BANDIT_SCOUT, new UnitCombatData(Constants.UNIT_BANDIT_SCOUT, 3, 30, 20, 10, 0.0f, 0.0f, 1.0f, 6, 0, 0, 0, 0.0f, 0.0f, 1, 0));
            _unitReferenceDatabase.Add(Constants.UNIT_BANDIT_LIEUTENANT, new UnitCombatData(Constants.UNIT_BANDIT_LIEUTENANT, 5, 50, 35, 15, 0.0f, 0.0f, 1.0f, 8, 0, 0, 0, 0.0f, 0.0f, 3, 0));
            _unitReferenceDatabase.Add(Constants.UNIT_BANDIT_CAPTAIN, new UnitCombatData(Constants.UNIT_BANDIT_CAPTAIN, 7, 70, 50, 25, 0.0f, 0.0f, 1.0f, 10, 0, 0, 0, 0.0f, 0.0f, 5, 0));
        }

        private static void InitializeUnitInstanceDatabase()
        {
            _unitInstanceDatabase = new Dictionary<unit, UnitInstance>();
            AddAllNeutralUnitsToDatabase();
        }

        private static void InitializeHeroUnitReferenceDatabase()
        {
            _heroUnitReferenceDatabase = new Dictionary<int, UnitCombatData>();
            _heroUnitReferenceDatabase.Add(Constants.UNIT_SOUL_OF_HERO, new UnitCombatData(Constants.UNIT_SOUL_OF_HERO, 1, 0, 50, 20, 0.0f, 0.0f, 1.0f, 0, 0, 0, 0, 0, 0, 0.0f, 0.0f, 0, UnitAttribute.NO_ATTRIBUTE, 0, 0, 0));
            _heroUnitReferenceDatabase.Add(Constants.UNIT_ARCHER, new UnitCombatData(Constants.UNIT_ARCHER, 1, 0, 75, 30, 0.0f, 0.0f, 1.0f, 8, 0, 0, 0, 0, 0, 0.0f, 0.0f, 1, UnitAttribute.AGILITY, 20, 1, 1));
            _heroUnitReferenceDatabase.Add(Constants.UNIT_MAGE, new UnitCombatData(Constants.UNIT_MAGE, 1, 0, 50, 1000, 0.0f, 0.0f, 1.0f, 5, 5, 0, 0, 0, 0, 0.5f, 0.5f, 1, UnitAttribute.INTELLIGENCE, 1, 2, 1));
            _heroUnitReferenceDatabase.Add(Constants.UNIT_WARRIOR, new UnitCombatData(Constants.UNIT_WARRIOR, 1, 0, 100, 20, 0.0f, 0.0f, 1.0f, 10, 0, 0, 0, 0, 0, 0.0f, 0.0f, 1, UnitAttribute.STRENGTH, 1, 1, 2));
        }

        private static void InitializeHeroUnitInstanceDatabase()
        {
            _heroUnitInstanceDatabase = new Dictionary<unit, UnitInstance>();
        }
    }
}