using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;
using static War3Api.Common;

namespace Source.Buffs
{
    public class IntelligenceAttributePassive : PassiveBuff
    {
        private float _primaryMagicReductionBonus = 0.25f;
        private float _primaryManaBonus = 10.0f;
        private float _primaryFlatManaRegenBonus = 0.5f;
        private float _primaryPercentManaRegenBonus = 0.02f;
        private float _primarySpellDamageBonus = 0.0005f;
        private float _primaryMagicalAttackBonus = 2.0f;

        private float _secondaryMagicReductionBonus = 0.1f;
        private float _secondaryManaBonus = 0.0f;
        private float _secondaryFlatManaRegenBonus = 0.0f;
        private float _secondaryPercentManaRegenBonus = 0.0f;
        private float _secondarySpellDamageBonus = 0.0005f;
        private float _secondaryMagicalAttackBonus = 0.0f;

        private float _appliedMagicReductionBonus = 0.0f;
        private float _appliedManaBonus = 0.0f;
        private float _appliedFlatManaRegenBonus = 0.0f;
        private float _appliedPercentManaRegenBonus = 0.0f;
        private float _appliedSpellDamageBonus = 0.0f;
        private float _appliedMagicAttackBonus = 0.0f;

        public IntelligenceAttributePassive(unit caster, unit target) : base(caster, target)
        {
            Duration = float.MaxValue;
            IsBeneficial = true;
            BuffTypes.Add("IntelligenceAttributePassive");
            BuffTypes.Add("Undispellable");
        }

        public override void OnApply()
        {
            if (UnitManager.HeroUnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                if (tmpInstance.UnitData.PrimaryAttribute == UnitAttribute.INTELLIGENCE)
                {
                    //Console.WriteLine("Applying primary intelligence passive!");
                    _appliedMagicReductionBonus = tmpInstance.UnitData.TotalIntelligence * _primaryMagicReductionBonus;
                    _appliedManaBonus = tmpInstance.UnitData.TotalIntelligence * _primaryManaBonus;
                    _appliedFlatManaRegenBonus = tmpInstance.UnitData.TotalIntelligence * _primaryFlatManaRegenBonus;
                    _appliedPercentManaRegenBonus = tmpInstance.UnitData.TotalIntelligence * _primaryPercentManaRegenBonus;
                    _appliedSpellDamageBonus = tmpInstance.UnitData.TotalIntelligence * _primarySpellDamageBonus;
                    _appliedMagicAttackBonus = tmpInstance.UnitData.TotalIntelligence * _primaryMagicalAttackBonus;
                }
                else
                {
                    //Console.WriteLine("Applying secondary intelligence passive!");
                    _appliedMagicReductionBonus = tmpInstance.UnitData.TotalIntelligence * _secondaryMagicReductionBonus;
                    _appliedManaBonus = tmpInstance.UnitData.TotalIntelligence * _secondaryManaBonus;
                    _appliedFlatManaRegenBonus = tmpInstance.UnitData.TotalIntelligence * _secondaryFlatManaRegenBonus;
                    _appliedPercentManaRegenBonus = tmpInstance.UnitData.TotalIntelligence * _secondaryPercentManaRegenBonus;
                    _appliedSpellDamageBonus = tmpInstance.UnitData.TotalIntelligence * _secondarySpellDamageBonus;
                    _appliedMagicAttackBonus = tmpInstance.UnitData.TotalIntelligence * _secondaryMagicalAttackBonus;
                }
                tmpInstance.UnitData.BonusFlatMagicalDamageReduction += _appliedMagicReductionBonus;
                tmpInstance.UnitData.BonusFlatMana += _appliedManaBonus;
                tmpInstance.UnitData.BonusFlatManaRegeneration += _appliedFlatManaRegenBonus;
                tmpInstance.UnitData.BonusPercentManaRegeneration += _appliedPercentManaRegenBonus;
                tmpInstance.UnitData.BonusPercentSpellDamage += _appliedSpellDamageBonus;
                tmpInstance.UnitData.BonusFlatMagicalAttackDamage += _appliedMagicAttackBonus;
            }
        }

        public override void OnDispose()
        {
            //Console.WriteLine("Attempting to dispose intelligence attribute passive!");
            if (UnitManager.HeroUnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                //Console.WriteLine("Removing intelligence passive!");
                tmpInstance.UnitData.BonusFlatMagicalDamageReduction -= _appliedMagicReductionBonus;
                tmpInstance.UnitData.BonusFlatMana -= _appliedManaBonus;
                tmpInstance.UnitData.BonusFlatManaRegeneration -= _appliedFlatManaRegenBonus;
                tmpInstance.UnitData.BonusPercentManaRegeneration -= _appliedPercentManaRegenBonus;
                tmpInstance.UnitData.BonusPercentSpellDamage -= _appliedSpellDamageBonus;
                tmpInstance.UnitData.BonusFlatMagicalAttackDamage -= _appliedMagicAttackBonus;
            }
            base.OnDispose();
        }
    }
}