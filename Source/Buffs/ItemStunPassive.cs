using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;
using WCSharp.Dummies;
using WCSharp.Effects;
using static War3Api.Common;

namespace Source.Buffs
{
    public class ItemStunPassive : PassiveBuff
    {
        private const float _stunChance = 1.0f;
        private const float _stunDuration = 2.0f;

        public ItemStunPassive(unit caster, unit target) : base(caster, target)
        {
            Duration = float.MaxValue;
            IsBeneficial = true;
            BuffTypes.Add("ItemStunPassive");
            BuffTypes.Add("Undispellable");
        }

        public ItemStunPassive(Wrapper<unit> caster, Wrapper<unit> target) : base(caster.Value, target.Value)
        {
            Duration = float.MaxValue;
            IsBeneficial = true;
            BuffTypes.Add("ItemStunPassive");
            BuffTypes.Add("Undispellable");
        }

        public override void OnApply()
        {
            Console.WriteLine("Passive bop is being applied to " + GetUnitName(Target));
            DamageEngine.UnitAutoAttacked += BopEffect;
        }

        private void BopEffect(object sender, UnitAttackEventArgs args)
        {
            if (args.AttackingUnit.LinkedUnit != Target)
            {
                return;
            }
            float procRoll = GetRandomInt(1, 100);
            Console.WriteLine("Proc roll is " + procRoll + " and needs to be <= " + _stunChance + "!");
            if (procRoll <= _stunChance * 100)
            {
                Console.WriteLine("Bop effect activated!");
                StatusEffectManager.StunUnit(TargetPlayer, args.TargetUnit.LinkedUnit, _stunDuration);
            }
        }

        public override void OnDispose()
        {
            if (UnitManager.HeroUnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                Console.WriteLine("Passive bop is being removed from " + GetUnitName(Target));
                DamageEngine.UnitAutoAttacked -= BopEffect;
            }
            base.OnDispose();
        }
    }
}
