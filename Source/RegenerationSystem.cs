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
    public static class RegenerationSystem
    {
        private static UnitCombatData tmpUnitData = null;
        public static void Init()
        {
            Console.WriteLine("Regeneration system initialized!");
            CreateRegenTrigger();
        }

        private static void CreateRegenTrigger()
        {
            trigger regenTickTrigger = CreateTrigger();
            TriggerRegisterTimerEventPeriodic(regenTickTrigger, 1.0f);
            TriggerAddAction(regenTickTrigger, Regenerate);
        }

        private static void Regenerate()
        {
            for (int i = 0; i < UnitManager.UnitInstanceDatabase.Count; i++)
            {
                tmpUnitData = UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData;
                tmpUnitData.CurrentHealth += MathRound((tmpUnitData.TotalHealthRegeneration + (tmpUnitData.TotalMaximumHealth * tmpUnitData.PercentMaximumHealthRegeneration)) * (1 + tmpUnitData.HealingReceivedBonus));
                tmpUnitData.CurrentMana += MathRound(tmpUnitData.TotalManaRegeneration + (tmpUnitData.TotalMaximumMana * tmpUnitData.PercentMaximumManaRegeneration));
                //UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.CurrentHealth += MathRound(UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalHealthRegeneration + (UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalMaximumHealth * UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.PercentMaximumHealthRegeneration));
                //UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.CurrentMana += MathRound(UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalManaRegeneration + (UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalMaximumMana * UnitManager.UnitInstanceDatabase.ElementAt(i).Value.UnitData.PercentMaximumManaRegeneration));
            }
            for (int i = 0; i < UnitManager.HeroUnitInstanceDatabase.Count; i++)
            {
                tmpUnitData = UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData;
                tmpUnitData.CurrentHealth += MathRound((tmpUnitData.TotalHealthRegeneration + (tmpUnitData.TotalMaximumHealth * tmpUnitData.PercentMaximumHealthRegeneration)) * (1 + tmpUnitData.HealingReceivedBonus));
                tmpUnitData.CurrentMana += MathRound(tmpUnitData.TotalManaRegeneration + (tmpUnitData.TotalMaximumMana * tmpUnitData.PercentMaximumManaRegeneration));
                //UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.CurrentHealth += MathRound(UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalHealthRegeneration + (UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalMaximumHealth * UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.PercentMaximumHealthRegeneration));
                //UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.CurrentMana += MathRound(UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalManaRegeneration + (UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.TotalMaximumMana * UnitManager.HeroUnitInstanceDatabase.ElementAt(i).Value.UnitData.PercentMaximumManaRegeneration));
            }
        }
    }
}