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

namespace Source.UI.OldUI
{
    public static class CustomStatPanel
    {
        private static framehandle[] Icon = new framehandle[13];
        private static framehandle[] Text = new framehandle[13];
        private static framehandle[] Button = new framehandle[13];
        private static framehandle[] ToolTip = new framehandle[13];
        private static string[] DataIcon = new string[13];
        private static string[] DataDesc = new string[13];
        private static string[] DataLabel = new string[13];
        private static int buttonCount = 12;
        private static framehandle parent;
        private static trigger buttonTrigger = CreateTrigger();

        public static void Init()
        {
            TimerStart(CreateTimer(), 0, false, At0s);
            TriggerAddAction(buttonTrigger, ButtonAction);
            FrameLoader.FrameLoaderAdd(At0s);
            InitData(1, "Damage: ", "ReplaceableTextures\\CommandButtons\\BTNSteelMelee", "The amount of damage your basic Attack deals");
            InitData(2, "Armor: ", "ReplaceableTextures\\CommandButtons\\BTNHumanArmorUpOne", "Reduces Taken non-magical damage");
            InitData(3, "Ress: ", "ReplaceableTextures\\CommandButtons\\BTNThickFur", "Reduces Taken magical damage");
            InitData(4, "Crit: ", "ReplaceableTextures\\CommandButtons\\BTNCriticalStrike", "");
            InitData(5, "Speed: ", "ReplaceableTextures\\CommandButtons\\BTNBootsOfSpeed", "The unit's current movespeed");
            InitData(6, "Str: ", "ReplaceableTextures\\CommandButtons\\BTNGauntletsOfOgrePower", "Increases Life and Life regeneration");
            InitData(7, "Agi: ", "ReplaceableTextures\\CommandButtons\\BTNSlippersOfAgility", "Improves Armor and Attack speed");
            InitData(8, "Int: ", "ReplaceableTextures\\CommandButtons\\BTNMantleOfIntelligence", "Improves Mana and Mana regeneration");
            InitData(9, "Power: ", "ReplaceableTextures\\CommandButtons\\BTNControlMagic", "Makes most abilities better");
            InitData(10, "Hp/s: ", "ReplaceableTextures\\CommandButtons\\BTNRegenerate", "");
            InitData(11, "Mp/s: ", "ReplaceableTextures\\CommandButtons\\BTNMagicalSentry", "");
            InitData(12, "Evasion: ", "ReplaceableTextures\\CommandButtons\\BTNEvasion", "");
        }

        public static bool TooltipAction()
        {
            int i = buttonCount;
            while (i > 0)
            {
                if (UnitInfoPanels.UnitInfoTooltipFrame == ToolTip[i])
                {
                    UnitInfoPanels.UnitInfoTooltipText = DataLabel[i] + BlzFrameGetText(Text[i]) + "\n" + DataDesc[i];
                    break;
                }
                i--;
            }
            return true;
        }

        public static bool Condition()
        {
            return IsUnitType(UnitInfoPanels.UnitInfoUnit, UNIT_TYPE_HERO);
        }

        public static void ButtonAction()
        {
            int i = buttonCount;
            framehandle frame = BlzGetTriggerFrame();
            UnitInfoPanels.UnitInfoGetUnit(GetTriggerPlayer());

            while (i > 0)
            {
                if (frame == Button[i])
                {
                    BJDebugMsg(GetPlayerName(GetTriggerPlayer()) + " Clicked: " + I2S(i) + " " + GetUnitName(UnitInfoPanels.UnitInfoUnit));
                    return;
                }
                i--;
            }

            frame = null;
        }

        public static bool Update()
        {
            Console.WriteLine("Custom stat panel update function running!");
            BlzFrameSetText(Text[1], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconValue", 0)));
            BlzFrameSetText(Text[2], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconValue", 2)));
            BlzFrameSetText(Text[3], "0");
            BlzFrameSetText(Text[4], "0");
            BlzFrameSetText(Text[5], I2S(R2I(GetUnitMoveSpeed(UnitInfoPanels.UnitInfoUnit))));
            BlzFrameSetText(Text[6], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconHeroStrengthValue", 6)));
            BlzFrameSetText(Text[7], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconHeroAgilityValue", 6)));
            BlzFrameSetText(Text[8], BlzFrameGetText(BlzGetFrameByName("InfoPanelIconHeroIntellectValue", 6)));
            BlzFrameSetText(Text[10], R2SW(BlzGetUnitRealField(UnitInfoPanels.UnitInfoUnit, UNIT_RF_HIT_POINTS_REGENERATION_RATE), 1, 1));
            BlzFrameSetText(Text[11], R2SW(BlzGetUnitRealField(UnitInfoPanels.UnitInfoUnit, UNIT_RF_MANA_REGENERATION), 1, 1));
            return true;
        }

        public static void At0s()
        {
            Console.WriteLine("Custom stat panel At0s function running!");
            framehandle prevIcon;
            int i = buttonCount;

            BlzGetFrameByName("InfoPanelIconValue", 0);
            BlzGetFrameByName("InfoPanelIconValue", 2);
            BlzGetFrameByName("InfoPanelIconHeroStrengthValue", 6);
            BlzGetFrameByName("InfoPanelIconHeroAgilityValue", 6);
            BlzGetFrameByName("InfoPanelIconHeroIntellectValue", 6);

            parent = BlzCreateSimpleFrame("CustomUnitInfoPanel3x4", BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0), 0);
            UnitInfoPanels.AddUnitInfoPanel(parent, Update, Condition);

            while (i > 0)
            {
                Button[i] = BlzGetFrameByName("CustomUnitInfoButton" + I2S(i), 0);
                ToolTip[i] = BlzCreateFrameByType("SIMPLEFRAME", "", Button[i], "", 0);
                Icon[i] = BlzGetFrameByName("CustomUnitInfoButtonIcon" + I2S(i), 0);
                Text[i] = BlzGetFrameByName("CustomUnitInfoButtonText" + I2S(i), 0);
                BlzTriggerRegisterFrameEvent(buttonTrigger, Button[i], FRAMEEVENT_CONTROL_CLICK);
                BlzFrameSetTooltip(Button[i], ToolTip[i]);
                BlzFrameSetVisible(ToolTip[i], false);
                BlzFrameSetTexture(Icon[i], DataIcon[i], 0, false);

                UnitInfoPanels.UnitInfoPanelAddTooltipListener(ToolTip[i], TooltipAction);
                i--;
            }
        }

        private static void InitData(int index, string label, string icon, string desc)
        {
            DataIcon[index] = icon;
            DataLabel[index] = label;
            DataDesc[index] = desc;
        }
    }
}