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
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.Abilities
{
    public abstract class CustomAbility
    {
        private int _rawCode;
        private int _customAbilityId;

        protected int RawCode
        {
            get
            {
                return _rawCode;
            }

            set
            {
                _rawCode = value;
            }
        }

        protected int CustomAbilityId
        {
            get
            {
                return _customAbilityId;
            }

            set
            {
                _customAbilityId = value;
            }
        }

        public virtual void Init()
        {
            CreateAbilityTrigger();
        }

        protected void CreateAbilityTrigger()
        {
            trigger castTrigger = CreateTrigger();
            TriggerRegisterAnyUnitEventBJ(castTrigger, EVENT_PLAYER_UNIT_SPELL_EFFECT);
            TriggerAddCondition(castTrigger, Condition(IsCorrectAbility));
            TriggerAddAction(castTrigger, CastAbility);
        }

        protected void CastAbility()
        {
            if (IsDummyCaster())
            {
                DummyCastAbility();
                return;
            }
            UnitCastAbility();
        }

        protected virtual void UnitCastAbility()
        {
            Console.WriteLine("A unit cast an ability!");
        }

        protected virtual void DummyCastAbility()
        {
            Console.WriteLine("A dummy cast an ability!");
        }

        protected bool IsCorrectAbility()
        {
            if (GetSpellAbilityId() == _customAbilityId)
            {
                return true;
            }
            return false;
        }

        protected bool IsDummyCaster()
        {
            if (GetUnitTypeId(GetSpellAbilityUnit()) == Constants.UNIT_DUMMY)
            {
                return true;
            }
            return false;
        }
    }
}