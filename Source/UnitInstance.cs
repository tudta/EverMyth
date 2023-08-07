using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using WCSharp.Buffs;

namespace Source
{
    public class UnitInstance
    {
        public delegate void UnitModifiedEventHandler(object sender, StatChangeEventArgs e);
        public event UnitModifiedEventHandler UnitModified;

        private unit _linkedUnit = null;
        private UnitCombatData _unitData = null;
        private int _lastKnownLevel = 0;

        public unit LinkedUnit
        {
            get
            {
                return _linkedUnit;
            }

            set
            {
                _linkedUnit = value;
            }
        }

        public UnitCombatData UnitData
        {
            get
            {
                return _unitData;
            }

            set
            {
                _unitData = value;
            }
        }

        public UnitInstance(unit unitRef, UnitCombatData unitDataRef)
        {
            _linkedUnit = unitRef;
            _unitData = unitDataRef;
            Init();
        }

        private void Init()
        {
            SubscribeToStatChange();
            CreateLevelTrigger();
            CreateAbilityTrigger();
            InitializeUnitLevel();
        }

        private void SubscribeToStatChange()
        {
            _unitData.StatChanged += UnitCombatData_OnStatChanged;
        }

        private void CreateLevelTrigger()
        {
            trigger levelTrigger = CreateTrigger();
            TriggerRegisterUnitEvent(levelTrigger, _linkedUnit, EVENT_UNIT_HERO_LEVEL);
            TriggerAddAction(levelTrigger, UpdateUnitLevel);
        }

        private void CreateAbilityTrigger()
        {
            if (IsHeroUnitId(_unitData.UnitTypeId))
            {
                if (_unitData.AbilityDatas == null)
                {
                    _unitData.AbilityDatas = new List<AbilityData>();
                }
                trigger abilityTrigger = CreateTrigger();
                TriggerRegisterPlayerUnitEvent(abilityTrigger, GetOwningPlayer(_linkedUnit), EVENT_PLAYER_HERO_SKILL, Filter(BypassFilter));
                TriggerAddAction(abilityTrigger, UpdateAbilityDictionary);
            }
        }

        private bool BypassFilter()
        {
            return true;
        }

        private void UpdateAbilityDictionary()
        {
            _unitData.SkillPoints -= 1;
            int abilityId = GetLearnedSkill();
            int abilityLevel = GetLearnedSkillLevel();
            List<AbilityData> abilityList = _unitData.AbilityDatas;
            AbilityData tmpAbilityData = new AbilityData { AbilityId = abilityId, AbilityLevel = abilityLevel };
            AbilityData existingAbilityData = abilityList.Find(data => data.AbilityId == abilityId);
            if (existingAbilityData == null)
            {
                abilityList.Add(tmpAbilityData);
                PrintAbilityList(abilityList);
                return;
            }
            existingAbilityData.AbilityLevel = abilityLevel;
            //Console.WriteLine("Player id of triggering unit: " + playerId);
            //Console.WriteLine("Learned skill id is: " + abilityId);
            //Console.WriteLine("Learned skill level is: " + abilityLevel);
            PrintAbilityList(abilityList);
        }

        public static void PrintAbilityList(List<AbilityData> abilityList)
        {
            Console.WriteLine("Printing ability list!");
            for (int i = 0; i < abilityList.Count; i++)
            {
                Console.WriteLine("Ability " + i + " has an id of " + abilityList[i].AbilityId + " and a level of " + abilityList[i].AbilityLevel + ".");
            }
        }

        private void InitializeUnitLevel()
        {
            _lastKnownLevel = _unitData.Level;
        }

        private void UpdateUnitLevel()
        {
            _unitData.Level = GetUnitLevel(_linkedUnit);
            _unitData.SkillPoints += _unitData.Level - _lastKnownLevel;
            _lastKnownLevel = _unitData.Level;
            // Call level up functions if any are needed
            UpdateUnitAttributes();
        }

        private void UpdateUnitAttributes()
        {
            Console.WriteLine("Updating unit attributes!");
            _unitData.BaseAgility = GetHeroAgi(LinkedUnit, false);
            _unitData.BaseIntelligence = GetHeroInt(LinkedUnit, false);
            _unitData.BaseStrength = GetHeroStr(LinkedUnit, false);
        }

        public void UnitCombatData_OnStatChanged(object s, StatChangeEventArgs e)
        {
            if (UnitModified != null)
            {
                UnitModified(this, e);
            }
        }
    }
}