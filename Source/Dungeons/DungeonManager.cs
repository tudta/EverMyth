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
using WCSharp.Shared.Data;
using WCSharp.Sync;
using static Constants;
using static Regions;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.Dungeons
{
    public static class DungeonManager
    {
        private static List<rect> _dungeonRects = null;
        private static Dungeon _firstDungeon = null;

        public static List<rect> DungeonRects
        {
            get
            {
                return _dungeonRects;
            }
        }

        public static void Init()
        {
            AssignDungeonRects();
            _firstDungeon = new CaveDungeon();
            _firstDungeon.Init();
        }

        private static void AssignDungeonRects ()
        {
            _dungeonRects = new List<rect>();
            _dungeonRects.Add(Regions.Dungeon1Zone.Rect);
        }
    }
}
