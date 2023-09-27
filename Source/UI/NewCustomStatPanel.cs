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
    public static class NewCustomStatPanel
    {
        private static framehandle[] _icons = new framehandle[12];
        private static framehandle[] _texts = new framehandle[12];
        private static framehandle[] _buttons = new framehandle[12];
        private static framehandle[] _tooltips = new framehandle[12];
        private static string[] _iconValues = new string[12];
        private static string[] _descValues = new string[12];
        private static string[] _labelValues = new string[12];
        private static int _buttonCount = 12;
        private static framehandle _parent;
        private static trigger _buttonTrigger = null;

        public static void Init()
        {
            _buttonTrigger = CreateTrigger();
            //TimerStart(CreateTimer(), 0, false, At0s);
            TriggerAddAction(_buttonTrigger, ButtonAction);
            InitData(0, "Damage: ", "ReplaceableTextures\\CommandButtons\\BTNSteelMelee", "The amount of damage your basic Attack deals");
            InitData(1, "Armor: ", "ReplaceableTextures\\CommandButtons\\BTNHumanArmorUpOne", "Reduces Taken non-magical damage");
            InitData(2, "Res: ", "ReplaceableTextures\\CommandButtons\\BTNThickFur", "Reduces Taken magical damage");
            InitData(3, "Crit: ", "ReplaceableTextures\\CommandButtons\\BTNCriticalStrike", "");
            InitData(4, "Speed: ", "ReplaceableTextures\\CommandButtons\\BTNBootsOfSpeed", "The unit's current movespeed");
            InitData(5, "Str: ", "ReplaceableTextures\\CommandButtons\\BTNGauntletsOfOgrePower", "Increases Life and Life regeneration");
            InitData(6, "Agi: ", "ReplaceableTextures\\CommandButtons\\BTNSlippersOfAgility", "Improves Armor and Attack speed");
            InitData(7, "Int: ", "ReplaceableTextures\\CommandButtons\\BTNMantleOfIntelligence", "Improves Mana and Mana regeneration");
            InitData(8, "Power: ", "ReplaceableTextures\\CommandButtons\\BTNControlMagic", "Makes most abilities better");
            InitData(9, "Hp/s: ", "ReplaceableTextures\\CommandButtons\\BTNRegenerate", "");
            InitData(10, "Mp/s: ", "ReplaceableTextures\\CommandButtons\\BTNMagicalSentry", "");
            InitData(11, "Evasion: ", "ReplaceableTextures\\CommandButtons\\BTNEvasion", "");
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
            Console.WriteLine("New custom stat panel update function running!");
            BlzFrameSetText(_texts[0], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconValue", 0)));
            BlzFrameSetText(_texts[1], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconValue", 2)));
            BlzFrameSetText(_texts[2], "0");
            BlzFrameSetText(_texts[3], "0");
            BlzFrameSetText(_texts[4], I2S(R2I(GetUnitMoveSpeed(UnitInfoPanelManager.SelectedUnit))));
            BlzFrameSetText(_texts[5], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconHeroStrengthValue", 6)));
            BlzFrameSetText(_texts[6], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconHeroAgilityValue", 6)));
            BlzFrameSetText(_texts[7], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconHeroIntellectValue", 6)));
            BlzFrameSetText(_texts[9], R2SW(BlzGetUnitRealField(UnitInfoPanelManager.SelectedUnit, UNIT_RF_HIT_POINTS_REGENERATION_RATE), 1, 1));
            BlzFrameSetText(_texts[10], R2SW(BlzGetUnitRealField(UnitInfoPanelManager.SelectedUnit, UNIT_RF_MANA_REGENERATION), 1, 1));
            return true;
        }

        public static void At0s()
        {
            Console.WriteLine("New custom stat panel At0s function running!");
            framehandle prevIcon;

            BlzGetFrameByName("InfoPanelIconValue", 0);
            BlzGetFrameByName("InfoPanelIconValue", 2);
            BlzGetFrameByName("InfoPanelIconHeroStrengthValue", 6);
            BlzGetFrameByName("InfoPanelIconHeroAgilityValue", 6);
            BlzGetFrameByName("InfoPanelIconHeroIntellectValue", 6);

            _parent = BlzCreateSimpleFrame("CustomUnitInfoPanel3x4", BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0), 0);
            UnitInfoPanelManager.AddUnitInfoPanel(_parent, Update, Condition);

            for (int i = 0; i < _buttonCount; i++)
            {
                _buttons[i] = BlzGetFrameByName("CustomUnitInfoButton" + I2S(i + 1), 0);
                _tooltips[i] = BlzCreateFrameByType("SIMPLEFRAME", "", _buttons[i], "", 0);
                _icons[i] = BlzGetFrameByName("CustomUnitInfoButtonIcon" + I2S(i + 1), 0);
                _texts[i] = BlzGetFrameByName("CustomUnitInfoButtonText" + I2S(i + 1), 0);
                BlzTriggerRegisterFrameEvent(_buttonTrigger, _buttons[i], FRAMEEVENT_CONTROL_CLICK);
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