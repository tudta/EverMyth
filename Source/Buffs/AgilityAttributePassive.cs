using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;
using static War3Api.Common;

namespace Source.Buffs
{
    public class AgilityAttributePassive : PassiveBuff
    {
        private float _primaryPhysicalDamageReductionBonus = 0.25f;
        private float _primaryAttackSpeedBonus = 0.05f;
        private float _primaryCriticalChanceBonus = 0.00025f;
        private float _primaryAbilityDamageBonus = 0.0005f;
        private float _primaryPhysicalAttackBonus = 1.0f;
        private float _primaryMagicalAttackBonus = 1.0f;

        private float _secondaryPhysicalDamageReductionBonus = 0.1f;
        private float _secondaryAttackSpeedBonus = 0.02f;
        private float _secondaryCriticalChanceBonus = 0.0001f;
        private float _secondaryAbilityDamageBonus = 0.0005f;
        private float _secondaryPhysicalAttackBonus = 0.0f;
        private float _secondaryMagicalAttackBonus = 0.0f;

        private float _appliedPhysicalDamageReductionBonus = 0.0f;
        private float _appliedAttackSpeedBonus = 0.0f;
        private float _appliedCriticalChanceBonus = 0.0f;
        private float _appliedAbilityDamageBonus = 0.0f;
        private float _appliedPhysicalAttackBonus = 0.0f;
        private float _appliedMagicalAttackBonus = 0.0f;

        public AgilityAttributePassive(unit caster, unit target) : base(caster, target)
        {
            Duration = float.MaxValue;
            IsBeneficial = true;
            BuffTypes.Add("AgilityAttributePassive");
            BuffTypes.Add("Undispellable");
        }

        public override void OnApply()
        {
            if (UnitManager.HeroUnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                if (tmpInstance.UnitData.PrimaryAttribute == UnitAttribute.AGILITY)
                {
                    Console.WriteLine("Applying primary agility passive!");
                    _appliedPhysicalDamageReductionBonus = tmpInstance.UnitData.TotalAgility * _primaryPhysicalDamageReductionBonus;
                    _appliedAttackSpeedBonus = tmpInstance.UnitData.TotalAgility * _primaryAttackSpeedBonus;
                    _appliedCriticalChanceBonus = tmpInstance.UnitData.TotalAgility * _primaryCriticalChanceBonus;
                    _appliedAbilityDamageBonus = tmpInstance.UnitData.TotalAgility * _primaryAbilityDamageBonus;
                    _appliedPhysicalAttackBonus = tmpInstance.UnitData.TotalAgility * _primaryPhysicalAttackBonus;
                    _appliedMagicalAttackBonus = tmpInstance.UnitData.TotalAgility * _primaryMagicalAttackBonus;
                }
                else
                {
                    Console.WriteLine("Applying secondary agility passive!");
                    _appliedPhysicalDamageReductionBonus = tmpInstance.UnitData.TotalAgility * _secondaryPhysicalDamageReductionBonus;
                    _appliedAttackSpeedBonus = tmpInstance.UnitData.TotalAgility * _secondaryAttackSpeedBonus;
                    _appliedCriticalChanceBonus = tmpInstance.UnitData.TotalAgility * _secondaryCriticalChanceBonus;
                    _appliedAbilityDamageBonus = tmpInstance.UnitData.TotalAgility * _secondaryAbilityDamageBonus;
                    _appliedPhysicalAttackBonus = tmpInstance.UnitData.TotalAgility * _secondaryPhysicalAttackBonus;
                    _appliedMagicalAttackBonus = tmpInstance.UnitData.TotalAgility * _secondaryMagicalAttackBonus;
                }
                tmpInstance.UnitData.BonusFlatPhysicalDamageReduction += _appliedPhysicalDamageReductionBonus;
                tmpInstance.UnitData.BonusAttackCooldown += _appliedAttackSpeedBonus;
                tmpInstance.UnitData.BonusCriticalChance += _appliedCriticalChanceBonus;
                tmpInstance.UnitData.BonusPercentAbilityDamage += _appliedAbilityDamageBonus;
                tmpInstance.UnitData.BonusFlatPhysicalAttackDamage += _appliedPhysicalAttackBonus;
                tmpInstance.UnitData.BonusFlatMagicalAttackDamage += _appliedMagicalAttackBonus;
            }
        }

        public override void OnDispose()
        {
            Console.WriteLine("Attempting to dispose agility attribute passive!");
            if (UnitManager.HeroUnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                Console.WriteLine("Removing agility passive!");
                tmpInstance.UnitData.BonusFlatPhysicalDamageReduction -= _appliedPhysicalDamageReductionBonus;
                tmpInstance.UnitData.BonusAttackCooldown -= _appliedAttackSpeedBonus;
                tmpInstance.UnitData.BonusCriticalChance -= _appliedCriticalChanceBonus;
                tmpInstance.UnitData.BonusPercentAbilityDamage -= _appliedAbilityDamageBonus;
                tmpInstance.UnitData.BonusFlatPhysicalAttackDamage -= _appliedPhysicalAttackBonus;
                tmpInstance.UnitData.BonusFlatMagicalAttackDamage -= _appliedMagicalAttackBonus;
            }
            base.OnDispose();
        }
    }
}
