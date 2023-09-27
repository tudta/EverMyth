using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.DamageSystem
{
    public class UnitDamagedEventArgs : EventArgs
    {
        public UnitInstance AttackingUnit;
        public UnitInstance TargetUnit;
        public DamageType DamageDealtType;
        public int DamageDealtAmount;
    }
}