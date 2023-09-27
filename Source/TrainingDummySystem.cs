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

namespace Source
{
    public static class TrainingDummySystem
    {
        private static List<bool> _trainingDummyActiveStates;

        private static List<int> _attackCounts;
        private static List<int> _totalPhysicalDamages;
        private static List<int> _totalMagicalDamages;
        private static List<int> _totalPureDamages;
        private static List<int> _totalDamages;

        private static float _resetDuration = 10.0f;
        private static float _updateDuration = 0.05f;

        private static List<timer> _damageTimers;
        private static List<timer> _updateTimers;
        private static List<timer> _resetTimers;

        private static List<texttag> _elapsedTimeTexts;
        private static List<texttag> _attacksPerSecondTexts;
        private static List<texttag> _physicalDamageTexts;
        private static List<texttag> _magicalDamageTexts;
        private static List<texttag> _pureDamageTexts;
        private static List<texttag> _totalDamageTexts;

        private static float[] _elapsedTimeColor = new float[3] { 255.0f, 0.0f, 255.0f };
        private static float[] _attacksPerSecondColor = new float[3] { 255.0f, 255.0f, 0.0f };
        private static float[] _physicalDamageColor = new float[3] { 255.0f, 0.0f, 0.0f };
        private static float[] _magicalDamageColor = new float[3] { 0.0f, 0.0f, 255.0f };
        private static float[] _pureDamageColor = new float[3] { 255.0f, 255.0f, 255.0f };
        private static float[] _totalDamageColor = new float[3] { 0.0f, 255.0f, 0.0f };

        private static float _textSize = 9.0f;
        private static float _startingTextZOffset = 200.0f;
        private static float _textZOffsetIncrement = -65.0f;

        private static List<UnitInstance> _trainingDummyUnits;

        public static void Init()
        {
            _trainingDummyActiveStates = new List<bool>();

            _attackCounts = new List<int>();
            _totalPhysicalDamages = new List<int>();
            _totalMagicalDamages = new List<int>();
            _totalPureDamages = new List<int>();
            _totalDamages = new List<int>();

            _damageTimers = new List<timer>();
            _updateTimers = new List<timer>();
            _resetTimers = new List<timer>();

            _elapsedTimeTexts = new List<texttag>();
            _attacksPerSecondTexts = new List<texttag>();
            _physicalDamageTexts = new List<texttag>();
            _magicalDamageTexts = new List<texttag>();
            _pureDamageTexts = new List<texttag>();
            _totalDamageTexts = new List<texttag>();

            _trainingDummyUnits = new List<UnitInstance>();

            DamageSystem.DamageEngine.UnitAutoAttacked += UpdateDummyAttackTrackers;
            DamageSystem.DamageEngine.UnitDamaged += UpdateDummyDamageTrackers;

            InitializeTrainingDummies();
        }

        private static void InitializeTrainingDummies()
        {
            _trainingDummyUnits = UnitManager.GetUnitInstancesOfType(Constants.UNIT_TRAINING_DUMMY);
            if (_trainingDummyUnits != null)
            {
                for (int i = 0; i < _trainingDummyUnits.Count; i++)
                {
                    InitializeTrainingDummy(i);
                }
            }
        }

        private static void InitializeTrainingDummy(int unitIndex)
        {
            InitializeActiveState();
            InitializeDamageCounts();
            InitializeTimers();
            InitializeTextTags(unitIndex);
        }

        private static void InitializeActiveState()
        {
            _trainingDummyActiveStates.Add(false);
        }

        private static void InitializeDamageCounts()
        {
            _attackCounts.Add(0);
            _totalPhysicalDamages.Add(0);
            _totalMagicalDamages.Add(0);
            _totalPureDamages.Add(0);
            _totalDamages.Add(0);
        }

        private static void InitializeTimers()
        {
            _damageTimers.Add(CreateTimer());
            _updateTimers.Add(CreateTimer());
            _resetTimers.Add(CreateTimer());
        }

