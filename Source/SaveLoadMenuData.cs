using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public class SaveLoadMenuData
    {
        private int _saveSlotPage = 0;
        private int _saveSlotIndex = 0;

        public int SaveSlotPage { get => _saveSlotPage; set => _saveSlotPage = value; }
        public int SaveSlotIndex { get => _saveSlotIndex; set => _saveSlotIndex = value; }
    }
}