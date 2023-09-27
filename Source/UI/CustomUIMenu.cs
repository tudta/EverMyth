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
    public abstract class CustomUIMenu
    {
        private framehandle _baseFrameHandle = null;
        private bool _isUICreated = false;

        public framehandle BaseFrameHandle { get { return _baseFrameHandle; } set { _baseFrameHandle = value; } }
        protected bool IsUICreated { get { return _isUICreated; } set { _isUICreated = value; } }


        public abstract void Init();

        protected abstract void CreateMenu();

        public virtual void OpenMenu()
        {
            BlzFrameSetVisible(_baseFrameHandle, true);
        }

        public virtual void CloseMenu()
        {
            BlzFrameSetVisible(_baseFrameHandle, false);
        }

        public virtual void ResetFrameFocus()
        {
            BlzFrameSetEnable(BlzGetTriggerFrame(), false);
            BlzFrameSetEnable(BlzGetTriggerFrame(), true);
        }
    }
}