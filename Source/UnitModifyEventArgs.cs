using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;

namespace Source
{
    public class UnitModifyEventArgs : EventArgs
    {
        public unit ModifiedUnit;
        public UnitCombatStat ModifiedStat;

        public UnitModifyEventArgs(unit modifiedUnit, UnitCombatStat modifiedStat)
        {
            ModifiedUnit = modifiedUnit;
            ModifiedStat = modifiedStat;
        }
    }
}
