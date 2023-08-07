using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;
using static War3Api.Common;

namespace Source.Buffs
{
    public class MaxHealthBuff : PassiveBuff
    {
        public MaxHealthBuff(unit caster, unit target) : base(caster, target)
        {
            Duration = float.MaxValue;
            IsBeneficial = true;
        }

        public override void OnApply()
        {
            Console.WriteLine("Max health buff is being applied to " + GetUnitName(Target));
            UnitManager.UnitInstanceDatabase[Target].UnitData.BonusFlatHealth += 100.0f;
        }

        public override void OnDispose()
        {
            if (UnitManager.UnitInstanceDatabase.TryGetValue(Target, out UnitInstance tmpInstance))
            {
                Console.WriteLine("Max health buff is being removed from " + GetUnitName(Target));
                UnitManager.UnitInstanceDatabase[Target].UnitData.BonusFlatHealth -= 100.0f;
            }
            base.OnDispose();
        }
    }
}
