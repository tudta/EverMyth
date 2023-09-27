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
    public static class UnitInfoPanelManager
    {
        private static int _currentUnitInfoPanelIndex = 0;
        private static int _unitInfoPanelCount = 0;
        private static int _tooltipListenerCount = 0;

        private static framehandle _standardUnitInfoPanel = null;
        private static framehandle _unitInfoPanelParent = null;
        private static framehandle _pageUpButton = null;
        private static framehandle _pageDownButton = null;
        private static trigger _pageArrowButtonTrigger = null;
        private static framehandle _tooltipBox = null;
        private static framehandle _tooltipText = null;

        private static List<framehandle> _unitInfoPanels = null;
        private static List<framehandle> _unitInfoPanelFrames = null;
        private static List<trigger> _unitInfoPanelConditions = null;
        private static List<trigger> _unitInfoPanelUpdateActions = null;

        private static List<framehandle> _tooltipListeners = null;
        private static List<trigger> _tooltipListenerActions = null;
        private static framehandle _tooltipFrame = null;
        private static string _tooltipTextValue = "";

        private static int _createContext = 1000;
        private static framehandle _unitInfoInfoFrame = null;
        private static framehandle _unitInfoIconFrame = null;
        private static framehandle _unitInfoLabelFrame = null;
        private static framehandle _unitInfoTextFrame = null;

        private static group _selectedUnitGroup = null;
        private static unit _selectedUnit = null;

        private static bool _tooltipFound = false;

        public static unit SelectedUnit
        {
            get
            {
                return _selectedUnit;
            }

            set
            {
                _selectedUnit = value;
            }
        }

        public static framehandle TooltipFrame
        {
            get
            {
                return _tooltipFrame;
            }

            set
            {
                _tooltipFrame = value;
            }
        }

        public static string TooltipTextValue
        {
            get
            {
                return _tooltipTextValue;
            }

            set
            {
                _tooltipTextValue = value;
            }
        }

        public static void Init()
        {
            Console.WriteLine("UnitInfoPanelManger.Init");
            BlzLoadTOCFile("war3mapImported\\UnitInfoPanels.toc");
            InitializeMembers();
            CreateUnitInfoPanelBaseElements();
            CreatePageChangeButtons();
            HideDefaultUnitInfoPanel();
            CreateUnitInfoPanelTooltipElements();
            TimerStart(CreateTimer(), 0.5f, true, Update);
        }

        private static void InitializeMembers()
        {
            _unitInfoPanels = new List<framehandle>();
            _unitInfoPanelFrames = new List<framehandle>();
            _unitInfoPanelConditions = new List<trigger>();
            _unitInfoPanelUpdateActions = new List<trigger>();
            _tooltipListeners = new List<framehandle>();
            _tooltipListenerActions = new List<trigger>();
            _selectedUnitGroup = CreateGroup();
    }

        private static void Update()
        {
            //Console.WriteLine("Info panel update function: start!");
            _tooltipFound = false;
            GetSelectedUnit(GetLocalPlayer());
            if (BlzFrameIsVisible(_standardUnitInfoPanel))
            {
                //Console.WriteLine("Info panel update function: info panel visible!");
                for (int i = 0; i < _tooltipListenerCount; i++)
                {
                    if (BlzFrameIsVisible(_tooltipListeners[i]))
                    {
                        _tooltipFrame = _tooltipListeners[i];
                        TriggerEvaluate(_tooltipListenerActions[i]);
                        BlzFrameSetText(_tooltipText, _tooltipTextValue);
                        _tooltipFound = true;
                        break;
                    }
                }
                BlzFrameSetVisible(_unitInfoPanels[_currentUnitInfoPanelIndex], true);
                TriggerEvaluate(_unitInfoPanelUpdateActions[_currentUnitInfoPanelIndex]);
            }
            else
            {
                //Console.WriteLine("Info panel update function: info panel hidden!");
                BlzFrameSetVisible(_unitInfoPanels[_currentUnitInfoPanelIndex], false);
            }
            BlzFrameSetVisible(_tooltipBox, _tooltipFound);
        }

        private static void CreateUnitInfoPanelBaseElements()
        {
            _standardUnitInfoPanel = BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0);
            _unitInfoPanelParent = BlzCreateFrameByType("SIMPLEFRAME", "", _standardUnitInfoPanel, "", 0);
        }

        private static void CreatePageChangeButtons()
        {
            _pageUpButton = BlzCreateSimpleFrame("UnitInfoSimpleIconButtonUp", _standardUnitInfoPanel, 0);
            _pageDownButton = BlzCreateSimpleFrame("UnitInfoSimpleIconButtonDown", _standardUnitInfoPanel, 0);
            BlzFrameSetAbsPoint(_pageUpButton, FRAMEPOINT_BOTTOMRIGHT, 0.51f, 0.08f);
            _pageArrowButtonTrigger = CreateTrigger();
            BlzTriggerRegisterFrameEvent(_pageArrowButtonTrigger, _pageUpButton, FRAMEEVENT_CONTROL_CLICK);
            BlzTriggerRegisterFrameEvent(_pageArrowButtonTrigger, _pageDownButton, FRAMEEVENT_CONTROL_CLICK);
            TriggerAddAction(_pageArrowButtonTrigger, PageButtonAction);
        }

        private static void HideDefaultUnitInfoPanel()
        {
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconDamage", 0), _unitInfoPanelParent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconDamage", 1), _unitInfoPanelParent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconArmor", 2), _unitInfoPanelParent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconRank", 3), _unitInfoPanelParent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconFood", 4), _unitInfoPanelParent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconGold", 5), _unitInfoPanelParent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconHero", 6), _unitInfoPanelParent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconAlly", 7), _unitInfoPanelParent);
            BlzFrameSetVisible(_unitInfoPanelParent, false);
        }

        private static void CreateUnitInfoPanelTooltipElements()
        {
            _tooltipBox = BlzCreateFrame("CustomUnitInfoTextBox", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), 0, 0);
            _tooltipText = BlzCreateFrame("CustomUnitInfoText", _tooltipBox, 0, 0);

            BlzFrameSetAbsPoint(_tooltipText, FRAMEPOINT_BOTTOMRIGHT, 0.79f, 0.18f);
            BlzFrameSetSize(_tooltipText, 0.275f, 0f);
            BlzFrameSetPoint(_tooltipBox, FRAMEPOINT_TOPLEFT, _tooltipText, FRAMEPOINT_TOPLEFT, -0.01f, 0.01f);
            BlzFrameSetPoint(_tooltipBox, FRAMEPOINT_BOTTOMRIGHT, _tooltipText, FRAMEPOINT_BOTTOMRIGHT, 0.005f, -0.01f);
            BlzFrameSetVisible(_tooltipBox, false);        
        }

        private static void SetUnitInfoPage(int infoPanelIndex)
        {
            //Console.WriteLine("Desired panel index: " + infoPanelIndex);
            //Console.WriteLine("Panel count: " + _unitInfoPanels.Count);
            //Console.WriteLine("Frame count: " + _unitInfoPanelFrames.Count);
            BlzFrameSetVisible(_unitInfoPanels[_currentUnitInfoPanelIndex], false);
            //BlzFrameSetVisible(_unitInfoPanelFrames[_currentUnitInfoPanelIndex], false);
            if (infoPanelIndex < 0)
            {
                infoPanelIndex = _unitInfoPanelCount - 1;
            }
            if (infoPanelIndex >= _unitInfoPanelCount)
            {
                infoPanelIndex = 0;
            }
            //Console.WriteLine("Clamped panel index: " + infoPanelIndex);
            _currentUnitInfoPanelIndex = infoPanelIndex;
            //Console.WriteLine("Setting unit info panel to page " + _currentUnitInfoPanelIndex + "!");
            BlzFrameSetVisible(_unitInfoPanels[_currentUnitInfoPanelIndex], true);
            //BlzFrameSetVisible(_unitInfoPanelFrames[_currentUnitInfoPanelIndex], true);
        }

        private static void SetUnitInfoPanelPageByFrame(framehandle newPage, bool updateWanted)
        {
            for (int i = 0; i < _unitInfoPanelCount; i++)
            {
                if (_unitInfoPanels[i] == newPage || _unitInfoPanelFrames[i] == newPage)
                {
                    SetUnitInfoPage(i);
                }
            }
        }

        private static void SwitchToPreviousUnitInfoPage()
        {
            SetUnitInfoPage(_currentUnitInfoPanelIndex - 1);
        }

        private static void SwitchToNextUnitInfoPage()
        {
            SetUnitInfoPage(_currentUnitInfoPanelIndex + 1);
        }

        private static void PageButtonAction()
        {
            if (GetTriggerPlayer() == GetLocalPlayer())
            {
                if (BlzGetTriggerFrame() == _pageUpButton)
                {
                    SwitchToPreviousUnitInfoPage();
                }
                else
                {
                    SwitchToNextUnitInfoPage();
                }
            }
        }
        public static void GetSelectedUnit(player targetPlayer)
        {
            GroupEnumUnitsSelected(_selectedUnitGroup, targetPlayer, null);
            _selectedUnit = FirstOfGroup(_selectedUnitGroup);
            GroupClear(_selectedUnitGroup);
        }

        public static void AddUnitInfoPanel(framehandle unitInfoPanel, Func<bool> updateFunction, Func<bool> condition)
        {
            _unitInfoPanelCount++;
            BlzFrameSetParent(unitInfoPanel, _standardUnitInfoPanel);
            _unitInfoPanels.Add(unitInfoPanel);
            _unitInfoPanelConditions.Add(CreateTrigger());
            if (condition != null)
            {
                TriggerAddCondition(_unitInfoPanelConditions[_unitInfoPanelCount - 1], Filter(condition));
            }
            if (updateFunction != null)
            {
                _unitInfoPanelUpdateActions.Add(CreateTrigger());
                TriggerAddCondition(_unitInfoPanelUpdateActions[_unitInfoPanelCount - 1], Filter(updateFunction));
            }
            BlzFrameSetVisible(unitInfoPanel, _currentUnitInfoPanelIndex == _unitInfoPanelCount - 1);
        }

        public static framehandle AddUnitInfoPanelEx(Func<bool> updateFunction, Func<bool> condition)
        {
            framehandle frame = BlzCreateFrameByType("SIMPLEFRAME", "", _standardUnitInfoPanel, "", 0);
            AddUnitInfoPanel(frame, updateFunction, condition);
            return frame;
        }

        public static void SetUnitInfoPanelFrame(framehandle frame)
        {
            _unitInfoPanelFrames.Add(frame);
            BlzFrameSetVisible(frame, BlzFrameIsVisible(_unitInfoPanels[_unitInfoPanelCount - 1]));
        }

        public static framehandle SetUnitInfoPanelFrameEx()
        {
            framehandle frame = BlzCreateFrameByType("FRAME", "", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), "", 0);
            SetUnitInfoPanelFrame(frame);
            return frame;
        }

        public static void UnitInfoPanelAddTooltipListener(framehandle frame, Func<bool> tooltipAction)
        {
            _tooltipListenerCount++;
            _tooltipListeners.Add(frame);
            _tooltipListenerActions.Add(CreateTrigger());
            TriggerAddCondition(_tooltipListenerActions[_tooltipListenerCount - 1], Filter(tooltipAction));
        }

        public static framehandle UnitInfoAddTooltip(framehandle parent, framehandle frame)
        {
            framehandle toolTip = null;
            framehandle button = null;

            button = BlzCreateSimpleFrame("EmptySimpleButton", parent, 0);
            toolTip = BlzCreateFrameByType("SIMPLEFRAME", "", button, "", 0);
            BlzFrameSetAllPoints(button, frame);
            BlzFrameSetTooltip(button, toolTip);
            BlzFrameSetLevel(button, 9);
            BlzFrameSetVisible(toolTip, false);
            return toolTip;
        }

        public static void UnitInfoAddTooltipEx(framehandle parent, framehandle frame, Func<bool> tooltipAction)
        {
            UnitInfoPanelAddTooltipListener(UnitInfoAddTooltip(parent, frame), tooltipAction);
        }

        public static int UnitInfoCreateCustomInfo(framehandle parent, string label, string texture, Func<bool> tooltipAction)
        {
            _createContext++;
            _unitInfoInfoFrame = BlzCreateSimpleFrame("SimpleInfoPanelIconRank", parent, _createContext++);
            _unitInfoIconFrame = BlzGetFrameByName("InfoPanelIconBackdrop", _createContext++);
            _unitInfoLabelFrame = BlzGetFrameByName("InfoPanelIconLabel", _createContext++);
            _unitInfoTextFrame = BlzGetFrameByName("InfoPanelIconValue", _createContext++);
            BlzFrameSetText(_unitInfoLabelFrame, label);
            BlzFrameSetText(_unitInfoTextFrame, "xxx");
            BlzFrameSetTexture(_unitInfoIconFrame, texture, 0, false);
            BlzFrameClearAllPoints(_unitInfoIconFrame);
            BlzFrameSetSize(_unitInfoIconFrame, 0.028f, 0.028f);
            if (tooltipAction != null)
            {
                UnitInfoAddTooltipEx(_unitInfoInfoFrame, _unitInfoIconFrame, tooltipAction);
            }
            return _createContext;
        }
    }
}