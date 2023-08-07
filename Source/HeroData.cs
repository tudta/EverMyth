using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.DateTime;

namespace Source
{
    public class HeroData
    {
        public int UnitTypeId { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public float HealthRegeneration { get; set; }
        public float ManaRegeneration { get; set; }
        public UnitAttribute PrimaryAttribute { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public float AttackCooldown { get; set; }
        public int PhysicalAttackDamage { get; set; }
        public int MagicalAttackDamage { get; set; }
        public int PhysicalPenetration { get; set; }
        public int MagicalPenetration { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamage { get; set; }
        public int PhysicalDamageReduction { get; set; }
        public int MagicalDamageReduction { get; set; }
        public int SkillPoints { get; set; }
        public List<AbilityData> AbilityDatas { get; set; }
    }
}