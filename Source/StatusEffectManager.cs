using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;
using WCSharp.Dummies;
using WCSharp.Effects;
using static War3Api.Common;

namespace Source
{
    public static class StatusEffectManager 
    {
        public static void StunUnit(player owningPlayer, unit targetUnit, float stunDuration)
        {
            location targetLoc = GetUnitLoc(targetUnit);
            var dummy = DummySystem.GetDummy(GetLocationX(targetLoc), GetLocationY(targetLoc), GetLocationZ(targetLoc), owningPlayer);
            UnitAddAbility(dummy, Constants.ABILITY_STUN_EFFECT);
            BlzSetAbilityRealLevelField(BlzGetUnitAbility(dummy, Constants.ABILITY_STUN_EFFECT), ABILITY_RLF_DURATION_NORMAL, 0, stunDuration);
            IssueTargetOrderById(dummy, Constants.ORDER_FIREBOLT, targetUnit);
            UnitRemoveAbility(dummy, Constants.ABILITY_STUN_EFFECT);
        }

        public static void SilenceUnit(player owningPlayer, unit targetUnit, float stunDuration)
        {

        }
    }
}
