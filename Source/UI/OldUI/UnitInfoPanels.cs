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
    public static class UnitInfoPanels
    {
        private static List<framehandle> panels = new List<framehandle>();
        private static List<framehandle> panelsFrame = new List<framehandle>();
        private static int panelsCount;
        private static List<trigger> panelsCondition = new List<trigger>();
        private static List<framehandle> tooltipListener = new List<framehandle>();
        private static List<trigger> tooltipListenerAction = new List<trigger>();
        public static framehandle UnitInfoTooltipFrame;
        private static int tooltipListenerCount;
        private static framehandle tooltipBox;
        private static framehandle tooltipText;
        private static bool isReforged;
        private static int createContext;
        public static unit UnitInfoUnit;
        private static List<trigger> updateAction = new List<trigger>();
        private static trigger buttonTrigger;
        private static int activeIndex;
        private static int wantedIndex = 1;
        private static group g;
        private static timer updateTimer;
        private static framehandle pageUp;
        private const bool HAVE_BIG_PAGE_BUTTON = false;
        private static framehandle pageUpBig;
        private static framehandle pageDown;
        private static framehandle unitInfo;
        private static framehandle parent;
        private static int pageSwaps;

        public static framehandle UnitInfoInfoFrame;
        public static framehandle UnitInfoIconFrame;
        public static framehandle UnitInfoLabelFrame;
        public static framehandle UnitInfoTextFrame;
        public static string UnitInfoTooltipText = "";

        //Transferred
        public static void Init()
        {
            isReforged = GetLocalizedString("REFORGED") != "REFORGED";
            TriggerAddAction(buttonTrigger, PageButtonAction);
            TimerStart(CreateTimer(), 0, false, At0s);
            FrameLoader.FrameLoaderAdd(At0s);
            //panelsCondition[1] = CreateTrigger();
            panelsCondition.Add(CreateTrigger());
            panelsCondition.Add(CreateTrigger());
        }

        //Transferred
        public static void UnitInfoGetUnit(player p)
        {
            GroupEnumUnitsSelected(g, p, null);
            UnitInfoUnit = FirstOfGroup(g);
            GroupClear(g);
        }

        //Transferred
        public static void AddUnitInfoPanel(framehandle frame, Func<bool> update, Func<bool> condition)
        {
            BlzFrameSetParent(frame, BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0));
            panelsCount++;
            //panels[panelsCount] = frame;
            panels.Add(frame);
            //panelsCondition[panelsCount] = CreateTrigger();
            panelsCondition.Add(CreateTrigger());
            if (condition != null)
            {
                //TriggerAddCondition(panelsCondition[panelsCount], Filter(condition));
                TriggerAddCondition(panelsCondition[panelsCondition.Count - 1], Filter(condition));
            }

            if (update != null)
            {
                //updateAction[panelsCount] = CreateTrigger();
                //TriggerAddCondition(updateAction[panelsCount], Filter(update));
                updateAction.Add(CreateTrigger());
                TriggerAddCondition(updateAction[updateAction.Count - 1], Filter(update));
            }
            BlzFrameSetVisible(frame, activeIndex == panelsCount);
        }

        //Transferred
        public static framehandle AddUnitInfoPanelEx(Func<bool> update, Func<bool> condition)
        {
            framehandle frame = BlzCreateFrameByType("SIMPLEFRAME", "", BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0), "", 0);
            AddUnitInfoPanel(frame, update, condition);
            return frame;
        }

        public static void SetUnitInfoPanelFrame(framehandle frame)
        {
            //panelsFrame[panelsCount] = frame;
            //BlzFrameSetVisible(frame, BlzFrameIsVisible(panels[panelsCount]));
            panelsFrame.Add(frame);
            BlzFrameSetVisible(frame, BlzFrameIsVisible(panels[panels.Count - 1]));
        }

        public static framehandle SetUnitInfoPanelFrameEx()
        {
            framehandle frame = BlzCreateFrameByType("FRAME", "", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), "", 0);
            SetUnitInfoPanelFrame(frame);
            return frame;
        }

        public static void UnitInfoPanelAddTooltipListener(framehandle frame, Func<bool> func)
        {
            tooltipListenerCount++;
            //tooltipListener[tooltipListenerCount] = frame;
            //tooltipListenerAction[tooltipListenerCount] = CreateTrigger();
            //TriggerAddCondition(tooltipListenerAction[tooltipListenerCount], Filter(func));
            tooltipListener.Add(frame);
            tooltipListenerAction.Add(CreateTrigger());
            TriggerAddCondition(tooltipListenerAction[tooltipListenerAction.Count - 1], Filter(func));
        }

        public static framehandle UnitInfoAddTooltip(framehandle parent, framehandle frame)
        {
            framehandle toolTip = null;
            framehandle but = null;

            but = BlzCreateSimpleFrame("EmptySimpleButton", parent, 0);
            toolTip = BlzCreateFrameByType("SIMPLEFRAME", "", but, "", 0);
            BlzFrameSetAllPoints(but, frame);
            BlzFrameSetTooltip(but, toolTip);
            BlzFrameSetLevel(but, 9);
            BlzFrameSetVisible(toolTip, false);
            return toolTip;
        }

        public static void UnitInfoAddTooltipEx(framehandle parent, framehandle frame, Func<bool> func)
        {
            UnitInfoPanelAddTooltipListener(UnitInfoAddTooltip(parent, frame), func);
        }

        public static int UnitInfoCreateCustomInfo(framehandle parent, string label, string texture, Func<bool> tooltipCode)
        {
            createContext++;
            UnitInfoInfoFrame = BlzCreateSimpleFrame("SimpleInfoPanelIconRank", parent, createContext);
            UnitInfoIconFrame = BlzGetFrameByName("InfoPanelIconBackdrop", createContext);
            UnitInfoLabelFrame = BlzGetFrameByName("InfoPanelIconLabel", createContext);
            UnitInfoTextFrame = BlzGetFrameByName("InfoPanelIconValue", createContext);
            BlzFrameSetText(UnitInfoLabelFrame, label);
            BlzFrameSetText(UnitInfoTextFrame, "xxx");
            BlzFrameSetTexture(UnitInfoIconFrame, texture, 0, false);
            BlzFrameClearAllPoints(UnitInfoIconFrame);
            BlzFrameSetSize(UnitInfoIconFrame, 0.028f, 0.028f);
            if (tooltipCode != null)
            {
                UnitInfoAddTooltipEx(UnitInfoInfoFrame, UnitInfoIconFrame, tooltipCode);
            }
            return createContext;
        }

        static bool PageSwapCheck()
        {
            pageSwaps++;
            if (pageSwaps > panelsCount)
            {
                BJDebugMsg("Unit Info Panel - NO VALID PANEL - " + GetUnitName(UnitInfoUnit));
                return false;
            }
            return true;
        }

        static void NextPanel()
        {
            activeIndex++;
            if (activeIndex > panelsCount)
            {
                activeIndex = 1;
            }
            if (PageSwapCheck() && !TriggerEvaluate(panelsCondition[activeIndex]))
            {
                NextPanel();
            }
        }

        static void PrevPanel()
        {
            activeIndex--;
            if (activeIndex < 1)
            {
                activeIndex = panelsCount;
            }
            if (PageSwapCheck() && !TriggerEvaluate(panelsCondition[activeIndex]))
            {
                PrevPanel();
            }
        }

        static void UnitInfoPanelSetPage(int newPage, bool updateWanted)
        {
            BlzFrameSetVisible(panels[activeIndex], false);
            BlzFrameSetVisible(panelsFrame[activeIndex], false);
            if (newPage == 0)
            {
                pageSwaps = 0;
                NextPanel();
            }
            else if (newPage == -1)
            {
                pageSwaps = 0;
                PrevPanel();
            }
            else
            {
                activeIndex = IMinBJ(panelsCount, IMaxBJ(1, newPage));
            }
            if (updateWanted)
            {
                wantedIndex = activeIndex;
            }
            BlzFrameSetVisible(panels[activeIndex], true);
            BlzFrameSetVisible(panelsFrame[activeIndex], true);
        }

        static void UnitInfoPanelSetPageByFrame(framehandle newPage, bool updateWanted)
        {
            int loopA = panelsCount;
            while (loopA > 0)
            {
                if (panels[loopA] == newPage || panelsFrame[loopA] == newPage)
                {
                    UnitInfoPanelSetPage(loopA, updateWanted);
                    break;
                }
                loopA--;
            }
        }

        static void Update()
        {
            bool found = false;
            int loopA = tooltipListenerCount;
            int useAblePages = 0;
            UnitInfoGetUnit(GetLocalPlayer());
            if (BlzFrameIsVisible(unitInfo))
            {
                while (loopA > 0)
                {
                    if (BlzFrameIsVisible(tooltipListener[loopA]))
                    {
                        UnitInfoTooltipFrame = tooltipListener[loopA];
                        TriggerEvaluate(tooltipListenerAction[loopA]);
                        BlzFrameSetText(tooltipText, UnitInfoTooltipText);
                        found = true;
                        break;
                    }
                    loopA--;
                }

                if (wantedIndex != activeIndex && TriggerEvaluate(panelsCondition[wantedIndex]))
                {
                    UnitInfoPanelSetPage(wantedIndex, false);
                }

                int loopB = panelsCount;
                while (loopB > 0)
                {
                    if (TriggerEvaluate(panelsCondition[loopB]))
                    {
                        useAblePages++;
                    }
                    loopB--;
                }
                BlzFrameSetVisible(pageDown, useAblePages > 1);
                BlzFrameSetVisible(pageUp, useAblePages > 1);
                if (!TriggerEvaluate(panelsCondition[activeIndex]))
                {
                    UnitInfoPanelSetPage(0, false);
                }
                BlzFrameSetVisible(panelsFrame[activeIndex], true);
                TriggerEvaluate(updateAction[activeIndex]);
            }
            else
            {
                BlzFrameSetVisible(panelsFrame[activeIndex], false);
            }
            BlzFrameSetVisible(tooltipBox, found);
        }

        static void PageButtonAction()
        {
            if (GetTriggerPlayer() == GetLocalPlayer())
            {
                if (BlzGetTriggerFrame() == pageDown)
                {
                    UnitInfoPanelSetPage(-1, true);
                }
                else
                {
                    UnitInfoPanelSetPage(0, true);
                }
            }
        }

        private static void At0s()
        {
            DestroyTimer(GetExpiredTimer());
            BlzLoadTOCFile("war3mapImported\\UnitInfoPanels.toc");
            //panelsCount = 1;
            panelsCount = 0;
            activeIndex = 1;
            tooltipListenerCount = 0;
            createContext = 1000;

            unitInfo = BlzGetFrameByName("SimpleInfoPanelUnitDetail", 0);
            parent = BlzCreateFrameByType("SIMPLEFRAME", "", unitInfo, "", 0);
            pageUp = BlzCreateSimpleFrame("UnitInfoSimpleIconButtonUp", unitInfo, 0);
            pageDown = BlzCreateSimpleFrame("UnitInfoSimpleIconButtonDown", unitInfo, 0);

            BlzFrameSetAbsPoint(pageUp, FRAMEPOINT_BOTTOMRIGHT, 0.51f, 0.08f);

#if HAVE_BIG_PAGE_BUTTON
            FrameHandle pageUpBig = BlzCreateSimpleFrame("EmptySimpleButton", unitInfo, 0);
            BlzFrameSetAllPoints(pageUpBig, unitInfo);
            BlzFrameSetLevel(pageUpBig, 0);
            BlzTriggerRegisterFrameEvent(buttonTrigger, pageUpBig, FrameEventType.FRAMEEVENT_CONTROL_CLICK);
#endif

            BlzTriggerRegisterFrameEvent(buttonTrigger, pageUp, FRAMEEVENT_CONTROL_CLICK);
            BlzTriggerRegisterFrameEvent(buttonTrigger, pageDown, FRAMEEVENT_CONTROL_CLICK);

            //panels[1] = parent;
            //if (activeIndex != 1)
            //{
            //    BlzFrameSetVisible(panels[1], false);
            //}
            BlzFrameSetVisible(parent, false);

            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconDamage", 0), parent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconDamage", 1), parent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconArmor", 2), parent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconRank", 3), parent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconFood", 4), parent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconGold", 5), parent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconHero", 6), parent);
            BlzFrameSetParent(BlzGetFrameByName("SimpleInfoPanelIconAlly", 7), parent);

            if (isReforged)
            {
                tooltipBox = BlzCreateFrame("CustomUnitInfoTextBox", BlzGetFrameByName("ConsoleUIBackdrop", 0), 0, 0);
            }
            else
            {
                tooltipBox = BlzCreateFrame("CustomUnitInfoTextBox", BlzGetOriginFrame(ORIGIN_FRAME_GAME_UI, 0), 0, 0);
            }

            tooltipText = BlzCreateFrame("CustomUnitInfoText", tooltipBox, 0, 0);
            BlzFrameSetAbsPoint(tooltipText, FRAMEPOINT_BOTTOMRIGHT, 0.79f, 0.18f);
            BlzFrameSetSize(tooltipText, 0.275f, 0f);
            BlzFrameSetPoint(tooltipBox, FRAMEPOINT_TOPLEFT, tooltipText, FRAMEPOINT_TOPLEFT, -0.01f, 0.01f);
            BlzFrameSetPoint(tooltipBox, FRAMEPOINT_BOTTOMRIGHT, tooltipText, FRAMEPOINT_BOTTOMRIGHT, 0.005f, -0.01f);
            BlzFrameSetVisible(tooltipBox, false);

            TimerStart(updateTimer, 0.05f, true, Update);
        }
    }
}