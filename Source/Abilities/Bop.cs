using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using WCSharp;
using WCSharp.Buffs;
using WCSharp.DateTime;
using WCSharp.Dummies;
using WCSharp.Effects;
using WCSharp.Events;
using WCSharp.Json;
using WCSharp.Knockbacks;
using WCSharp.Lightnings;
using WCSharp.Missiles;
using WCSharp.SaveLoad;
using WCSharp.Shared;
using WCSharp.Sync;
using static Constants;
using static Regions;
using War3Api;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source.DamageSystem;

namespace Source.Abilities
{
    public class Bop : CustomAbility
    {
        public override void Init()
        {
            CustomAbilityId = Constants.ABILITY_BOP;
            base.Init();
        }

        protected override void UnitCastAbility()
        {
            UnitInstance casterUnitInstance = UnitManager.GetUnitInstance(GetSpellAbilityUnit());
            UnitInstance targetUnitInstance = UnitManager.GetUnitInstance(GetSpellTargetUnit());
            Console.WriteLine(GetUnitName(casterUnitInstance.LinkedUnit) + " has cast " + GetAbilityName(CustomAbilityId) + " on " + GetUnitName(targetUnitInstance.LinkedUnit));
            DamageSystem.DamageEngine.DamageUnit(casterUnitInstance, targetUnitInstance, DamageType.PURE, 100.0f);
        }
    }
}