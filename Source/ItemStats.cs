using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public class ItemStats
    {
        private float _bonusFlatHealth = 0;
        private float _bonusPercentHealth = 0.0f;

        private float _bonusFlatHealthRegeneration = 0.0f;
        private float _bonusPercentHealthRegeneration = 0.0f;
        private float _percentMaximumHealthRegeneration = 0.0f;
        private float _healingReceivedBonus = 0.0f;

        private float _bonusFlatMana = 0;
        private float _bonusPercentMana = 0.0f;

        private float _bonusFlatManaRegeneration = 0.0f;
        private float _bonusPercentManaRegeneration = 0.0f;
        private float _percentMaximumManaRegeneration = 0.0f;

        private float _bonusFlatAgility = 0;
        private float _bonusPercentAgility = 0.0f;

        private float _bonusFlatIntelligence = 0;
        private float _bonusPercentIntelligence = 0.0f;

        private float _bonusFlatStrength = 0;
        private float _bonusPercentStrength = 0.0f;

        private float _bonusAttackCooldown = 0.0f;

        private float _bonusFlatPhysicalAttackDamage = 0;
        private float _bonusPercentPhysicalAttackDamage = 0.0f;

        private float _bonusFlatMagicalAttackDamage = 0;
        private float _bonusPercentMagicalAttackDamage = 0.0f;

        private float _bonusFlatPhysicalPenetration = 0.0f;
        private float _absolutePercentPhysicalPenetration = 0.0f;

        private float _bonusFlatMagicalPenetration = 0.0f;
        private float _absolutePercentMagicalPenetration = 0.0f;

        private float _bonusPercentSkillDamage = 0.0f;
        private float _bonusPercentAbilityDamage = 0.0f;
        private float _bonusPercentSpellDamage = 0.0f;

        private float _bonusCriticalChance = 0.0f;
        private float _bonusCriticalDamage = 0.0f;

        private float _bonusFlatPhysicalDamageReduction = 0;
        private float _bonusPercentPhysicalDamageReduction = 0.0f;
        private float _absolutePhysicalDamageReductionPercent = 0.0f;

        private float _bonusFlatMagicalDamageReduction = 0;
        private float _bonusPercentMagicalDamageReduction = 0.0f;
        private float _absoluteMagicalDamageReductionPercent = 0.0f;

        public float BonusFlatHealth
        {
            get
            {
                return _bonusFlatHealth;
            }

            set
            {
                _bonusFlatHealth = value;
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

        public float BonusFlatMana
        {
            get
            {
                return _bonusFlatMana;
            }

            set
            {
                _bonusFlatMana = value;
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

        public float BonusFlatAgility
        {
            get
            {
                return _bonusFlatAgility;
            }

            set
            {
                _bonusFlatAgility = value;
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
            }
        }
    }
}
