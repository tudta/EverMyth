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
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source
{
    public static class ItemSystem
    {
        private static Dictionary<int, ItemData> _itemReferenceDatabase;
        private static Dictionary<item, ItemData> _itemInstanceDatabase;

        public static void Init()
        {
            Console.WriteLine("Initializing item system!");
            InitializeItemDatabases();
            PlayerUnitEvents.Register(ItemTypeEvent.IsPickedUp, CheckItemPickup);
            PlayerUnitEvents.Register(ItemTypeEvent.IsDropped, ItemDropped);
        }

        private static void CheckItemPickup()
        {
            Console.WriteLine("Checking item pickup!");
            item eventItem = GetManipulatedItem();
            int itemId = GetItemTypeId(eventItem);
            unit triggerUnit = GetTriggerUnit();
            ItemData itemData = new ItemData(triggerUnit, GetItemData(itemId));
            UnitInstance unitInstance = UnitManager.GetUnitInstance(triggerUnit);
            Console.WriteLine("Name of triggering unit: " + GetUnitName(triggerUnit));
            Console.WriteLine("Name of picked up item: " + GetItemName(eventItem));
            Console.WriteLine("Id of picked up item: " + itemId);
            ApplyItemStats(unitInstance, itemData);
            _itemInstanceDatabase.Add(eventItem, itemData);
            if (!UnitCanAcquireItem(unitInstance, itemData))
            {
                //force drop item
                UnitRemoveItem(triggerUnit, eventItem);
            }
        }

        private static void ItemDropped()
        {
            item eventItem = GetManipulatedItem();
            int itemId = GetItemTypeId(eventItem);
            ItemData itemData = GetItemData(itemId);
            unit triggerUnit = GetTriggerUnit();
            UnitInstance unitInstance = UnitManager.GetUnitInstance(triggerUnit);
            Console.WriteLine(GetUnitName(triggerUnit) + " dropped " + GetItemName(eventItem));
            RemoveItemStats(unitInstance, itemData);
            _itemInstanceDatabase.Remove(eventItem);
        }

        public static void PopulateInventory(unit targetUnit, InventoryData unitInventoryData)
        {
            for (int i = 0; i < unitInventoryData.HeroInventoryIds.Length; i++)
            {
                if (unitInventoryData.HeroInventoryIds[i] != 0)
                {
                    UnitAddItemById(targetUnit, unitInventoryData.HeroInventoryIds[i]);
                }
            }
        }

        public static ItemData GetItemData(int itemId)
        {
            _itemReferenceDatabase.TryGetValue(itemId, out ItemData data);
            return data;
        }

        private static bool UnitCanAcquireItem(UnitInstance unitInstance, ItemData targetItem)
        {
            //check if item can be picked up
            List<ItemData> items = GetUnitInventoryItems(unitInstance.LinkedUnit);
            int targetItemCount = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    // Check if class can wield
                    if (items[i].ClassIdRestrictions.Contains(unitInstance.UnitData.UnitTypeId))
                    {
                        Console.WriteLine("Can't pick up item because your class can't use it!");
                        return false;
                    }
                    // Ignore first instance of item.
                    if (items[i].ItemId == targetItem.ItemId)
                    {
                        targetItemCount++;
                        if (targetItemCount > 1)
                        {
                            Console.WriteLine("Can't pick up item because you already have one of the same type!");
                            return false;
                        }
                        continue;
                    }
                    // Check if item type is already equipped.
                    if (items[i].ItemClassification == targetItem.ItemClassification)
                    {
                        Console.WriteLine("Can't pick up item because you already have one of the same type!");
                        return false;
                    }
                }
            }
            return true;
        }

        public static List<ItemData> GetUnitInventoryItems(unit targetUnit)
        {
            List<ItemData> items = new List<ItemData>();
            item tmpItem;
            for (int i = 0; i < UnitInventorySize(targetUnit); i++)
            {
                tmpItem = UnitItemInSlot(targetUnit, i);
                items.Add(GetItemData(GetItemTypeId(tmpItem)));
            }
            return items;
        }

        private static void ApplyItemStats(UnitInstance targetInstance, ItemData targetItem)
        {
            targetInstance.UnitData.BonusFlatHealth += targetItem.Stats.BonusFlatHealth;
            targetInstance.UnitData.BonusPercentHealth += targetItem.Stats.BonusPercentHealth;
            targetInstance.UnitData.BonusFlatHealthRegeneration += targetItem.Stats.BonusFlatHealthRegeneration;
            targetInstance.UnitData.BonusPercentHealthRegeneration += targetItem.Stats.BonusPercentHealthRegeneration;
            targetInstance.UnitData.PercentMaximumHealthRegeneration += targetItem.Stats.PercentMaximumHealthRegeneration;
            targetInstance.UnitData.HealingReceivedBonus += targetItem.Stats.HealingReceivedBonus;
            targetInstance.UnitData.BonusFlatMana += targetItem.Stats.BonusFlatMana;
            targetInstance.UnitData.BonusPercentMana += targetItem.Stats.BonusPercentMana;
            targetInstance.UnitData.BonusFlatManaRegeneration += targetItem.Stats.BonusFlatManaRegeneration;
            targetInstance.UnitData.BonusPercentManaRegeneration += targetItem.Stats.BonusPercentManaRegeneration;
            targetInstance.UnitData.PercentMaximumManaRegeneration += targetItem.Stats.PercentMaximumManaRegeneration;
            targetInstance.UnitData.BonusFlatAgility += targetItem.Stats.BonusFlatAgility;
            targetInstance.UnitData.BonusPercentAgility += targetItem.Stats.BonusPercentAgility;
            targetInstance.UnitData.BonusFlatIntelligence += targetItem.Stats.BonusFlatIntelligence;
            targetInstance.UnitData.BonusPercentIntelligence += targetItem.Stats.BonusPercentIntelligence;
            targetInstance.UnitData.BonusFlatStrength += targetItem.Stats.BonusFlatStrength;
            targetInstance.UnitData.BonusPercentStrength += targetItem.Stats.BonusPercentStrength;
            targetInstance.UnitData.BonusAttackCooldown += targetItem.Stats.BonusAttackCooldown;
            targetInstance.UnitData.BonusFlatPhysicalAttackDamage += targetItem.Stats.BonusFlatPhysicalAttackDamage;
            targetInstance.UnitData.BonusPercentPhysicalAttackDamage += targetItem.Stats.BonusPercentPhysicalAttackDamage;
            targetInstance.UnitData.BonusFlatMagicalAttackDamage += targetItem.Stats.BonusFlatMagicalAttackDamage;
            targetInstance.UnitData.BonusPercentMagicalAttackDamage += targetItem.Stats.BonusPercentMagicalAttackDamage;
            targetInstance.UnitData.BonusFlatPhysicalPenetration += targetItem.Stats.BonusFlatPhysicalPenetration;
            targetInstance.UnitData.AbsolutePercentPhysicalPenetration += targetItem.Stats.AbsolutePercentPhysicalPenetration;
            targetInstance.UnitData.BonusFlatMagicalPenetration += targetItem.Stats.BonusFlatMagicalPenetration;
            targetInstance.UnitData.AbsolutePercentMagicalPenetration += targetItem.Stats.AbsolutePercentMagicalPenetration;
            targetInstance.UnitData.BonusPercentSkillDamage += targetItem.Stats.BonusPercentSkillDamage;
            targetInstance.UnitData.BonusPercentAbilityDamage += targetItem.Stats.BonusPercentAbilityDamage;
            targetInstance.UnitData.BonusPercentSpellDamage += targetItem.Stats.BonusPercentSpellDamage;
            targetInstance.UnitData.BonusCriticalChance += targetItem.Stats.BonusCriticalChance;
            targetInstance.UnitData.BonusCriticalDamage += targetItem.Stats.BonusCriticalDamage;
            targetInstance.UnitData.BonusFlatPhysicalDamageReduction += targetItem.Stats.BonusFlatPhysicalDamageReduction;
            targetInstance.UnitData.BonusPercentPhysicalDamageReduction += targetItem.Stats.BonusPercentPhysicalDamageReduction;
            targetInstance.UnitData.AbsolutePhysicalDamageReductionPercent += targetItem.Stats.AbsolutePhysicalDamageReductionPercent;
            targetInstance.UnitData.BonusFlatMagicalDamageReduction += targetItem.Stats.BonusFlatMagicalDamageReduction;
            targetInstance.UnitData.BonusPercentMagicalDamageReduction += targetItem.Stats.BonusPercentMagicalDamageReduction;
            targetInstance.UnitData.AbsoluteMagicalDamageReductionPercent += targetItem.Stats.AbsoluteMagicalDamageReductionPercent;
            if (targetItem.PassiveBuff != null)
            {
                BuffSystem.Add(targetItem.PassiveBuff);
            }
        }

        private static void RemoveItemStats(UnitInstance targetInstance, ItemData targetItem)
        {
            targetInstance.UnitData.BonusFlatHealth -= targetItem.Stats.BonusFlatHealth;
            targetInstance.UnitData.BonusPercentHealth -= targetItem.Stats.BonusPercentHealth;
            targetInstance.UnitData.BonusFlatHealthRegeneration -= targetItem.Stats.BonusFlatHealthRegeneration;
            targetInstance.UnitData.BonusPercentHealthRegeneration -= targetItem.Stats.BonusPercentHealthRegeneration;
            targetInstance.UnitData.PercentMaximumHealthRegeneration -= targetItem.Stats.PercentMaximumHealthRegeneration;
            targetInstance.UnitData.HealingReceivedBonus -= targetItem.Stats.HealingReceivedBonus;
            targetInstance.UnitData.BonusFlatMana -= targetItem.Stats.BonusFlatMana;
            targetInstance.UnitData.BonusPercentMana -= targetItem.Stats.BonusPercentMana;
            targetInstance.UnitData.BonusFlatManaRegeneration -= targetItem.Stats.BonusFlatManaRegeneration;
            targetInstance.UnitData.BonusPercentManaRegeneration -= targetItem.Stats.BonusPercentManaRegeneration;
            targetInstance.UnitData.PercentMaximumManaRegeneration -= targetItem.Stats.PercentMaximumManaRegeneration;
            targetInstance.UnitData.BonusFlatAgility -= targetItem.Stats.BonusFlatAgility;
            targetInstance.UnitData.BonusPercentAgility -= targetItem.Stats.BonusPercentAgility;
            targetInstance.UnitData.BonusFlatIntelligence -= targetItem.Stats.BonusFlatIntelligence;
            targetInstance.UnitData.BonusPercentIntelligence -= targetItem.Stats.BonusPercentIntelligence;
            targetInstance.UnitData.BonusFlatStrength -= targetItem.Stats.BonusFlatStrength;
            targetInstance.UnitData.BonusPercentStrength -= targetItem.Stats.BonusPercentStrength;
            targetInstance.UnitData.BonusAttackCooldown -= targetItem.Stats.BonusAttackCooldown;
            targetInstance.UnitData.BonusFlatPhysicalAttackDamage -= targetItem.Stats.BonusFlatPhysicalAttackDamage;
            targetInstance.UnitData.BonusPercentPhysicalAttackDamage -= targetItem.Stats.BonusPercentPhysicalAttackDamage;
            targetInstance.UnitData.BonusFlatMagicalAttackDamage -= targetItem.Stats.BonusFlatMagicalAttackDamage;
            targetInstance.UnitData.BonusPercentMagicalAttackDamage -= targetItem.Stats.BonusPercentMagicalAttackDamage;
            targetInstance.UnitData.BonusFlatPhysicalPenetration -= targetItem.Stats.BonusFlatPhysicalPenetration;
            targetInstance.UnitData.AbsolutePercentPhysicalPenetration -= targetItem.Stats.AbsolutePercentPhysicalPenetration;
            targetInstance.UnitData.BonusFlatMagicalPenetration -= targetItem.Stats.BonusFlatMagicalPenetration;
            targetInstance.UnitData.AbsolutePercentMagicalPenetration -= targetItem.Stats.AbsolutePercentMagicalPenetration;
            targetInstance.UnitData.BonusPercentSkillDamage -= targetItem.Stats.BonusPercentSkillDamage;
            targetInstance.UnitData.BonusPercentAbilityDamage -= targetItem.Stats.BonusPercentAbilityDamage;
            targetInstance.UnitData.BonusPercentSpellDamage -= targetItem.Stats.BonusPercentSpellDamage;
            targetInstance.UnitData.BonusCriticalChance -= targetItem.Stats.BonusCriticalChance;
            targetInstance.UnitData.BonusCriticalDamage -= targetItem.Stats.BonusCriticalDamage;
            targetInstance.UnitData.BonusFlatPhysicalDamageReduction -= targetItem.Stats.BonusFlatPhysicalDamageReduction;
            targetInstance.UnitData.BonusPercentPhysicalDamageReduction -= targetItem.Stats.BonusPercentPhysicalDamageReduction;
            targetInstance.UnitData.AbsolutePhysicalDamageReductionPercent -= targetItem.Stats.AbsolutePhysicalDamageReductionPercent;
            targetInstance.UnitData.BonusFlatMagicalDamageReduction -= targetItem.Stats.BonusFlatMagicalDamageReduction;
            targetInstance.UnitData.BonusPercentMagicalDamageReduction -= targetItem.Stats.BonusPercentMagicalDamageReduction;
            targetInstance.UnitData.AbsoluteMagicalDamageReductionPercent -= targetItem.Stats.AbsoluteMagicalDamageReductionPercent;
            if (targetItem.PassiveBuff != null)
            {
                BuffSystem.Dispel(targetInstance.LinkedUnit, null, true, 1, targetItem.PassiveBuff.BuffTypes[0]);
            }
        }

        private static void InitializeItemDatabases()
        {
            _itemInstanceDatabase = new Dictionary<item, ItemData>();
            InitializeItemReferenceDatabase();
        }

        private static void InitializeItemReferenceDatabase()
        {
            _itemReferenceDatabase = new Dictionary<int, ItemData>();
            _itemReferenceDatabase.Add(Constants.ITEM_DUMMYITEM, new ItemData(Constants.ITEM_DUMMYITEM, ItemType.ACCESSORY, new List<int>() {  }, new ItemStats(), null));
            _itemReferenceDatabase.Add(Constants.ITEM_CLAWS_OF_BOP, new ItemData(Constants.ITEM_CLAWS_OF_BOP, ItemType.WEAPON, new List<int>() {  }, new ItemStats() { BonusFlatHealth = 1000.0f }, null));
            _itemReferenceDatabase.Add(Constants.ITEM_CLAWS_OF_PASSIVE_BOP, new ItemData(Constants.ITEM_CLAWS_OF_PASSIVE_BOP, ItemType.WEAPON, new List<int>() {  }, new ItemStats(), new Buffs.ItemStunPassive((unit)null, (unit)null)));
        }
    }
}