        private static void InitializeTextTags(int unitIndex)
        {
            _elapsedTimeTexts.Add(CreateTextTagUnitBJ("", _trainingDummyUnits[unitIndex].LinkedUnit, _startingTextZOffset, _textSize, _elapsedTimeColor[0], _elapsedTimeColor[1], _elapsedTimeColor[2], 0.0f));
            SetTextTagPermanent(_elapsedTimeTexts[unitIndex], true);
            SetTextTagVisibility(_elapsedTimeTexts[unitIndex], false);
            _attacksPerSecondTexts.Add(CreateTextTagUnitBJ("", _trainingDummyUnits[unitIndex].LinkedUnit, _startingTextZOffset + (_textZOffsetIncrement * 1), _textSize, _attacksPerSecondColor[0], _attacksPerSecondColor[1], _attacksPerSecondColor[2], 0.0f));
            SetTextTagPermanent(_attacksPerSecondTexts[unitIndex], true);
            SetTextTagVisibility(_attacksPerSecondTexts[unitIndex], false);
            _physicalDamageTexts.Add(CreateTextTagUnitBJ("", _trainingDummyUnits[unitIndex].LinkedUnit, _startingTextZOffset + (_textZOffsetIncrement * 2), _textSize, _physicalDamageColor[0], _physicalDamageColor[1], _physicalDamageColor[2], 0.0f));
            SetTextTagPermanent(_physicalDamageTexts[unitIndex], true);
            SetTextTagVisibility(_physicalDamageTexts[unitIndex], false);
            _magicalDamageTexts.Add(CreateTextTagUnitBJ("", _trainingDummyUnits[unitIndex].LinkedUnit, _startingTextZOffset + (_textZOffsetIncrement * 3), _textSize, _magicalDamageColor[0], _magicalDamageColor[1], _magicalDamageColor[2], 0.0f));
            SetTextTagPermanent(_magicalDamageTexts[unitIndex], true);
            SetTextTagVisibility(_magicalDamageTexts[unitIndex], false);
            _pureDamageTexts.Add(CreateTextTagUnitBJ("", _trainingDummyUnits[unitIndex].LinkedUnit, _startingTextZOffset + (_textZOffsetIncrement * 4), _textSize, _pureDamageColor[0], _pureDamageColor[1], _pureDamageColor[2], 0.0f));
            SetTextTagPermanent(_pureDamageTexts[unitIndex], true);
            SetTextTagVisibility(_pureDamageTexts[unitIndex], false);
            _totalDamageTexts.Add(CreateTextTagUnitBJ("", _trainingDummyUnits[unitIndex].LinkedUnit, _startingTextZOffset + (_textZOffsetIncrement * 5), _textSize, _totalDamageColor[0], _totalDamageColor[1], _totalDamageColor[2], 0.0f));
            SetTextTagPermanent(_totalDamageTexts[unitIndex], true);
            SetTextTagVisibility(_totalDamageTexts[unitIndex], false);
        }

        // Called when attacked while inactive.
        private static void ActivateDummy(int unitIndex)
        {
            _trainingDummyActiveStates[unitIndex] = true;

            TimerStart(_damageTimers[unitIndex], float.MaxValue, false, null);
            TimerStart(_updateTimers[unitIndex], _updateDuration, true, UpdateDummyTextValues);
            TimerStart(_resetTimers[unitIndex], _resetDuration, false, ResetDummy);

            SetTextTagVisibility(_elapsedTimeTexts[unitIndex], true);
            SetTextTagVisibility(_attacksPerSecondTexts[unitIndex], true);
            SetTextTagVisibility(_physicalDamageTexts[unitIndex], true);
            SetTextTagVisibility(_magicalDamageTexts[unitIndex], true);
            SetTextTagVisibility(_pureDamageTexts[unitIndex], true); ;
            SetTextTagVisibility(_totalDamageTexts[unitIndex], true);
        }

        // Called at end of update timer to update text values.
        private static void UpdateDummyTextValues()
        {
            int timerIndex = _updateTimers.IndexOf(GetExpiredTimer());
            float elapsedTime = TimerGetElapsed(_damageTimers[timerIndex]);
            float attacksPerSecond = _attackCounts[timerIndex] / elapsedTime;
            float physicalDamagePerSecond = _totalPhysicalDamages[timerIndex] / elapsedTime;
            float magicalDamagePerSecond = _totalMagicalDamages[timerIndex] / elapsedTime;
            float pureDamagePerSecond = _totalPureDamages[timerIndex] / elapsedTime;
            float totalDamagePerSecond = _totalDamages[timerIndex] / elapsedTime;

            SetTextTagTextBJ(_elapsedTimeTexts[timerIndex], "Time elapsed: " + R2S(elapsedTime), _textSize);
            SetTextTagTextBJ(_attacksPerSecondTexts[timerIndex], "Attacks per second: " + R2S(attacksPerSecond), _textSize);
            SetTextTagTextBJ(_physicalDamageTexts[timerIndex], "Physical DPS: " + R2S(physicalDamagePerSecond), _textSize);
            SetTextTagTextBJ(_magicalDamageTexts[timerIndex], "Magical DPS: " + R2S(magicalDamagePerSecond), _textSize);
            SetTextTagTextBJ(_pureDamageTexts[timerIndex], "Pure DPS: " + R2S(pureDamagePerSecond), _textSize);
            SetTextTagTextBJ(_totalDamageTexts[timerIndex], "Total DPS: " + R2S(totalDamagePerSecond), _textSize);
        }

