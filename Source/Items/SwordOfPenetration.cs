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
//using static Constants;
//using static Regions;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.Items
{
    public static class SwordOfPenetration
    {
        public static void Init()
        {
            PlayerUnitEvents.Register(ItemTypeEvent.IsPickedUp, PickUpItem);
            PlayerUnitEvents.Register(ItemTypeEvent.IsDropped, DropItem);
        }

        //Add item stats to player
        private static void PickUpItem()
        {

        }

        //Remove items stats from player
        private static void DropItem()
        {

        }
    }
}
