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
    public class SaveLoadMenu : CustomUIMenu
    {
        private framehandle _saveLoadBackdrop = null;
        private framehandle _triggerSaveLoadBackdrop = null;
        private framehandle _exitButton = null;
        private trigger _triggerExitButton = null;
        private framehandle _slotButton01 = null;
        private trigger _triggerSlotButton01 = null;
        private framehandle _slotButton02 = null;
        private trigger _triggerSlotButton02 = null;
        private framehandle _slotButton03 = null;
        private trigger _triggerSlotButton03 = null;
        private framehandle _slotButton04 = null;
        private trigger _triggerSlotButton04 = null;
        private framehandle _slotButton05 = null;
        private trigger _triggerSlotButton05 = null;
        private framehandle _previousSlotsButton = null;
        private trigger _triggerPreviousSlotsButton = null;
        private framehandle _nextSlotsButton = null;
        private trigger _triggerNextSlotsButton = null;
        private framehandle _saveButton = null;
        private trigger _triggerSaveButton = null;
        private framehandle _loadButton = null;
        private trigger _triggerLoadButton = null;
        private framehandle _slotIndexBackdrop = null;
        private trigger _triggerSlotIndexBackdrop = null;
        private framehandle _saveInformationBackdrop = null;
        private trigger _triggerSaveInformationBackdrop = null;
        private framehandle _currentSlotIndexText = null;
        private framehandle _triggerCurrentSlotIndexText = null;
        private framehandle _saveInformationText = null;
        private framehandle _triggerSaveInformationText = null;
        private Action _exitButtonFunc = null;
        private Action _slotButton01Func = null;
        private Action _slotButton02Func = null;
        private Action _slotButton03Func = null;
        private Action _slotButton04Func = null;
        private Action _slotButton05Func = null;
        private Action _previousSlotsButtonFunc = null;
        private Action _nextSlotsButtonFunc = null;
        private Action _saveButtonFunc = null;
        private Action _loadButtonFunc = null;

        private const string _textColor = "|cffFFCC00";
        private const string _emptySaveMessage = "Save slot is empty.";
        private const string _defaultSaveInfoMessage = "Please select a save slot before using the save and load buttons.";

        private List<framehandle> _saveSlotButtons = null;
        private Dictionary<int, SaveLoadMenuData> _playerMenuDatas = null;

        public override void Init()
        {
            SetTriggerFunctions();
            CreateMenu();
            CreateMenuData();
            InitializeSaveSlotButtons();
        }

        protected override void CreateMenu()
        {
            if (!IsUICreated)
            {
                _saveLoadBackdrop = BlzCreateFrame("QuestButtonBaseTemplate", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), 0, 0);
                BlzFrameSetAbsPoint(_saveLoadBackdrop, FRAMEPOINT_TOPLEFT, 0.0200000f, 0.550000f);
                BlzFrameSetAbsPoint(_saveLoadBackdrop, FRAMEPOINT_BOTTOMRIGHT, 0.780000f, 0.170000f);

                _exitButton = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_exitButton, FRAMEPOINT_TOPLEFT, 0.650000f, 0.535000f);
                BlzFrameSetAbsPoint(_exitButton, FRAMEPOINT_BOTTOMRIGHT, 0.750000f, 0.485000f);
                BlzFrameSetText(_exitButton, "|cffFCD20DExit|r");
                BlzFrameSetScale(_exitButton, 1.00f);
                _triggerExitButton = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerExitButton, _exitButton, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerExitButton, _exitButtonFunc);

                _slotButton01 = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_slotButton01, FRAMEPOINT_TOPLEFT, 0.0500000f, 0.535000f);
                BlzFrameSetAbsPoint(_slotButton01, FRAMEPOINT_BOTTOMRIGHT, 0.190000f, 0.485000f);
                BlzFrameSetText(_slotButton01, "|cffFCD20DSlot X|r");
                BlzFrameSetScale(_slotButton01, 1.00f);
                _triggerSlotButton01 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerSlotButton01, _slotButton01, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerSlotButton01, _slotButton01Func);

                _slotButton02 = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_slotButton02, FRAMEPOINT_TOPLEFT, 0.0500000f, 0.475000f);
                BlzFrameSetAbsPoint(_slotButton02, FRAMEPOINT_BOTTOMRIGHT, 0.190000f, 0.425000f);
                BlzFrameSetText(_slotButton02, "|cffFCD20DSlot X|r");
                BlzFrameSetScale(_slotButton02, 1.00f);
                _triggerSlotButton02 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerSlotButton02, _slotButton02, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerSlotButton02, _slotButton02Func);

                _slotButton03 = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_slotButton03, FRAMEPOINT_TOPLEFT, 0.0500000f, 0.415000f);
                BlzFrameSetAbsPoint(_slotButton03, FRAMEPOINT_BOTTOMRIGHT, 0.190000f, 0.365000f);
                BlzFrameSetText(_slotButton03, "|cffFCD20DSlot X|r");
                BlzFrameSetScale(_slotButton03, 1.00f);
                _triggerSlotButton03 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerSlotButton03, _slotButton03, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerSlotButton03, _slotButton03Func);

                _slotButton04 = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_slotButton04, FRAMEPOINT_TOPLEFT, 0.0500000f, 0.355000f);
                BlzFrameSetAbsPoint(_slotButton04, FRAMEPOINT_BOTTOMRIGHT, 0.190000f, 0.305000f);
                BlzFrameSetText(_slotButton04, "|cffFCD20DSlot X|r");
                BlzFrameSetScale(_slotButton04, 1.00f);
                _triggerSlotButton04 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerSlotButton04, _slotButton04, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerSlotButton04, _slotButton04Func);

                _slotButton05 = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_slotButton05, FRAMEPOINT_TOPLEFT, 0.0500000f, 0.295000f);
                BlzFrameSetAbsPoint(_slotButton05, FRAMEPOINT_BOTTOMRIGHT, 0.190000f, 0.245000f);
                BlzFrameSetText(_slotButton05, "|cffFCD20DSlot X|r");
                BlzFrameSetScale(_slotButton05, 1.00f);
                _triggerSlotButton05 = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerSlotButton05, _slotButton05, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerSlotButton05, _slotButton05Func);

                _previousSlotsButton = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_previousSlotsButton, FRAMEPOINT_TOPLEFT, 0.0500000f, 0.235000f);
                BlzFrameSetAbsPoint(_previousSlotsButton, FRAMEPOINT_BOTTOMRIGHT, 0.115000f, 0.185000f);
                BlzFrameSetText(_previousSlotsButton, "|cffFCD20D<|r");
                BlzFrameSetScale(_previousSlotsButton, 2.43f);
                _triggerPreviousSlotsButton = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerPreviousSlotsButton, _previousSlotsButton, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerPreviousSlotsButton, _previousSlotsButtonFunc);

                _nextSlotsButton = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_nextSlotsButton, FRAMEPOINT_TOPLEFT, 0.125000f, 0.235000f);
                BlzFrameSetAbsPoint(_nextSlotsButton, FRAMEPOINT_BOTTOMRIGHT, 0.190000f, 0.185000f);
                BlzFrameSetText(_nextSlotsButton, "|cffFCD20D>|r");
                BlzFrameSetScale(_nextSlotsButton, 2.43f);
                _triggerNextSlotsButton = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerNextSlotsButton, _nextSlotsButton, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerNextSlotsButton, _nextSlotsButtonFunc);

                _saveButton = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_saveButton, FRAMEPOINT_TOPLEFT, 0.350000f, 0.535000f);
                BlzFrameSetAbsPoint(_saveButton, FRAMEPOINT_BOTTOMRIGHT, 0.490000f, 0.485000f);
                BlzFrameSetText(_saveButton, "|cffFCD20DSave|r");
                BlzFrameSetScale(_saveButton, 1.00f);
                _triggerSaveButton = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerSaveButton, _saveButton, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerSaveButton, _saveButtonFunc);

                _loadButton = BlzCreateFrame("ScriptDialogButton", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_loadButton, FRAMEPOINT_TOPLEFT, 0.500000f, 0.535000f);
                BlzFrameSetAbsPoint(_loadButton, FRAMEPOINT_BOTTOMRIGHT, 0.640000f, 0.485000f);
                BlzFrameSetText(_loadButton, "|cffFCD20DLoad|r");
                BlzFrameSetScale(_loadButton, 1.00f);
                _triggerLoadButton = CreateTrigger();
                BlzTriggerRegisterFrameEvent(_triggerLoadButton, _loadButton, FRAMEEVENT_CONTROL_CLICK);
                TriggerAddAction(_triggerLoadButton, _loadButtonFunc);

                _slotIndexBackdrop = BlzCreateFrame("EscMenuBackdrop", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_slotIndexBackdrop, FRAMEPOINT_TOPLEFT, 0.200000f, 0.535000f);
                BlzFrameSetAbsPoint(_slotIndexBackdrop, FRAMEPOINT_BOTTOMRIGHT, 0.340000f, 0.485000f);

                _saveInformationBackdrop = BlzCreateFrame("EscMenuBackdrop", _saveLoadBackdrop, 0, 0);
                BlzFrameSetAbsPoint(_saveInformationBackdrop, FRAMEPOINT_TOPLEFT, 0.200000f, 0.475000f);
                BlzFrameSetAbsPoint(_saveInformationBackdrop, FRAMEPOINT_BOTTOMRIGHT, 0.750000f, 0.185000f);

                _currentSlotIndexText = BlzCreateFrameByType("TEXT", "name", _slotIndexBackdrop, "", 0);
                BlzFrameSetAbsPoint(_currentSlotIndexText, FRAMEPOINT_TOPLEFT, 0.200000f, 0.530000f);
                BlzFrameSetAbsPoint(_currentSlotIndexText, FRAMEPOINT_BOTTOMRIGHT, 0.320000f, 0.495000f);
                BlzFrameSetText(_currentSlotIndexText, "|cffFCD20DCurrent Slot: X|r");
                BlzFrameSetEnable(_currentSlotIndexText, false);
                BlzFrameSetScale(_currentSlotIndexText, 1.00f);
                BlzFrameSetTextAlignment(_currentSlotIndexText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_MIDDLE);

                _saveInformationText = BlzCreateFrameByType("TEXT", "name", _saveInformationBackdrop, "", 0);
                BlzFrameSetAbsPoint(_saveInformationText, FRAMEPOINT_TOPLEFT, 0.260000f, 0.440000f);
                BlzFrameSetAbsPoint(_saveInformationText, FRAMEPOINT_BOTTOMRIGHT, 0.695000f, 0.220000f);
                BlzFrameSetText(_saveInformationText, _defaultSaveInfoMessage);
                BlzFrameSetEnable(_saveInformationText, false);
                BlzFrameSetScale(_saveInformationText, 1.00f);
                BlzFrameSetTextAlignment(_saveInformationText, TEXT_JUSTIFY_CENTER, TEXT_JUSTIFY_MIDDLE);

                _saveSlotButtons = new List<framehandle> { _slotButton01, _slotButton02, _slotButton03, _slotButton04, _slotButton05 };

                BaseFrameHandle = _saveLoadBackdrop;
                IsUICreated = true;
            }
        }

        private void SetTriggerFunctions()
        {
            _exitButtonFunc = CloseSaveLoadMenu;
            _slotButton01Func = SelectSaveSlot;
            _slotButton02Func = SelectSaveSlot;
            _slotButton03Func = SelectSaveSlot;
            _slotButton04Func = SelectSaveSlot;
            _slotButton05Func = SelectSaveSlot;
            _saveButtonFunc = SaveToSlot;
            _loadButtonFunc = LoadFromSlot;
            _nextSlotsButtonFunc = SwitchToNextSaveSlotPage;
            _previousSlotsButtonFunc = SwitchToPreviousSaveSlotPage;
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
        }

        private void CloseSaveLoadMenu()
        {
            int playerId = GetPlayerId(GetTriggerPlayer());
            base.ResetFrameFocus();
            UIManager.CloseMenu(playerId, BaseFrameHandle);
        }

        private void CreateMenuData()
        {
            _playerMenuDatas = new Dictionary<int, SaveLoadMenuData>();
            int playerId = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerId = PlayerManager.Players.ElementAt(i).Key;
                _playerMenuDatas.Add(playerId, new SaveLoadMenuData());
            }
        }

        private void InitializeSaveSlotButtons()
        {
            int playerId = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerId = PlayerManager.Players.ElementAt(i).Key;
                //ClearSaveSlotButtons();
                UpdateSaveSlotButtons(playerId);
                // Select first save slot for player.
                //InitializeSaveSlotIndexData(PlayerManager.players[i], i);
            }
        }

        //private void ClearSaveSlotButtons()
        //      {
        //          for (int i = 0; i < _saveSlotButtons.Count; i++)
        //          {
        //              BlzFrameSetVisible(_saveSlotButtons[i], false);
        //          }
        //      }

        // Call on menu Init and page change.
        private void UpdateSaveSlotButtons(int playerId)
        {
            string tmpMenuText = "";
            for (int i = 0; i < _saveSlotButtons.Count; i++)
            {
                tmpMenuText = _textColor + "Slot - " + (i + 1 + _playerMenuDatas[playerId].SaveSlotPage * _saveSlotButtons.Count);
                UIManager.SetMenuText(playerId, _saveSlotButtons[i], tmpMenuText);
                //BlzFrameSetText(_saveSlotButtons[i], _textColor + "Slot - " + ((i + 1) + (_playerMenuDatas[playerId].SaveSlotPage * _saveSlotButtons.Count)));
            }
        }

        private void UpdateSaveSlotIndexText(int playerId)
        {
            string tmpMenuText = "Current Slot: " + _playerMenuDatas[playerId].SaveSlotIndex.ToString();
            UIManager.SetMenuText(playerId, _currentSlotIndexText, tmpMenuText);
            //BlzFrameSetText(_currentSlotIndexText, _textColor + "Current Slot: " + _playerMenuDatas[playerId].SaveSlotIndex.ToString());
        }

        // Call when slot is selected.
        // Populate save information text from selected save slot data.
        private void UpdateSaveInformation(player targetPlayer, int playerId)
        {
            // Get all save slots for player.
            // Populate save slot ui elements.
            // Populate save info elements for selected save slot.
            // Subtract 1 from save slot index as menu data uses zero origin.
            int playerSaveIndex = _playerMenuDatas[playerId].SaveSlotIndex - 1;
            SaveData tmpSave = SaveManager.SavesByPlayer[targetPlayer][playerSaveIndex];
            string tmpDataString = "";
            if (!tmpSave.IsPopulated)
            {
                tmpDataString = _emptySaveMessage;
            }
            else
            {
                tmpDataString = _textColor + "Save Slot: " + tmpSave.GetSaveSlot() + "\nHero ID: " + tmpSave.HeroUnitData.UnitTypeId +
                    "\nLevel: " + tmpSave.HeroUnitData.Level + "\nExperience: " + tmpSave.HeroUnitData.Experience + "\nStrength: " + tmpSave.HeroUnitData.Strength +
                    "\nAgility: " + tmpSave.HeroUnitData.Agility + "\nIntelligence: " + tmpSave.HeroUnitData.Intelligence +
                    "\nGold: " + tmpSave.ResourceSaveData.Gold + "\nLumber: " + tmpSave.ResourceSaveData.Lumber;
            }
            UIManager.SetMenuText(playerId, _saveInformationText, tmpDataString);
            //BlzFrameSetText(_saveInformationText, tmpDataString);
        }

        private int GetButtonSlotIndex(int playerId)
        {
            framehandle triggerButton = BlzGetTriggerFrame();
            // Get element index of button frame.
            for (int i = 0; i < _saveSlotButtons.Count; i++)
            {
                if (_saveSlotButtons[i] == triggerButton)
                {
                    //add 1 to index since save slots use 1 as origin
                    return i + 1 + _playerMenuDatas[playerId].SaveSlotPage * _saveSlotButtons.Count;
                }
            }
            return 0;
        }

        //CALL WHEN SLOT BUTTON IS CLICKED
        private void SelectSaveSlot()
        {
            player triggerPlayer = GetTriggerPlayer();
            int playerId = GetPlayerId(triggerPlayer);
            _playerMenuDatas[playerId].SaveSlotIndex = GetButtonSlotIndex(playerId);
            UpdateSaveSlotIndexText(playerId);
            UpdateSaveInformation(triggerPlayer, playerId);
            base.ResetFrameFocus();
        }

        //// Selects 1st save slot for player on initialization
        //private void InitializeSaveSlotIndexData(player targetPlayer, int playerId)
        //      {
        //	_playerMenuDatas[playerId].SaveSlotIndex = 1;
        //	UpdateSaveSlotIndexText(playerId);
        //	UpdateSaveInformation(targetPlayer, playerId);
        //      }

        //CALL WHEN SAVE BUTTON IS CLICKED
        private void SaveToSlot()
        {
            // Call save function of SaveManager using current slot index.
            player triggerPlayer = GetTriggerPlayer();
            int playerId = GetPlayerId(triggerPlayer);
            int playerSaveIndex = _playerMenuDatas[playerId].SaveSlotIndex - 1;
            SaveData tmpSave = SaveManager.SavesByPlayer[triggerPlayer][playerSaveIndex];
            if (tmpSave == null)
            {
                Console.WriteLine(_defaultSaveInfoMessage);
                return;
            }
            SaveManager.SavePlayerData(triggerPlayer, playerId, _playerMenuDatas[playerId].SaveSlotIndex);
            UpdateSaveInformation(triggerPlayer, playerId);
            base.ResetFrameFocus();
        }

        //CALL WHEN LOAD BUTTON IS CLICKED
        private void LoadFromSlot()
        {
            player triggerPlayer = GetTriggerPlayer();
            int playerId = GetPlayerId(triggerPlayer);
            int playerSaveIndex = _playerMenuDatas[playerId].SaveSlotIndex - 1;
            SaveData tmpSave = SaveManager.SavesByPlayer[triggerPlayer][playerSaveIndex];
            if (tmpSave == null)
            {
                Console.WriteLine(_defaultSaveInfoMessage);
                return;
            }
            SaveManager.LoadPlayerData(triggerPlayer, playerId, _playerMenuDatas[playerId].SaveSlotIndex);
            base.ResetFrameFocus();
        }

        private void SwitchToPreviousSaveSlotPage()
        {
            int playerId = GetPlayerId(GetTriggerPlayer());
            if (_playerMenuDatas[playerId].SaveSlotPage > 0)
            {
                _playerMenuDatas[playerId].SaveSlotPage--;
            }
            UpdateSaveSlotButtons(playerId);
            base.ResetFrameFocus();
        }

        private void SwitchToNextSaveSlotPage()
        {
            int playerId = GetPlayerId(GetTriggerPlayer());
            if (_playerMenuDatas[playerId].SaveSlotPage < 19)
            {
                _playerMenuDatas[playerId].SaveSlotPage++;
            }
            UpdateSaveSlotButtons(playerId);
            base.ResetFrameFocus();
        }
    }
}