using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public class UnitAttackEventArgs : EventArgs
    {
        public UnitInstance AttackingUnit;
        public UnitInstance TargetUnit;
    }
}
