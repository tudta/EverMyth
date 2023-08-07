using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp.SaveLoad;

namespace Source
{
    public class SaveData : Saveable
    {
        public bool IsPopulated { get; set; }
        public ResourceData ResourceSaveData { get; set; }
        public HeroData HeroUnitData { get; set; }
        public InventoryData HeroInventoryData { get; set; }
    }
}