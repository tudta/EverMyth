using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Source.UI.OldUI
{
    public class UnitStatPanel : CustomUIMenu
    {
        private framehandle _unitStatBackground = null;
        private framehandle _triggerUnitStatBackground = null;
        private framehandle _physicalDamageIcon = null;
        private framehandle _triggerPhysicalDamageIcon = null;
        private framehandle _physicalDamageText = null;
        private framehandle _triggerPhysicalDamageText = null;
        private framehandle _criticalStrikeChanceIcon = null;
        private framehandle _triggerCriticalStrikeChanceIcon = null;
        private framehandle _magicalDamageIcon = null;
        private framehandle _triggerMagicalDamageIcon = null;
        private framehandle _criticalStrikeDamageIcon = null;
        private framehandle _triggerCriticalStrikeDamageIcon = null;
        private framehandle _attackSpeedIcon = null;
        private framehandle _triggerAttackSpeedIcon = null;
        private framehandle _cooldownReductionIcon = null;
        private framehandle _triggerCooldownReductionIcon = null;
        private framehandle _physicalDamageReductionIcon = null;
        private framehandle _triggerPhysicalDamageReductionIcon = null;
        private framehandle _healthRegenerationIcon = null;
        private framehandle _triggerHealthRegenerationIcon = null;
        private framehandle _magicalDamageReductionIcon = null;
        private framehandle _triggerMagicalDamageReductionIcon = null;
        private framehandle _manaRegenerationIcon = null;
        private framehandle _triggerManaRegenerationIcon = null;
        private framehandle _criticalStrikeChanceText = null;
        private framehandle _triggerCriticalStrikeChanceText = null;
        private framehandle _magicalDamageText = null;
        private framehandle _triggerMagicalDamageText = null;
        private framehandle _criticalStrikeDamageText = null;
        private framehandle _triggerCriticalStrikeDamageText = null;
        private framehandle _attackSpeedText = null;
        private framehandle _triggerAttackSpeedText = null;
        private framehandle _cooldownReductionText = null;
        private framehandle _triggerCooldownReductionText = null;
        private framehandle _physicalDamageReductionText = null;
        private framehandle _triggerPhysicalDamageReductionText = null;
        private framehandle _healthRegenerationText = null;
        private framehandle _triggerHealthRegenerationText = null;
        private framehandle _magicalDamageReductionText = null;
        private framehandle _triggerMagicalDamageReductionText = null;
        private framehandle _manaRegenerationText = null;
        private framehandle _triggerManaRegenerationText = null;

        private trigger _unitSelectedTrigger = null;
        private trigger _unitDeselectedTrigger = null;

        private int _playerId = 0;

        public override void Init()
        {
            CreateMenu();
        }

        protected override void CreateMenu()
        {
            if (!IsUICreated)
            {
                _unitStatBackground = BlzCreateFrame("QuestButtonDisabledBackdropTemplate", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), 0, 0);
                BlzFrameSetAbsPoint(_unitStatBackground, FRAMEPOINT_TOPLEFT, 0.305000f, 0.0850000f);
                BlzFrameSetAbsPoint(_unitStatBackground, FRAMEPOINT_BOTTOMRIGHT, 0.405000f, 0.0200000f);

                _physicalDamageIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_physicalDamageIcon, FRAMEPOINT_TOPLEFT, 0.309000f, 0.0815000f);
                BlzFrameSetAbsPoint(_physicalDamageIcon, FRAMEPOINT_BOTTOMRIGHT, 0.319000f, 0.0715000f);
                BlzFrameSetTexture(_physicalDamageIcon, "PhysicalDamageIcon.dds", 0, true);

                _physicalDamageText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_physicalDamageText, FRAMEPOINT_TOPLEFT, 0.321000f, 0.0815000f);
                BlzFrameSetAbsPoint(_physicalDamageText, FRAMEPOINT_BOTTOMRIGHT, 0.362000f, 0.0715000f);
                BlzFrameSetText(_physicalDamageText, "|cffffffff7777777777|r");
                BlzFrameSetEnable(_physicalDamageText, false);
                BlzFrameSetScale(_physicalDamageText, 0.572f);
                BlzFrameSetTextAlignment(_physicalDamageText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _criticalStrikeChanceIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_criticalStrikeChanceIcon, FRAMEPOINT_TOPLEFT, 0.364000f, 0.0815000f);
                BlzFrameSetAbsPoint(_criticalStrikeChanceIcon, FRAMEPOINT_BOTTOMRIGHT, 0.374000f, 0.0715000f);
                BlzFrameSetTexture(_criticalStrikeChanceIcon, "CriticalStrikeChanceIcon.dds", 0, true);

                _magicalDamageIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_magicalDamageIcon, FRAMEPOINT_TOPLEFT, 0.309000f, 0.0695000f);
                BlzFrameSetAbsPoint(_magicalDamageIcon, FRAMEPOINT_BOTTOMRIGHT, 0.319000f, 0.0595000f);
                BlzFrameSetTexture(_magicalDamageIcon, "MagicalDamageIcon.dds", 0, true);

                _criticalStrikeDamageIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_criticalStrikeDamageIcon, FRAMEPOINT_TOPLEFT, 0.364000f, 0.0695000f);
                BlzFrameSetAbsPoint(_criticalStrikeDamageIcon, FRAMEPOINT_BOTTOMRIGHT, 0.374000f, 0.0595000f);
                BlzFrameSetTexture(_criticalStrikeDamageIcon, "CriticalStrikeDamageIcon.dds", 0, true);

                _attackSpeedIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_attackSpeedIcon, FRAMEPOINT_TOPLEFT, 0.309000f, 0.0575000f);
                BlzFrameSetAbsPoint(_attackSpeedIcon, FRAMEPOINT_BOTTOMRIGHT, 0.319000f, 0.0475000f);
                BlzFrameSetTexture(_attackSpeedIcon, "AttackSpeedIcon.dds", 0, true);

                _cooldownReductionIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_cooldownReductionIcon, FRAMEPOINT_TOPLEFT, 0.364000f, 0.0575000f);
                BlzFrameSetAbsPoint(_cooldownReductionIcon, FRAMEPOINT_BOTTOMRIGHT, 0.374000f, 0.0475000f);
                BlzFrameSetTexture(_cooldownReductionIcon, "CooldownReductionIcon.dds", 0, true);

                _physicalDamageReductionIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_physicalDamageReductionIcon, FRAMEPOINT_TOPLEFT, 0.309000f, 0.0455000f);
                BlzFrameSetAbsPoint(_physicalDamageReductionIcon, FRAMEPOINT_BOTTOMRIGHT, 0.319000f, 0.0355000f);
                BlzFrameSetTexture(_physicalDamageReductionIcon, "PhysicalDamageReductionIcon.dds", 0, true);

                _healthRegenerationIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_healthRegenerationIcon, FRAMEPOINT_TOPLEFT, 0.364000f, 0.0455000f);
                BlzFrameSetAbsPoint(_healthRegenerationIcon, FRAMEPOINT_BOTTOMRIGHT, 0.374000f, 0.0355000f);
                BlzFrameSetTexture(_healthRegenerationIcon, "HealthRegenerationIcon.dds", 0, true);

                _magicalDamageReductionIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_magicalDamageReductionIcon, FRAMEPOINT_TOPLEFT, 0.309000f, 0.0335000f);
                BlzFrameSetAbsPoint(_magicalDamageReductionIcon, FRAMEPOINT_BOTTOMRIGHT, 0.319000f, 0.0235000f);
                BlzFrameSetTexture(_magicalDamageReductionIcon, "MagicalDamageReductionIcon.dds", 0, true);

                _manaRegenerationIcon = BlzCreateFrameByType("BACKDROP", "BACKDROP", _unitStatBackground, "", 1);
                BlzFrameSetAbsPoint(_manaRegenerationIcon, FRAMEPOINT_TOPLEFT, 0.364000f, 0.0335000f);
                BlzFrameSetAbsPoint(_manaRegenerationIcon, FRAMEPOINT_BOTTOMRIGHT, 0.374000f, 0.0235000f);
                BlzFrameSetTexture(_manaRegenerationIcon, "ManaRegenerationIcon.dds", 0, true);

                _criticalStrikeChanceText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_criticalStrikeChanceText, FRAMEPOINT_TOPLEFT, 0.376000f, 0.0815000f);
                BlzFrameSetAbsPoint(_criticalStrikeChanceText, FRAMEPOINT_BOTTOMRIGHT, 0.417000f, 0.0715000f);
                BlzFrameSetText(_criticalStrikeChanceText, "|cffffffff100%|r");
                BlzFrameSetEnable(_criticalStrikeChanceText, false);
                BlzFrameSetScale(_criticalStrikeChanceText, 0.572f);
                BlzFrameSetTextAlignment(_criticalStrikeChanceText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _magicalDamageText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_magicalDamageText, FRAMEPOINT_TOPLEFT, 0.321000f, 0.0695000f);
                BlzFrameSetAbsPoint(_magicalDamageText, FRAMEPOINT_BOTTOMRIGHT, 0.362000f, 0.0595000f);
                BlzFrameSetText(_magicalDamageText, "|cffffffff7777777777|r");
                BlzFrameSetEnable(_magicalDamageText, false);
                BlzFrameSetScale(_magicalDamageText, 0.572f);
                BlzFrameSetTextAlignment(_magicalDamageText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _criticalStrikeDamageText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_criticalStrikeDamageText, FRAMEPOINT_TOPLEFT, 0.376000f, 0.0695000f);
                BlzFrameSetAbsPoint(_criticalStrikeDamageText, FRAMEPOINT_BOTTOMRIGHT, 0.417000f, 0.0595000f);
                BlzFrameSetText(_criticalStrikeDamageText, "|cffffffff10000%|r");
                BlzFrameSetEnable(_criticalStrikeDamageText, false);
                BlzFrameSetScale(_criticalStrikeDamageText, 0.572f);
                BlzFrameSetTextAlignment(_criticalStrikeDamageText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _attackSpeedText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_attackSpeedText, FRAMEPOINT_TOPLEFT, 0.321000f, 0.0575000f);
                BlzFrameSetAbsPoint(_attackSpeedText, FRAMEPOINT_BOTTOMRIGHT, 0.362000f, 0.0475000f);
                BlzFrameSetText(_attackSpeedText, "|cffffffff3.8|r");
                BlzFrameSetEnable(_attackSpeedText, false);
                BlzFrameSetScale(_attackSpeedText, 0.572f);
                BlzFrameSetTextAlignment(_attackSpeedText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _cooldownReductionText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_cooldownReductionText, FRAMEPOINT_TOPLEFT, 0.376000f, 0.0575000f);
                BlzFrameSetAbsPoint(_cooldownReductionText, FRAMEPOINT_BOTTOMRIGHT, 0.417000f, 0.0475000f);
                BlzFrameSetText(_cooldownReductionText, "|cffffffff100%|r");
                BlzFrameSetEnable(_cooldownReductionText, false);
                BlzFrameSetScale(_cooldownReductionText, 0.572f);
                BlzFrameSetTextAlignment(_cooldownReductionText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _physicalDamageReductionText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_physicalDamageReductionText, FRAMEPOINT_TOPLEFT, 0.321000f, 0.0455000f);
                BlzFrameSetAbsPoint(_physicalDamageReductionText, FRAMEPOINT_BOTTOMRIGHT, 0.362000f, 0.0355000f);
                BlzFrameSetText(_physicalDamageReductionText, "|cffffffff7777777777|r");
                BlzFrameSetEnable(_physicalDamageReductionText, false);
                BlzFrameSetScale(_physicalDamageReductionText, 0.572f);
                BlzFrameSetTextAlignment(_physicalDamageReductionText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _healthRegenerationText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_healthRegenerationText, FRAMEPOINT_TOPLEFT, 0.376000f, 0.0455000f);
                BlzFrameSetAbsPoint(_healthRegenerationText, FRAMEPOINT_BOTTOMRIGHT, 0.417000f, 0.0355000f);
                BlzFrameSetText(_healthRegenerationText, "|cffffffff14550.2|r");
                BlzFrameSetEnable(_healthRegenerationText, false);
                BlzFrameSetScale(_healthRegenerationText, 0.572f);
                BlzFrameSetTextAlignment(_healthRegenerationText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _magicalDamageReductionText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_magicalDamageReductionText, FRAMEPOINT_TOPLEFT, 0.321000f, 0.0335000f);
                BlzFrameSetAbsPoint(_magicalDamageReductionText, FRAMEPOINT_BOTTOMRIGHT, 0.362000f, 0.0235000f);
                BlzFrameSetText(_magicalDamageReductionText, "|cffffffff7777777777|r");
                BlzFrameSetEnable(_magicalDamageReductionText, false);
                BlzFrameSetScale(_magicalDamageReductionText, 0.572f);
                BlzFrameSetTextAlignment(_magicalDamageReductionText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                _manaRegenerationText = BlzCreateFrameByType("TEXT", "name", _unitStatBackground, "", 0);
                BlzFrameSetAbsPoint(_manaRegenerationText, FRAMEPOINT_TOPLEFT, 0.376000f, 0.0335000f);
                BlzFrameSetAbsPoint(_manaRegenerationText, FRAMEPOINT_BOTTOMRIGHT, 0.417000f, 0.0235000f);
                BlzFrameSetText(_manaRegenerationText, "|cffffffff125344|r");
                BlzFrameSetEnable(_manaRegenerationText, false);
                BlzFrameSetScale(_manaRegenerationText, 0.572f);
                BlzFrameSetTextAlignment(_manaRegenerationText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_LEFT);

                BaseFrameHandle = _unitStatBackground;
                IsUICreated = true;
                //base.CloseMenu();
            }
        }

        public void AssignPlayerSelectionTrigger(int playerId)
        {
            _playerId = playerId;

            _unitSelectedTrigger = CreateTrigger();
            TriggerRegisterPlayerSelectionEventBJ(_unitSelectedTrigger, PlayerManager.Players[_playerId], true);
            TriggerAddAction(_unitSelectedTrigger, UpdateStatMenuText);

            _unitDeselectedTrigger = CreateTrigger();
            TriggerRegisterPlayerSelectionEventBJ(_unitSelectedTrigger, PlayerManager.Players[_playerId], false);
            TriggerAddAction(_unitSelectedTrigger, ClosePanel);
        }

        private void UpdateStatMenuText()
        {
            Console.WriteLine(GetUnitName(GetTriggerUnit()));
            if (GetLocalPlayer() == PlayerManager.Players[_playerId])
            {
                if (!BlzFrameIsVisible(BaseFrameHandle))
                {
                    Console.WriteLine("Opening stat panel!");
                    base.OpenMenu();
                }
                Console.WriteLine("Updating stat panel!");
                UnitCombatData data = UnitManager.GetUnitInstance(GetTriggerUnit()).UnitData;
                BlzFrameSetText(_physicalDamageText, "|cffffffff" + data.TotalPhysicalAttackDamage + "|r");
                BlzFrameSetText(_criticalStrikeChanceText, "|cffffffff" + data.TotalCriticalChance + "|r");
                BlzFrameSetText(_magicalDamageText, "|cffffffff" + data.TotalMagicalAttackDamage + "|r");
                BlzFrameSetText(_criticalStrikeDamageText, "|cffffffff" + data.TotalCriticalDamage + "|r");
                BlzFrameSetText(_attackSpeedText, "|cffffffff" + data.TotalAttackCooldown + "|r");
                BlzFrameSetText(_cooldownReductionText, "|cffffffff" + "0" + "|r");
                BlzFrameSetText(_physicalDamageReductionText, "|cffffffff" + data.TotalPhysicalDamageReduction + "|r");
                BlzFrameSetText(_healthRegenerationText, "|cffffffff" + data.TotalHealthRegeneration + "|r");
                BlzFrameSetText(_magicalDamageReductionText, "|cffffffff" + data.TotalMagicalDamageReduction + "|r");
                BlzFrameSetText(_manaRegenerationText, "|cffffffff" + data.TotalManaRegeneration + "|r");
            }
        }

        private void ClosePanel()
        {
            if (GetLocalPlayer() == PlayerManager.Players[_playerId])
            {
                group selectedUnitGroup = GetUnitsSelectedAll(PlayerManager.Players[_playerId]);
                if (BlzGroupGetSize(selectedUnitGroup) == 0)
                {
                    Console.WriteLine("Closing stat panel!");
                    base.CloseMenu();
                }
            }
        }
    }
}
