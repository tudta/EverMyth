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
    public static class FrameLoader
    {
        // in 1.31 and upto 1.32.9 PTR (when I wrote this). Frames are not correctly saved and loaded, breaking the game.
        // This library runs all functions added to it with a 0s delay after the game was loaded.
        // function FrameLoaderAdd takes code func returns nothing
        // func runs when the game is loaded.

        private static trigger eventTrigger = CreateTrigger();
        private static trigger actionTrigger = CreateTrigger();
        private static timer t = CreateTimer();

        public static void Init()
        {
            TriggerRegisterGameEvent(eventTrigger, EVENT_GAME_LOADED);
            TriggerAddAction(eventTrigger, EventAction);
        }

        public static void FrameLoaderAdd(Action func)
        {
            TriggerAddAction(actionTrigger, func);
        }

        private static void TimerAction()
        {
            TriggerExecute(actionTrigger);
        }

        private static void EventAction()
        {
            TimerStart(t, 0, false, TimerAction);
        }
    }
}