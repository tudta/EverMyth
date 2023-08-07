using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;

namespace Source
{
    public class StatChangeEventArgs : EventArgs
    {
        public UnitCombatStat CombatStat;

        public StatChangeEventArgs(UnitCombatStat combatStat)
        {
            CombatStat = combatStat;
        }
    }
}