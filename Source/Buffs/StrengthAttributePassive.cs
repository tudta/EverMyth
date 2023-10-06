using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;
using static War3Api.Common;

namespace Source.Buffs
{
    public class StrengthAttributePassive : PassiveBuff
    {
        private float _primaryHealthBonus = 10.0f;
        private float _primaryPhysicalPenetrationBonus = 0.25f;
        private float _primaryFlatHealthRegenBonus = 0.5f;
        private float _primaryPercentHealthRegenBonus = 0.02f;
        private float _primaryCriticalDamageBonus = 0.0025f;
        private float _primaryPhysicalAttackBonus = 2.0f;

        private float _secondaryHealthBonus = 5.0f;
        private float _secondaryPhysicalPenetrationBonus = 0.1f;
        private float _secondaryFlatHealthRegenBonus = 0.2f;
        private float _secondaryPercentHealthRegenBonus = 0.01f;
        private float _secondaryCriticalDamageBonus = 0.001f;
        private float _secondaryPhysicalAttackBonus = 0.0f;

        private float _appliedHealthBonus = 0.0f;
        private float _appliedPhysicalPenetrationBonus = 0.0f;
        private float _appliedFlatHealthRegenBonus = 0.0f;
        private float _appliedPercentHealthRegenBonus = 0.0f;
        private float _appliedCriticalDamageBonus = 0.0f;
        private float _appliedPhysicalAttackBonus = 0.0f;

        public StrengthAttributePassive(unit caster, unit target) : base(caster, target)
        {
            Duration = float.MaxValue;
            IsBeneficial = true;
            BuffTypes.Add("StrengthAttributePassive");
            BuffTypes.Add("Undispellable");
        }

        public override void OnApply()
        {
            if (UnitManager.HeroUnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                if (tmpInstance.UnitData.PrimaryAttribute == UnitAttribute.STRENGTH)
                {
                    //Console.WriteLine("Applying primary strength passive!");
                    _appliedHealthBonus = tmpInstance.UnitData.TotalStrength * _primaryHealthBonus;
                    _appliedPhysicalPenetrationBonus = tmpInstance.UnitData.TotalStrength * _primaryPhysicalPenetrationBonus;
                    _appliedFlatHealthRegenBonus = tmpInstance.UnitData.TotalStrength * _primaryFlatHealthRegenBonus;
                    _appliedPercentHealthRegenBonus = tmpInstance.UnitData.TotalStrength * _primaryPercentHealthRegenBonus;
                    _appliedCriticalDamageBonus = tmpInstance.UnitData.TotalStrength * _primaryCriticalDamageBonus;
                    _appliedPhysicalAttackBonus = tmpInstance.UnitData.TotalStrength * _primaryPhysicalAttackBonus;
                }
                else
                {
                    //Console.WriteLine("Applying secondary strength passive!");
                    _appliedHealthBonus = tmpInstance.UnitData.TotalStrength * _secondaryHealthBonus;
                    _appliedPhysicalPenetrationBonus = tmpInstance.UnitData.TotalStrength * _secondaryPhysicalPenetrationBonus;
                    _appliedFlatHealthRegenBonus = tmpInstance.UnitData.TotalStrength * _secondaryFlatHealthRegenBonus;
                    _appliedPercentHealthRegenBonus = tmpInstance.UnitData.TotalStrength * _secondaryPercentHealthRegenBonus;
                    _appliedCriticalDamageBonus = tmpInstance.UnitData.TotalStrength * _secondaryCriticalDamageBonus;
                    _appliedPhysicalAttackBonus = tmpInstance.UnitData.TotalStrength * _secondaryPhysicalAttackBonus;
                }
                tmpInstance.UnitData.BonusFlatHealth += _appliedHealthBonus;
                tmpInstance.UnitData.BonusFlatPhysicalPenetration += _appliedPhysicalPenetrationBonus;
                tmpInstance.UnitData.BonusFlatHealthRegeneration += _appliedFlatHealthRegenBonus;
                tmpInstance.UnitData.BonusPercentHealthRegeneration += _appliedPercentHealthRegenBonus;
                tmpInstance.UnitData.BonusCriticalDamage += _appliedCriticalDamageBonus;
                tmpInstance.UnitData.BonusFlatPhysicalAttackDamage += _appliedPhysicalAttackBonus;
            }
        }

        public override void OnDispose()
        {
            //Console.WriteLine("Attempting to dispose strength attribute passive!");
            if (UnitManager.HeroUnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                //Console.WriteLine("Removing strength passive!");
                tmpInstance.UnitData.BonusFlatHealth -= _appliedHealthBonus;
                tmpInstance.UnitData.BonusFlatPhysicalPenetration -= _appliedPhysicalPenetrationBonus;
                tmpInstance.UnitData.BonusFlatHealthRegeneration -= _appliedFlatHealthRegenBonus;
                tmpInstance.UnitData.BonusPercentHealthRegeneration -= _appliedPercentHealthRegenBonus;
                tmpInstance.UnitData.BonusCriticalDamage -= _appliedCriticalDamageBonus;
                tmpInstance.UnitData.BonusFlatPhysicalAttackDamage -= _appliedPhysicalAttackBonus;
            }
            base.OnDispose();
        }
    }
}