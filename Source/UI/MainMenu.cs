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
    public class MainMenu : CustomUIMenu
    {
        private framehandle _mainMenuBackdrop = null;
        private trigger _triggerMainMenuBackdrop = null;
        private framehandle _saveLoadButton = null;
        private trigger _triggerSaveLoadButton = null;
        private framehandle _button2 = null;
        private trigger _triggerButton2 = null;
        private framehandle _button3 = null;
        private trigger _triggerButton3 = null;
        private framehandle _button4 = null;
        private trigger _triggerButton4 = null;
        private framehandle _exitButton = null;
        private trigger _triggerExitButton = null;

        public override void Init()
        {
            CreateMenu();
        }

        protected override void CreateMenu()
        {
            if (!IsUICreated)
            {
                _mainMenuBackdrop = BlzCreateFrame("QuestButtonBaseTemplate", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), 0, 0);
                BlzFrameSetAbsPoint(_mainMenuBackdrop, FRAMEPOINT_TOPLEFT, 0.200000f, 0.550000f);
                BlzFrameSetAbsPoint(_mainMenuBackdrop, FRAMEPOINT_BOTTOMRIGHT, 0.600000f, 0.170000f);

                _saveLoadButton = BlzCreateFrame("ScriptDialogButton", _mainMenuBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_saveLoadButton, FRAMEPOINT_TOPLEFT, 0.220000f, 0.530000f);
                BlzFrameSetAbsPoint(_saveLoadButton, FRAMEPOINT_BOTTOMRIGHT, 0.580000f, 0.470000f);
                BlzFrameSetText(_saveLoadButton, "|cffFCD20DSave/Load|r");
                BlzFrameSetScale(_saveLoadButton, 1.00f);
                _triggerSaveLoadButton = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerSaveLoadButton, _saveLoadButton, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerSaveLoadButton, OpenSaveLoadMenu);

                _button2 = BlzCreateFrame("ScriptDialogButton", _mainMenuBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_button2, FRAMEPOINT_TOPLEFT, 0.220000f, 0.460000f);
                BlzFrameSetAbsPoint(_button2, FRAMEPOINT_BOTTOMRIGHT, 0.580000f, 0.400000f);
                BlzFrameSetText(_button2, "|cffFCD20DN/A|r");
                BlzFrameSetScale(_button2, 1.00f);
                _triggerButton2 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerButton2, _button2, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerButton2, EmptyFunction);

                _button3 = BlzCreateFrame("ScriptDialogButton", _mainMenuBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_button3, FRAMEPOINT_TOPLEFT, 0.220000f, 0.390000f);
                BlzFrameSetAbsPoint(_button3, FRAMEPOINT_BOTTOMRIGHT, 0.580000f, 0.330000f);
                BlzFrameSetText(_button3, "|cffFCD20DN/A|r");
                BlzFrameSetScale(_button3, 1.00f);
                _triggerButton3 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerButton3, _button3, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerButton3, EmptyFunction);

                _button4 = BlzCreateFrame("ScriptDialogButton", _mainMenuBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_button4, FRAMEPOINT_TOPLEFT, 0.220000f, 0.320000f);
                BlzFrameSetAbsPoint(_button4, FRAMEPOINT_BOTTOMRIGHT, 0.580000f, 0.260000f);
                BlzFrameSetText(_button4, "|cffFCD20DN/A|r");
                BlzFrameSetScale(_button4, 1.00f);
                _triggerButton4 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerButton4, _button4, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerButton4, EmptyFunction);

                _exitButton = BlzCreateFrame("ScriptDialogButton", _mainMenuBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_exitButton, FRAMEPOINT_TOPLEFT, 0.220000f, 0.250000f);
                BlzFrameSetAbsPoint(_exitButton, FRAMEPOINT_BOTTOMRIGHT, 0.580000f, 0.190000f);
                BlzFrameSetText(_exitButton, "|cffFCD20DExit|r");
                BlzFrameSetScale(_exitButton, 1.00f);
                _triggerExitButton = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerExitButton, _exitButton, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerExitButton, CloseMainMenu);

                BaseFrameHandle = _mainMenuBackdrop;
                IsUICreated = true;
            }
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
        }

        private void OpenSaveLoadMenu()
        {
            int playerId = GetPlayerId(GetTriggerPlayer());
            UIManager.OpenMenu(playerId, CustomUIMenuType.SaveLoadMenu);
            base.ResetFrameFocus();
        }

        private void CloseMainMenu()
        {
            int playerId = GetPlayerId(GetTriggerPlayer());
            base.ResetFrameFocus();
            UIManager.CloseMenu(playerId, BaseFrameHandle);
        }

        private void EmptyFunction()
        {
            base.ResetFrameFocus();
        }
    }
}
