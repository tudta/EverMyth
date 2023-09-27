using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.DamageSystem
{
    public class DamageInstance
    {
        public UnitInstance DamageInstanceSource;
        public UnitInstance DamageInstanceTarget;
        public DamageType DamageInstanceType;
        public float DamageInstanceAmount;

        public DamageInstance(UnitInstance source, UnitInstance target, DamageType type, int amount)
        {
            DamageInstanceSource = source;
            DamageInstanceTarget = target;
            DamageInstanceType = type;
            DamageInstanceAmount = amount;
        }
    }
}