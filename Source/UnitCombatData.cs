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
    public class UnitCombatData
    {
        public delegate void StatChangedEventHandler(object sender, StatChangeEventArgs e);
        public event StatChangedEventHandler StatChanged;

        private int _unitTypeId = 0;

        private int _level = 0;
        private int _experience = 0;
        private float _experienceGainModifier = 0;
        private int _experienceAwarded = 0;

        private int _currentHealth = 0;
        private int _baseMaximumHealth = 0;
        private float _bonusFlatHealth = 0;
        private float _bonusPercentHealth = 0.0f;
        private int _totalMaximumHealth = 0;

        private float _baseHealthRegeneration = 0.0f;
        private float _bonusFlatHealthRegeneration = 0.0f;
        private float _bonusPercentHealthRegeneration = 0.0f;
        private float _totalHealthRegeneration = 0.0f;
        private float _percentMaximumHealthRegeneration = 0.0f;
        private float _healingReceivedBonus = 0.0f;

        private int _currentMana = 0;
        private int _baseMaximumMana = 0;
        private float _bonusFlatMana = 0;
        private float _bonusPercentMana = 0.0f;
        private int _totalMaximumMana = 0;

        private float _baseManaRegeneration = 0.0f;
        private float _bonusFlatManaRegeneration = 0.0f;
        private float _bonusPercentManaRegeneration = 0.0f;
        private float _totalManaRegeneration = 0.0f;
        private float _percentMaximumManaRegeneration = 0.0f;

        private UnitAttribute _primaryAttribute;

        private int _baseAgility = 0;
        private float _bonusFlatAgility = 0;
        private float _bonusPercentAgility = 0.0f;
        private int _totalAgility = 0;

        private int _baseIntelligence = 0;
        private float _bonusFlatIntelligence = 0;
        private float _bonusPercentIntelligence = 0.0f;
        private int _totalIntelligence = 0;

        private int _baseStrength = 0;
        private float _bonusFlatStrength = 0;
        private float _bonusPercentStrength = 0.0f;
        private int _totalStrength = 0;

        private float _baseAttackCooldown = 0.0f;
        private float _bonusAttackCooldown = 0.0f;
        private float _totalAttackCooldown = 0.0f;

        private int _basePhysicalAttackDamage = 0;
        private float _bonusFlatPhysicalAttackDamage = 0;
        private float _bonusPercentPhysicalAttackDamage = 0.0f;
        private int _totalPhysicalAttackDamage = 0;

        private int _baseMagicalAttackDamage = 0;
        private float _bonusFlatMagicalAttackDamage = 0;
        private float _bonusPercentMagicalAttackDamage = 0.0f;
        private int _totalMagicalAttackDamage = 0;

        private int _basePhysicalPenetration = 0;
        private float _bonusFlatPhysicalPenetration = 0.0f;
        private int _totalPhysicalPenetration = 0;
        private float _absolutePercentPhysicalPenetration = 0.0f;

        private int _baseMagicalPenetration = 0;
        private float _bonusFlatMagicalPenetration = 0.0f;
        private int _totalMagicalPenetration = 0;
        private float _absolutePercentMagicalPenetration = 0.0f;

        private float _bonusPercentSkillDamage = 0.0f;
        private float _bonusPercentAbilityDamage = 0.0f;
        private float _bonusPercentSpellDamage = 0.0f;

        private float _baseCriticalChance = 0.0f;
        private float _bonusCriticalChance = 0.0f;
        private float _totalCriticalChance = 0.0f;
        private float _baseCriticalDamage = 0.0f;
        private float _bonusCriticalDamage = 0.0f;
        private float _totalCriticalDamage = 0.0f;

        private int _basePhysicalDamageReduction = 0;
        private float _bonusFlatPhysicalDamageReduction = 0;
        private float _bonusPercentPhysicalDamageReduction = 0.0f;
        private int _totalPhysicalDamageReduction = 0;
        private float _absolutePhysicalDamageReductionPercent = 0.0f;

        private int _baseMagicalDamageReduction = 0;
        private float _bonusFlatMagicalDamageReduction = 0;
        private float _bonusPercentMagicalDamageReduction = 0.0f;
        private int _totalMagicalDamageReduction = 0;
        private float _absoluteMagicalDamageReductionPercent = 0.0f;

        private int _skillPoints = 0;
        private List<AbilityData> _abilityDatas;

        public int UnitTypeId
        {
            get
            {
                return _unitTypeId;
            }

            set
            {
                _unitTypeId = value;
            }
        }

        public int Level
        {
            get
            {
                return _level;
            }

            set
            {
                _level = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.LEVEL));
            }
        }

        public int Experience
        {
            get
            {
                return _experience;
            }

            set
            {
                _experience = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.EXPERIENCE));
            }
        }

        public float ExperienceGainModifier
        {
            get
            {
                return _experienceGainModifier;
            }

            set
            {
                _experienceGainModifier = value;
            }
        }

        public int ExperienceAwarded
        {
            get
            {
                return _experienceAwarded;
            }

            set
            {
                _experienceAwarded = value;
            }
        }

        public int CurrentHealth
        {
            get
            {
                return _currentHealth;
            }

            set
            {
                _currentHealth = value;
                ClampCurrentHealth();
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.CURRENT_HEALTH));
            }
        }

        public int BaseMaximumHealth
        {
            get
            {
                return _baseMaximumHealth;
            }

            set
            {
                _baseMaximumHealth = value;
                CalculateHealthTotal();
            }
        }

        public float BonusFlatHealth
        {
            get
            {
                return _bonusFlatHealth;
            }

            set
            {
                _bonusFlatHealth = value;
                CalculateHealthTotal();
            }
        }

        public float BonusPercentHealth
        {
            get
            {
                return _bonusPercentHealth;
            }

            set
            {
                _bonusPercentHealth = value;
                CalculateHealthTotal();
            }
        }

        public int TotalMaximumHealth
        {
            get
            {
                return _totalMaximumHealth;
            }
        }

        public float BaseHealthRegeneration
        {
            get
            {
                return _baseHealthRegeneration;
            }

            set
            {
                _baseHealthRegeneration = value;
                CalculateHealthRegenerationTotal();
            }
        }

        public float BonusFlatHealthRegeneration
        {
            get
            {
                return _bonusFlatHealthRegeneration;
            }

            set
            {
                _bonusFlatHealthRegeneration = value;
                CalculateHealthRegenerationTotal();
            }
        }

        public float BonusPercentHealthRegeneration
        {
            get
            {
                return _bonusPercentHealthRegeneration;
            }

            set
            {
                _bonusPercentHealthRegeneration = value;
                CalculateHealthRegenerationTotal();
            }
        }

        public float TotalHealthRegeneration
        {
            get
            {
                return _totalHealthRegeneration;
            }
        }

        public float PercentMaximumHealthRegeneration
        {
            get
            {
                return _percentMaximumHealthRegeneration;
            }

            set
            {
                _percentMaximumHealthRegeneration = value;
            }
        }

        public float HealingReceivedBonus
        {
            get
            {
                return _healingReceivedBonus;
            }

            set
            {
                _healingReceivedBonus = value;
            }
        }

        public int CurrentMana
        {
            get
            {
                return _currentMana;
            }

            set
            {
                _currentMana = value;
                ClampCurrentMana();
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.CURRENT_MANA));
            }
        }

        public int BaseMaximumMana
        {
            get
            {
                return _baseMaximumMana;
            }

            set
            {
                _baseMaximumMana = value;
                CalculateManaTotal();
            }
        }

        public float BonusFlatMana
        {
            get
            {
                return _bonusFlatMana;
            }

            set
            {
                _bonusFlatMana = value;
                CalculateManaTotal();
            }
        }

        public float BonusPercentMana
        {
            get
            {
                return _bonusPercentMana;
            }

            set
            {
                _bonusPercentMana = value;
                CalculateManaTotal();
            }
        }

        public int TotalMaximumMana
        {
            get
            {
                return _totalMaximumMana;
            }
        }

        public float BaseManaRegeneration
        {
            get
            {
                return _baseManaRegeneration;
            }

            set
            {
                _baseManaRegeneration = value;
                CalculateManaRegenerationTotal();
            }
        }

        public float BonusFlatManaRegeneration
        {
            get
            {
                return _bonusFlatManaRegeneration;
            }

            set
            {
                _bonusFlatManaRegeneration = value;
                CalculateManaRegenerationTotal();
            }
        }

        public float BonusPercentManaRegeneration
        {
            get
            {
                return _bonusPercentManaRegeneration;
            }

            set
            {
                _bonusPercentManaRegeneration = value;
                CalculateManaRegenerationTotal();
            }
        }

        public float TotalManaRegeneration
        {
            get
            {
                return _totalManaRegeneration;
            }
        }

        public float PercentMaximumManaRegeneration
        {
            get
            {
                return _percentMaximumManaRegeneration;
            }

            set
            {
                _percentMaximumManaRegeneration = value;
            }
        }

        public UnitAttribute PrimaryAttribute
        {
            get
            {
                return _primaryAttribute;
            }

            set
            {
                _primaryAttribute = value;
            }
        }

        public int BaseAgility
        {
            get
            {
                return _baseAgility;
            }

            set
            {
                _baseAgility = value;
                CalculateAgilityTotal();
            }
        }

        public float BonusFlatAgility
        {
            get
            {
                return _bonusFlatAgility;
            }

            set
            {
                _bonusFlatAgility = value;
                if (value != _bonusFlatAgility)
                {
                    CalculateAgilityTotal();
                }
            }
        }

        public float BonusPercentAgility
        {
            get
            {
                return _bonusPercentAgility;
            }

            set
            {
                _bonusPercentAgility = value;
                if (value != _bonusPercentAgility)
                {
                    CalculateAgilityTotal();
                }
            }
        }

        public int TotalAgility
        {
            get
            {
                return _totalAgility;
            }
        }

        public int BaseIntelligence
        {
            get
            {
                return _baseIntelligence;
            }

            set
            {
                _baseIntelligence = value;
                CalculateIntelligenceTotal();
            }
        }

        public float BonusFlatIntelligence
        {
            get
            {
                return _bonusFlatIntelligence;
            }

            set
            {
                _bonusFlatIntelligence = value;
                if (value != _bonusFlatIntelligence)
                {
                    CalculateIntelligenceTotal();
                }
            }
        }

        public float BonusPercentIntelligence
        {
            get
            {
                return _bonusPercentIntelligence;
            }

            set
            {
                _bonusPercentIntelligence = value;
                if (value != _bonusPercentIntelligence)
                {
                    CalculateIntelligenceTotal();
                }
            }
        }

        public int TotalIntelligence
        {
            get
            {
                return _totalIntelligence;
            }
        }

        public int BaseStrength
        {
            get
            {
                return _baseStrength;
            }

            set
            {
                _baseStrength = value;
                CalculateStrengthTotal();
            }
        }

        public float BonusFlatStrength
        {
            get
            {
                return _bonusFlatStrength;
            }

            set
            {
                _bonusFlatStrength = value;
                if (value != _bonusFlatStrength)
                {
                    CalculateStrengthTotal();
                }
            }
        }

        public float BonusPercentStrength
        {
            get
            {
                return _bonusPercentStrength;
            }

            set
            {
                _bonusPercentStrength = value;
                if (value != _bonusPercentStrength)
                {
                    CalculateStrengthTotal();
                }
            }
        }

        public int TotalStrength
        {
            get
            {
                return _totalStrength;
            }
        }

        public float BaseAttackCooldown
        {
            get
            {
                return _baseAttackCooldown;
            }

            set
            {
                _baseAttackCooldown = value;
                CalculateAttackCooldownTotal();
            }
        }

        public float BonusAttackCooldown
        {
            get
            {
                return _bonusAttackCooldown;
            }

            set
            {
                _bonusAttackCooldown = value;
                CalculateAttackCooldownTotal();
            }
        }

        public float TotalAttackCooldown
        {
            get
            {
                return _totalAttackCooldown;
            }
        }

        public int BasePhysicalAttackDamage
        {
            get
            {
                return _basePhysicalAttackDamage;
            }

            set
            {
                _basePhysicalAttackDamage = value;
                CalculatePhysicalAttackDamageTotal();
            }
        }

        public float BonusFlatPhysicalAttackDamage
        {
            get
            {
                return _bonusFlatPhysicalAttackDamage;
            }

            set
            {
                _bonusFlatPhysicalAttackDamage = value;
                CalculatePhysicalAttackDamageTotal();
            }
        }

        public float BonusPercentPhysicalAttackDamage
        {
            get
            {
                return _bonusPercentPhysicalAttackDamage;
            }

            set
            {
                _bonusPercentPhysicalAttackDamage = value;
                CalculatePhysicalAttackDamageTotal();
            }
        }

        public int TotalPhysicalAttackDamage
        {
            get
            {
                return _totalPhysicalAttackDamage;
            }
        }

        public int BaseMagicalAttackDamage
        {
            get
            {
                return _baseMagicalAttackDamage;
            }

            set
            {
                _baseMagicalAttackDamage = value;
                CalculateMagicalAttackDamageTotal();
            }
        }

        public float BonusFlatMagicalAttackDamage
        {
            get
            {
                return _bonusFlatMagicalAttackDamage;
            }

            set
            {
                _bonusFlatMagicalAttackDamage = value;
                CalculateMagicalAttackDamageTotal();
            }
        }

        public float BonusPercentMagicalAttackDamage
        {
            get
            {
                return _bonusPercentMagicalAttackDamage;
            }

            set
            {
                _bonusPercentMagicalAttackDamage = value;
                CalculateMagicalAttackDamageTotal();
            }
        }

        public int TotalMagicalAttackDamage
        {
            get
            {
                return _totalMagicalAttackDamage;
            }
        }

        public int BasePhysicalPenetration
        {
            get
            {
                return _basePhysicalPenetration;
            }

            set
            {
                _basePhysicalPenetration = value;
                CalculateTotalPhysicalPenetration();
            }
        }

        public float BonusFlatPhysicalPenetration
        {
            get
            {
                return _bonusFlatPhysicalPenetration;
            }

            set
            {
                _bonusFlatPhysicalPenetration = value;
                CalculateTotalPhysicalPenetration();
            }
        }

        public int TotalPhysicalPenetration
        {
            get
            {
                return _totalPhysicalPenetration;
            }
        }

        public float AbsolutePercentPhysicalPenetration
        {
            get
            {
                return _absolutePercentPhysicalPenetration;
            }

            set
            {
                _absolutePercentPhysicalPenetration = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.PERCENT_PHYSICAL_PENETRATION));
            }
        }

        public int BaseMagicalPenetration
        {
            get
            {
                return _baseMagicalPenetration;
            }

            set
            {
                _baseMagicalPenetration = value;
                CalculateTotalMagicalPeneration();
            }
        }

        public float BonusFlatMagicalPenetration
        {
            get
            {
                return _bonusFlatMagicalPenetration;
            }

            set
            {
                _bonusFlatMagicalPenetration = value;
                CalculateTotalMagicalPeneration();
            }
        }

        public int TotalMagicalPenetration
        {
            get
            {
                return _totalMagicalPenetration;
            }
        }

        public float AbsolutePercentMagicalPenetration
        {
            get
            {
                return _absolutePercentMagicalPenetration;
            }

            set
            {
                _absolutePercentMagicalPenetration = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.PERCENT_MAGICAL_PENETRATION));
            }
        }

        public float BonusPercentSkillDamage
        {
            get
            {
                return _bonusPercentSkillDamage;
            }

            set
            {
                _bonusPercentSkillDamage = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.SKILL_DAMAGE));
            }
        }

        public float BonusPercentAbilityDamage
        {
            get
            {
                return _bonusPercentAbilityDamage;
            }

            set
            {
                _bonusPercentAbilityDamage = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.ABILITY_DAMAGE));
            }
        }

        public float BonusPercentSpellDamage
        {
            get
            {
                return _bonusPercentSpellDamage;
            }

            set
            {
                _bonusPercentSpellDamage = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.SPELL_DAMAGE));
            }
        }

        public float BaseCriticalChance
        {
            get
            {
                return _baseCriticalChance;
            }

            set
            {
                _baseCriticalChance = value;
                CalculateTotalCriticalChance();
            }
        }

        public float BonusCriticalChance
        {
            get
            {
                return _bonusCriticalChance;
            }

            set
            {
                _bonusCriticalChance = value;
                CalculateTotalCriticalChance();
            }
        }

        public float TotalCriticalChance
        {
            get
            {
                return _totalCriticalChance;
            }
        }

        public float BaseCriticalDamage
        {
            get
            {
                return _baseCriticalDamage;
            }

            set
            {
                _baseCriticalDamage = value;
                CalculateTotalCriticalDamage();
            }
        }

        public float BonusCriticalDamage
        {
            get
            {
                return _bonusCriticalDamage;
            }

            set
            {
                _bonusCriticalDamage = value;
                CalculateTotalCriticalDamage();
            }
        }

        public float TotalCriticalDamage
        {
            get
            {
                return _totalCriticalDamage;
            }
        }

        public int BasePhysicalDamageReduction
        {
            get
            {
                return _basePhysicalDamageReduction;
            }

            set
            {
                _basePhysicalDamageReduction = value;
                CalculatePhysicalDamageReductionTotal();
            }
        }

        public float BonusFlatPhysicalDamageReduction
        {
            get
            {
                return _bonusFlatPhysicalDamageReduction;
            }

            set
            {
                _bonusFlatPhysicalDamageReduction = value;
                CalculatePhysicalDamageReductionTotal();
            }
        }

        public float BonusPercentPhysicalDamageReduction
        {
            get
            {
                return _bonusPercentPhysicalDamageReduction;
            }

            set
            {
                _bonusPercentPhysicalDamageReduction = value;
                CalculatePhysicalDamageReductionTotal();
            }
        }

        public int TotalPhysicalDamageReduction
        {
            get
            {
                return _totalPhysicalDamageReduction;
            }
        }

        public float AbsolutePhysicalDamageReductionPercent
        {
            get
            {
                return _absolutePhysicalDamageReductionPercent;
            }

            set
            {
                _absolutePhysicalDamageReductionPercent = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.PERCENT_PHYSICAL_DAMAGE_REDUCTION));
            }
        }

        public int BaseMagicalDamageReduction
        {
            get
            {
                return _baseMagicalDamageReduction;
            }

            set
            {
                _baseMagicalDamageReduction = value;
                CalculateMagicalDamageReductionTotal();
            }
        }

        public float BonusFlatMagicalDamageReduction
        {
            get
            {
                return _bonusFlatMagicalDamageReduction;
            }

            set
            {
                _bonusFlatMagicalDamageReduction = value;
                CalculateMagicalDamageReductionTotal();
            }
        }

        public float BonusPercentMagicalDamageReduction
        {
            get
            {
                return _bonusPercentMagicalDamageReduction;
            }

            set
            {
                _bonusPercentMagicalDamageReduction = value;
                CalculateMagicalDamageReductionTotal();
            }
        }

        public int TotalMagicalDamageReduction
        {
            get
            {
                return _totalMagicalDamageReduction;
            }
        }

        public float AbsoluteMagicalDamageReductionPercent
        {
            get
            {
                return _absoluteMagicalDamageReductionPercent;
            }

            set
            {
                _absoluteMagicalDamageReductionPercent = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.PERCENT_MAGICAL_DAMAGE_REDUCTION));
            }
        }

        public int SkillPoints
        {
            get
            {
                return _skillPoints;
            }

            set
            {
                _skillPoints = value;
                OnStatChanged(new StatChangeEventArgs(UnitCombatStat.SKILL_POINTS));
            }
        }

        public List<AbilityData> AbilityDatas
        {
            get
            {
                return _abilityDatas;
            }

            set
            {
                _abilityDatas = value;
            }
        }

        public UnitCombatData()
        {
            
        }

        /// <summary>
        /// Use for creating unit reference data only
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="level"></param>
        /// <param name="experienceAwarded"></param>
        /// <param name="health"></param>
        /// <param name="mana"></param>
        /// <param name="attackSpeed"></param>
        /// <param name="physicalDamage"></param>
        /// <param name="physicalDR"></param>
        /// <param name="magicalDR"></param>
        public UnitCombatData(int unitType, int level, int experienceAwarded, int health, int mana, float hpRegen, float mpRegen, float attackSpeed, int physicalDamage, int magicalDamage, int physicalPenetration, int magicalPenetration, float critChance, float critDamage, int physicalDR, int magicalDR)
        {
            _unitTypeId = unitType;
            _level = level;
            _experienceAwarded = experienceAwarded;
            _currentHealth = health;
            _baseMaximumHealth = health;
            _totalMaximumHealth = health;
            _currentMana = mana;
            _baseMaximumMana = mana;
            _totalMaximumMana = mana;
            _baseHealthRegeneration = hpRegen;
            _totalHealthRegeneration = hpRegen;
            _baseManaRegeneration = mpRegen;
            _totalManaRegeneration = mpRegen;
            _baseAttackCooldown = attackSpeed;
            _totalAttackCooldown = attackSpeed;
            _basePhysicalAttackDamage = physicalDamage;
            _totalPhysicalAttackDamage = physicalDamage;
            _baseMagicalAttackDamage = magicalDamage;
            _totalMagicalAttackDamage = magicalDamage;
            _basePhysicalPenetration = physicalPenetration;
            _totalPhysicalPenetration = physicalPenetration;
            _baseMagicalPenetration = magicalPenetration;
            _totalMagicalPenetration = magicalPenetration;
            _baseCriticalChance = critChance;
            _totalCriticalChance = critChance;
            _baseCriticalDamage = critDamage;
            _totalCriticalDamage = critDamage;
            _basePhysicalDamageReduction = physicalDR;
            _totalPhysicalDamageReduction = physicalDR;
            _baseMagicalDamageReduction = magicalDR;
            _totalMagicalDamageReduction = magicalDR;
        }

        /// <summary>
        /// Use for creating hero unit reference only
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="level"></param>
        /// <param name="experience"></param>
        /// <param name="health"></param>
        /// <param name="mana"></param>
        /// <param name="attackSpeed"></param>
        /// <param name="physicalDamage"></param>
        /// <param name="physicalDR"></param>
        /// <param name="magicalDR"></param>
        /// <param name="skillPointValue"></param>
        public UnitCombatData(int unitType, int level, int experience, int health, int mana, float hpRegen, float mpRegen, float attackSpeed, int physicalDamage, int magicalDamage, int physicalPenetration, int magicalPenetration, int physicalDR, int magicalDR, float critChance, float critDamage, int skillPointValue, UnitAttribute primaryAttribute, int agility, int intelligence, int strength)
        {
            _unitTypeId = unitType;
            _level = level;
            _experience = experience;
            _currentHealth = health;
            _baseMaximumHealth = health;
            _totalMaximumHealth = health;
            _currentMana = mana;
            _baseMaximumMana = mana;
            _totalMaximumMana = mana;
            _baseHealthRegeneration = hpRegen;
            _totalHealthRegeneration = hpRegen;
            _baseManaRegeneration = mpRegen;
            _totalManaRegeneration = mpRegen;
            _primaryAttribute = primaryAttribute;
            _baseAgility = agility;
            _totalAgility = agility;
            _baseIntelligence = intelligence;
            _totalIntelligence = intelligence;
            _baseStrength = strength;
            _totalStrength = strength;
            _baseAttackCooldown = attackSpeed;
            _totalAttackCooldown = attackSpeed;
            _basePhysicalAttackDamage = physicalDamage;
            _totalPhysicalAttackDamage = physicalDamage;
            _baseMagicalAttackDamage = magicalDamage;
            _totalMagicalAttackDamage = magicalDamage;
            _basePhysicalPenetration = physicalPenetration;
            _totalPhysicalPenetration = physicalPenetration;
            _baseMagicalPenetration = magicalPenetration;
            _totalMagicalPenetration = magicalPenetration;
            _baseCriticalChance = critChance;
            _totalCriticalChance = critChance;
            _baseCriticalDamage = critDamage;
            _totalCriticalDamage = critDamage;
            _basePhysicalDamageReduction = physicalDR;
            _totalPhysicalDamageReduction = physicalDR;
            _baseMagicalDamageReduction = magicalDR;
            _totalMagicalDamageReduction = magicalDR;
            _skillPoints = skillPointValue;
        }

        /// <summary>
        /// Use for loading a hero unit
        /// </summary>
        /// <param name="heroData"></param>
        public UnitCombatData(HeroData heroData)
        {
            _unitTypeId = heroData.UnitTypeId;
            _level = heroData.Level;
            _experience = heroData.Experience;
            _currentHealth = heroData.Health;
            _baseMaximumHealth = heroData.Health;
            _totalMaximumHealth = heroData.Health;
            _currentMana = heroData.Mana;
            _baseMaximumMana = heroData.Mana;
            _totalMaximumMana = heroData.Mana;
            _baseHealthRegeneration = heroData.HealthRegeneration;
            _totalHealthRegeneration = heroData.HealthRegeneration;
            _baseManaRegeneration = heroData.ManaRegeneration;
            _totalManaRegeneration = heroData.ManaRegeneration;
            _primaryAttribute = heroData.PrimaryAttribute;
            _baseAgility = heroData.Agility;
            _baseIntelligence = heroData.Intelligence;
            _baseStrength = heroData.Strength;
            _baseAttackCooldown = heroData.AttackCooldown;
            _totalAttackCooldown = heroData.AttackCooldown;
            _basePhysicalAttackDamage = heroData.PhysicalAttackDamage;
            _totalPhysicalAttackDamage = heroData.PhysicalAttackDamage;
            _baseMagicalAttackDamage = heroData.MagicalAttackDamage;
            _totalMagicalAttackDamage = heroData.MagicalAttackDamage;
            _basePhysicalPenetration = heroData.PhysicalPenetration;
            _totalPhysicalPenetration = heroData.PhysicalPenetration;
            _baseMagicalPenetration = heroData.MagicalPenetration;
            _totalMagicalPenetration = heroData.MagicalPenetration;
            _baseCriticalChance = heroData.CriticalChance;
            _totalCriticalChance = heroData.CriticalChance;
            _baseCriticalDamage = heroData.CriticalDamage;
            _totalCriticalDamage = heroData.CriticalDamage;
            _basePhysicalDamageReduction = heroData.PhysicalDamageReduction;
            _totalPhysicalDamageReduction = heroData.PhysicalDamageReduction;
            _baseMagicalDamageReduction = heroData.MagicalDamageReduction;
            _totalMagicalDamageReduction = heroData.MagicalDamageReduction;
            _skillPoints = heroData.SkillPoints;
            _abilityDatas = heroData.AbilityDatas;
        }

        /// <summary>
        /// Create a 1:1 copy of unit combat data
        /// </summary>
        /// <param name="data"></param>
        public UnitCombatData(UnitCombatData data)
        {
            _unitTypeId = data.UnitTypeId;

            _level = data.Level;
            _experience = data.Experience;
            _experienceAwarded = data._experienceAwarded;

            _currentHealth = data.CurrentHealth;
            _baseMaximumHealth = data.BaseMaximumHealth;
            _bonusFlatHealth = data.BonusFlatHealth;
            _bonusPercentHealth = data.BonusPercentHealth;
            _totalMaximumHealth = data.TotalMaximumHealth;

            _baseHealthRegeneration = data.BaseHealthRegeneration;
            _bonusFlatHealthRegeneration = data.BonusFlatHealthRegeneration;
            _bonusPercentHealthRegeneration = data.BonusPercentHealthRegeneration;
            _totalHealthRegeneration = data.TotalHealthRegeneration;
            _percentMaximumHealthRegeneration = data.PercentMaximumHealthRegeneration;
            _healingReceivedBonus = data.HealingReceivedBonus;

            _currentMana = data.CurrentMana;
            _baseMaximumMana = data.BaseMaximumMana;
            _bonusFlatMana = data.BonusFlatMana;
            _bonusPercentMana = data.BonusPercentMana;
            _totalMaximumMana = data.TotalMaximumMana;

            _baseManaRegeneration = data.BaseManaRegeneration;
            _bonusFlatManaRegeneration = data.BonusFlatManaRegeneration;
            _bonusPercentManaRegeneration = data.BonusPercentManaRegeneration;
            _totalManaRegeneration = data.TotalManaRegeneration;
            _percentMaximumManaRegeneration = data.PercentMaximumManaRegeneration;

            _primaryAttribute = data.PrimaryAttribute;

            _baseAgility = data.BaseAgility;
            _bonusFlatAgility = data.BonusFlatAgility;
            _bonusPercentAgility = data.BonusPercentAgility;
            _totalAgility = data.TotalAgility;

            _baseIntelligence = data.BaseIntelligence;
            _bonusFlatIntelligence = data.BonusFlatIntelligence;
            _bonusPercentIntelligence = data.BonusPercentIntelligence;
            _totalIntelligence = data.TotalIntelligence;

            _baseStrength = data.BaseStrength;
            _bonusFlatStrength = data.BonusFlatStrength;
            _bonusPercentStrength = data.BonusPercentStrength;
            _totalStrength = data.TotalStrength;

            _baseAttackCooldown = data.BaseAttackCooldown;
            _bonusAttackCooldown = data.BonusAttackCooldown;
            _totalAttackCooldown = data.TotalAttackCooldown;

            _basePhysicalAttackDamage = data.BasePhysicalAttackDamage;
            _bonusFlatPhysicalAttackDamage = data.BonusFlatPhysicalAttackDamage;
            _bonusPercentPhysicalAttackDamage = data.BonusPercentPhysicalAttackDamage;
            _totalPhysicalAttackDamage = data.TotalPhysicalAttackDamage;

            _baseMagicalAttackDamage = data.BaseMagicalAttackDamage;
            _bonusFlatMagicalAttackDamage = data.BonusFlatMagicalAttackDamage;
            _bonusPercentMagicalAttackDamage = data.BonusPercentMagicalAttackDamage;
            _totalMagicalAttackDamage = data.TotalMagicalAttackDamage;

            _basePhysicalPenetration = data.BasePhysicalPenetration;
            _bonusFlatPhysicalPenetration = data.BonusFlatPhysicalPenetration;
            _totalPhysicalPenetration = data.TotalPhysicalPenetration;
            _absolutePercentPhysicalPenetration = data.AbsolutePercentPhysicalPenetration;

            _baseMagicalPenetration = data.BaseMagicalPenetration;
            _bonusFlatMagicalPenetration = data.BonusFlatMagicalPenetration;
            _totalMagicalPenetration = data.TotalMagicalPenetration;
            _absolutePercentMagicalPenetration = data.AbsolutePercentMagicalPenetration;

            _bonusPercentSkillDamage = data.BonusPercentSkillDamage;
            _bonusPercentAbilityDamage = data.BonusPercentAbilityDamage;
            _bonusPercentSpellDamage = data.BonusPercentSpellDamage;

            _baseCriticalChance = data.BaseCriticalChance;
            _bonusCriticalChance = data.BonusCriticalChance;
            _totalCriticalChance = data.TotalCriticalChance;
            _baseCriticalDamage = data.BaseCriticalDamage;
            _bonusCriticalDamage = data.BonusCriticalDamage;
            _totalCriticalDamage = data.TotalCriticalDamage;

            _basePhysicalDamageReduction = data.BasePhysicalDamageReduction;
            _bonusFlatPhysicalDamageReduction = data.BonusFlatPhysicalDamageReduction;
            _bonusPercentPhysicalDamageReduction = data.BonusPercentPhysicalDamageReduction;
            _totalPhysicalDamageReduction = data.TotalPhysicalDamageReduction;
            _absolutePhysicalDamageReductionPercent = data.AbsolutePhysicalDamageReductionPercent;

            _baseMagicalDamageReduction = data.BaseMagicalDamageReduction;
            _bonusFlatMagicalDamageReduction = data.BonusFlatMagicalDamageReduction;
            _bonusPercentMagicalDamageReduction = data.BonusPercentMagicalDamageReduction;
            _totalMagicalDamageReduction = data.TotalMagicalDamageReduction;
            _absoluteMagicalDamageReductionPercent = data.AbsoluteMagicalDamageReductionPercent;

            _skillPoints = data.SkillPoints;
            _abilityDatas = data.AbilityDatas;
        }

        private void ClampCurrentHealth()
        {
            _currentHealth = Math.Clamp(_currentHealth, 0, _totalMaximumHealth);
        }

        private void ClampCurrentMana()
        {
            _currentMana = Math.Clamp(_currentMana, 0, _totalMaximumMana);
        }

        private void CalculateHealthTotal()
        {
            //base + (base * percent) + bonus flat
            _totalMaximumHealth = MathRound(_baseMaximumHealth + (_baseMaximumHealth * _bonusPercentHealth) + _bonusFlatHealth);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.MAXIMUM_HEALTH));
            ClampCurrentHealth();
        }

        private void CalculateHealthRegenerationTotal()
        {
            //base + (base * percent) + bonus flat
            _totalHealthRegeneration = _baseHealthRegeneration + (_baseHealthRegeneration * _bonusPercentHealthRegeneration) + _bonusFlatHealthRegeneration;
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.HEALTH_REGENERATION));
        }

        private void CalculateManaTotal()
        {
            //base + (base * percent) + bonus flat
            _totalMaximumMana = MathRound(_baseMaximumMana + (_baseMaximumMana * _bonusPercentMana) + _bonusFlatMana);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.MAXIMUM_MANA));
            ClampCurrentMana();
        }

        private void CalculateManaRegenerationTotal()
        {
            //base + (base * percent) + bonus flat
            _totalManaRegeneration = _baseManaRegeneration + (_baseManaRegeneration * _bonusPercentManaRegeneration) + _bonusFlatManaRegeneration;
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.MANA_REGENERATION));
        }

        private void CalculateAgilityTotal()
        {
            //base + (base * percent) + bonus flat
            _totalAgility = MathRound(_baseAgility + (_baseAgility * _bonusPercentAgility) + _bonusFlatAgility);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.AGILITY));
        }

        private void CalculateIntelligenceTotal()
        {
            //base + (base * percent) + bonus flat
            _totalIntelligence = MathRound(_baseIntelligence + (_baseIntelligence * _bonusPercentIntelligence) + _bonusFlatIntelligence);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.INTELLIGENCE));
        }

        private void CalculateStrengthTotal()
        {
            //base + (base * percent) + bonus flat
            _totalStrength = MathRound(_baseStrength + (_baseStrength * _bonusPercentStrength) + _bonusFlatStrength);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.STRENGTH));
        }

        private void CalculateAttackCooldownTotal()
        {
            //base + (base * percent) + bonus flat
            float attacksPerSecond = 1.0f / _baseAttackCooldown;
            attacksPerSecond = attacksPerSecond * (1.0f + BonusAttackCooldown);
            _totalAttackCooldown = 1.0f / attacksPerSecond;
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.ATTACK_SPEED));
        }

        private void CalculatePhysicalAttackDamageTotal()
        {
            //base + (base * percent) + bonus flat
            _totalPhysicalAttackDamage = MathRound(_basePhysicalAttackDamage + (_basePhysicalAttackDamage * _bonusPercentPhysicalAttackDamage) + _bonusFlatPhysicalAttackDamage);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.PHYSICAL_ATTACK_DAMAGE));
        }

        private void CalculateMagicalAttackDamageTotal()
        {
            //base + (base * percent) + bonus flat
            _totalMagicalAttackDamage = MathRound(_baseMagicalAttackDamage + (_baseMagicalAttackDamage * _bonusPercentMagicalAttackDamage) + _bonusFlatMagicalAttackDamage);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.MAGICAL_ATTACK_DAMAGE));
        }

        private void CalculateTotalPhysicalPenetration()
        {
            _totalPhysicalPenetration = MathRound(_basePhysicalPenetration + _bonusFlatPhysicalPenetration);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.FLAT_PHYSICAL_PENETRATION));
        }

        private void CalculateTotalMagicalPeneration()
        {
            _totalMagicalPenetration = MathRound(_baseMagicalPenetration + _bonusFlatMagicalPenetration);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.FLAT_MAGICAL_PENETRATION));
        }

        private void CalculateTotalCriticalChance()
        {
            _totalCriticalChance = _baseCriticalChance + _bonusCriticalChance;
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.CRITICAL_CHANCE));
        }

        private void CalculateTotalCriticalDamage()
        {
            _totalCriticalDamage = _baseCriticalDamage + _bonusCriticalDamage;
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.CRITICAL_DAMAGE));
        }

        private void CalculatePhysicalDamageReductionTotal()
        {
            //base + (base * percent) + bonus flat
            _totalPhysicalDamageReduction = MathRound(_basePhysicalDamageReduction + (_basePhysicalDamageReduction * _bonusPercentPhysicalDamageReduction) + _bonusFlatPhysicalAttackDamage);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.FLAT_PHYSICAL_DAMAGE_REDUCTION));
        }

        private void CalculateMagicalDamageReductionTotal()
        {
            //base + (base * percent) + bonus flat
            _totalMagicalDamageReduction = MathRound(_baseMagicalDamageReduction + (_baseMagicalDamageReduction * _bonusPercentMagicalDamageReduction) + _bonusFlatMagicalDamageReduction);
            OnStatChanged(new StatChangeEventArgs(UnitCombatStat.FLAT_MAGICAL_DAMAGE_REDUCTION));
        }

        protected virtual void OnStatChanged(StatChangeEventArgs e)
        {
            //Console.WriteLine("Stat changed detected in UnitCombatData: " + e.CombatStat + "!");
            if (StatChanged != null)
            {
                StatChanged(this, new StatChangeEventArgs(e.CombatStat));
            }
        }
    }
}