        // Called at end of reset timer.
        private static void ResetDummy()
        {
            int timerIndex = _resetTimers.IndexOf(GetExpiredTimer());
            _trainingDummyActiveStates[timerIndex] = false;

            // Reset attack variables.
            _attackCounts[timerIndex] = 0;
            _totalPhysicalDamages[timerIndex] = 0;
            _totalMagicalDamages[timerIndex] = 0;
            _totalPureDamages[timerIndex] = 0;
            _totalDamages[timerIndex] = 0;

            // Stop timers.
            PauseTimer(_damageTimers[timerIndex]);
            PauseTimer(_updateTimers[timerIndex]);

            // Reset floating text values.
            SetTextTagText(_elapsedTimeTexts[timerIndex], R2S(0.0f), _textSize);
            SetTextTagText(_attacksPerSecondTexts[timerIndex], R2S(0.0f), _textSize);
            SetTextTagText(_physicalDamageTexts[timerIndex], R2S(0.0f), _textSize);
            SetTextTagText(_magicalDamageTexts[timerIndex], R2S(0.0f), _textSize);
            SetTextTagText(_pureDamageTexts[timerIndex], R2S(0.0f), _textSize);
            SetTextTagText(_totalDamageTexts[timerIndex], R2S(0.0f), _textSize);

            // Hide floating texts.
            SetTextTagVisibility(_elapsedTimeTexts[timerIndex], false);
            SetTextTagVisibility(_attacksPerSecondTexts[timerIndex], false);
            SetTextTagVisibility(_physicalDamageTexts[timerIndex], false);
            SetTextTagVisibility(_magicalDamageTexts[timerIndex], false);
            SetTextTagVisibility(_pureDamageTexts[timerIndex], false); ;
            SetTextTagVisibility(_totalDamageTexts[timerIndex], false);

            // Heal training dummy.
            _trainingDummyUnits[timerIndex].UnitData.CurrentHealth = _trainingDummyUnits[timerIndex].UnitData.TotalMaximumHealth;
        }

        private static void UpdateDummyAttackTrackers(object sender, DamageSystem.UnitAttackEventArgs args)
        {
            if (GetUnitTypeId(args.TargetUnit.LinkedUnit) == Constants.UNIT_TRAINING_DUMMY)
            {
                int unitIndex = _trainingDummyUnits.IndexOf(args.TargetUnit);
                if (_trainingDummyActiveStates[unitIndex] == false)
                {
                    ActivateDummy(unitIndex);
                }
                else
                {
                    TimerStart(_resetTimers[unitIndex], _resetDuration, false, ResetDummy);
                }
                _attackCounts[unitIndex]++;
            }
        }

        private static void UpdateDummyDamageTrackers(object sender, DamageSystem.UnitDamagedEventArgs args)
        {
            if (GetUnitTypeId(args.TargetUnit.LinkedUnit) == Constants.UNIT_TRAINING_DUMMY)
            {
                int unitIndex = _trainingDummyUnits.IndexOf(args.TargetUnit);
                if (_trainingDummyActiveStates[unitIndex] == false)
                {
                    ActivateDummy(unitIndex);
                }
                else
                {
                    TimerStart(_resetTimers[unitIndex], _resetDuration, false, ResetDummy);
                }
                switch (args.DamageDealtType)
                {
                    case DamageSystem.DamageType.PHYSICAL:
                        _totalPhysicalDamages[unitIndex] += args.DamageDealtAmount;
                        break;
                    case DamageSystem.DamageType.MAGICAL:
                        _totalMagicalDamages[unitIndex] += args.DamageDealtAmount;
                        break;
                    case DamageSystem.DamageType.PURE:
                        _totalPureDamages[unitIndex] += args.DamageDealtAmount;
                        break;
                    default:
                        break;
                }
                _totalDamages[unitIndex] += args.DamageDealtAmount;
            }
        }
    }
}