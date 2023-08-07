using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;

namespace Source
{
    public static class BuffManager
    {
        public static void Init()
        {

        }

        public static void RemoveAllBuffs(UnitInstance unitInstance)
        {
            Console.WriteLine("Removing all buffs from unit!");
            int buffCount = BuffSystem.GetBuffsOnUnit(unitInstance.LinkedUnit).Count();
            BuffSystem.Dispel(unitInstance.LinkedUnit, null, true, buffCount);
            BuffSystem.Dispel(unitInstance.LinkedUnit, null, false, buffCount);
        }

        public static void ApplyAttributePassives(UnitInstance unitInstance)
        {
            ApplyAgilityAttributePassive(unitInstance);
            ApplyIntelligenceAttributePassive(unitInstance);
            ApplyStrengthAttributePassive(unitInstance);
        }
            
        public static void ApplyAgilityAttributePassive(UnitInstance unitInstance)
        {
            Console.WriteLine("Applying agility passive from buff manager!");
            BuffSystem.Dispel(unitInstance.LinkedUnit, null, true, 1, "AgilityAttributePassive");
            BuffSystem.Add(new Buffs.AgilityAttributePassive(null, unitInstance.LinkedUnit));
        }

        public static void ApplyIntelligenceAttributePassive(UnitInstance unitInstance)
        {
            Console.WriteLine("Applying intelligence passive from buff manager!");
            BuffSystem.Dispel(unitInstance.LinkedUnit, null, true, 1, "IntelligenceAttributePassive");
            BuffSystem.Add(new Buffs.IntelligenceAttributePassive(null, unitInstance.LinkedUnit));
        }

        public static void ApplyStrengthAttributePassive(UnitInstance unitInstance)
        {
            Console.WriteLine("Applying strength passive from buff manager!");
            BuffSystem.Dispel(unitInstance.LinkedUnit, null, true, 1, "StrengthAttributePassive");
            BuffSystem.Add(new Buffs.StrengthAttributePassive(null, unitInstance.LinkedUnit));
        }
    }
}