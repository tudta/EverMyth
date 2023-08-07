using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using WCSharp.Buffs;

namespace Source
{
    public static class BuffTestSystem
    {
        public static void Init()
        {
            Console.WriteLine("Buff test system initialized!");
            AddHealthBuffToUnits();
        }

        private static void AddHealthBuffToUnits()
        {
            Buff tmpBuff;
            for (int i = 0; i < UnitManager.UnitInstanceDatabase.Count; i++)
            {
                unit tmpUnit = UnitManager.UnitInstanceDatabase.ElementAt(i).Key;
                Console.WriteLine("Adding health buff to " + GetUnitName(tmpUnit) + "!");
                tmpBuff = new Buffs.MaxHealthBuff(tmpUnit, tmpUnit);
                BuffSystem.Add(tmpBuff);
            }
        }
    }
}
