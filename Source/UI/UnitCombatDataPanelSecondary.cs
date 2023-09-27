using System;
using System.Collections.Generic;
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

namespace Source.UI
{
    public static class UnitCombatDataPanelSecondary
    {
        private static framehandle[] _icons = new framehandle[8];
        private static framehandle[] _texts = new framehandle[8];
        private static framehandle[] _buttons = new framehandle[8];
        private static framehandle[] _tooltips = new framehandle[8];
        private static string[] _iconValues = new string[8];
        private static string[] _descValues = new string[8];
        private static string[] _labelValues = new string[8];
        private static int _buttonCount = 8;
        private static framehandle _parent;
        private static trigger _buttonTrigger = null;

        public static void Init()
        {
            _buttonTrigger = CreateTrigger();
            TriggerAddAction(_buttonTrigger, ButtonAction);
            InitData(0, "Physical Damage: ", "PhysicalDamageIcon.dds", "Physical damage dealt with auto attacks");
            InitData(1, "Magical Damage: ", "MagicalDamageIcon.dds", "Physical damage dealt with auto attacks");
            InitData(2, "Physical Resistance: ", "PhysicalDamageReductionIcon.dds", "Damage reduced from physical attacks");
            InitData(3, "Magical Resistance: ", "MagicalDamageReductionIcon.dds", "Damage reduced from magical attacks");
            InitData(4, "Attack Speed: ", "AttackSpeedIcon.dds", "Increases rate of auto attacks");
            InitData(5, "Strength: ", "ui\\widgets\\console\\human\\infocard-heroattributes-str.dds", "Increases health, health regeneration, physical penetration, and critical damage");
            InitData(6, "Agility: ", "ui\\widgets\\console\\human\\infocard-heroattributes-agi.dds", "Increases physical reduction, attack speed, and critical chance");
            InitData(7, "Intelligence: ", "ui\\widgets\\console\\human\\infocard-heroattributes-int.dds", "Increases mana, mana regeneration, magical reduction, and magical penetration");
            At0s();
        }

        public static bool TooltipAction()
        {
            for (int i = 0; i < _buttonCount; i++)
            {
                if (UnitInfoPanelManager.TooltipFrame == _tooltips[i])
                {
                    UnitInfoPanelManager.TooltipTextValue = _labelValues[i] + BlzFrameGetText(_texts[i]) + "\n" + _descValues[i];
                    break;
                }
            }
            return true;
        }

        public static bool Condition()
        {
            return IsUnitType(UnitInfoPanelManager.SelectedUnit, UNIT_TYPE_HERO);
        }

        public static void ButtonAction()
        {
            framehandle frame = BlzGetTriggerFrame();
            UnitInfoPanelManager.GetSelectedUnit(GetTriggerPlayer());

            for (int i = 0; i < _buttonCount; i++)
            {
                if (frame == _buttons[i])
                {
                    BJDebugMsg(GetPlayerName(GetTriggerPlayer()) + " Clicked: " + I2S(i) + " " + GetUnitName(UnitInfoPanelManager.SelectedUnit));
                    return;
                }
            }
            frame = null;
        }

        public static bool Update()
        {
            //Console.WriteLine("UnitCombatDataPanelPrimary update function running!");
            UnitCombatData data = UnitManager.GetUnitInstance(UnitInfoPanelManager.SelectedUnit).UnitData;
            BlzFrameSetText(_texts[0], I2S(data.TotalPhysicalAttackDamage));
            BlzFrameSetText(_texts[1], I2S(data.TotalMagicalAttackDamage));
            BlzFrameSetText(_texts[2], I2S(data.TotalPhysicalDamageReduction));
            BlzFrameSetText(_texts[3], I2S(data.TotalMagicalDamageReduction));
            BlzFrameSetText(_texts[4], R2S(1.0f / data.TotalAttackCooldown));
            BlzFrameSetText(_texts[5], I2S(data.TotalStrength));
            BlzFrameSetText(_texts[6], I2S(data.TotalAgility));
            BlzFrameSetText(_texts[7], I2S(data.TotalIntelligence));
            return true;
        }

        public static void At0s()
        {
            //Console.WriteLine("UnitCombatDataPanelPrimary At0s function running!");

            _parent = BlzCreateSimpleFrame("CustomUnitInfoPanel2x4", BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0), 0);
            UnitInfoPanelManager.AddUnitInfoPanel(_parent, Update, Condition);

            for (int i = 0; i < _buttonCount; i++)
            {
                _buttons[i] = BlzGetFrameByName("CustomUnitInfoButton" + I2S(i + 1), 0);
                _tooltips[i] = BlzCreateFrameByType("SIMPLEFRAME", "", _buttons[i], "", 0);
                _icons[i] = BlzGetFrameByName("CustomUnitInfoButtonIcon" + I2S(i + 1), 0);
                _texts[i] = BlzGetFrameByName("CustomUnitInfoButtonText" + I2S(i + 1), 0);
                //BlzTriggerRegisterFrameEvent(_buttonTrigger, _buttons[i], FRAMEEVENT_CONTROL_CLICK);
                BlzFrameSetTooltip(_buttons[i], _tooltips[i]);
                BlzFrameSetVisible(_tooltips[i], false);
                BlzFrameSetTexture(_icons[i], _iconValues[i], 0, false);

                UnitInfoPanelManager.UnitInfoPanelAddTooltipListener(_tooltips[i], TooltipAction);
            }
        }

        private static void InitData(int index, string label, string icon, string desc)
        {
            _iconValues[index] = icon;
            _labelValues[index] = label;
            _descValues[index] = desc;
        }
    }
}