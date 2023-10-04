using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WCSharp.Buffs;
using static War3Api.Common;

namespace Source
{
    public class ItemData
    {
        private int _itemId = 0;
        private string _itemDescription = string.Empty;
        private ItemType _itemClassification;
        private List<int> _classIdRestrictions;
        private ItemStats _stats;
        private Buff _passiveBuff;
        //active ability = attached to item

        public int ItemId
        {
            get
            {
                return _itemId;
            }

            set
            {
                _itemId = value;
            }
        }

        public string ItemDescription
        {
            get
            {
                return _itemDescription;
            }

            set
            {
                _itemDescription = value;
            }
        }

        public ItemType ItemClassification
        {
            get
            {
                return _itemClassification;
            }

            set
            {
                _itemClassification = value;
            }
        }

        public List<int> ClassIdRestrictions
        {
            get
            {
                return _classIdRestrictions;
            }

            set
            {
                _classIdRestrictions = value;
            }
        }

        public ItemStats Stats
        {
            get
            {
                return _stats;
            }

            set
            {
                _stats = value;
            }
        }

        public Buff PassiveBuff
        {
            get
            {
                return _passiveBuff;
            }

            set
            {
                _passiveBuff = value;
            }
        }

        public ItemData(ItemData referenceData)
        {
            Console.WriteLine("Creating item data from only reference data!");
            _itemId = referenceData.ItemId;
            _itemClassification = referenceData.ItemClassification;
            _classIdRestrictions = referenceData.ClassIdRestrictions;
            _stats = referenceData.Stats;
        }

        public ItemData(unit targetUnit, ItemData referenceData)
        {
            Console.WriteLine("Creating item data from unit and reference data!");
            _itemId = referenceData.ItemId;
            _itemClassification = referenceData.ItemClassification;
            _classIdRestrictions = referenceData.ClassIdRestrictions;
            _stats = referenceData.Stats;
            if (referenceData.PassiveBuff != null)
            {
                Wrapper<unit> targetUnitWrapper = new Wrapper<unit>(targetUnit);
                _passiveBuff = (Buff)Activator.CreateInstance(referenceData.PassiveBuff.GetType(), targetUnitWrapper, targetUnitWrapper);
            }
        }

        /// <summary>
        /// Creates item data from scratch to be used as reference data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemClass"></param>
        /// <param name="classRestrictions"></param>
        /// <param name="stats"></param>
        /// <param name="passiveBuff"></param>
        public ItemData(int id, ItemType itemClass, List<int> classRestrictions, ItemStats stats, Buff passiveBuff)
        {
            Console.WriteLine("Creating reference item data from provided stats!");
            _itemId = id;
            _itemClassification = itemClass;
            _classIdRestrictions = classRestrictions;
            _stats = stats;
            _passiveBuff = passiveBuff;
        }
    }
}