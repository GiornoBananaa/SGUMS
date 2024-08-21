using System;
using UnityEngine;

namespace UnitSystem.UnitModifierSystem
{
    [Serializable]
    public class UnitStatsModifier : IUnitModifier
    {
        [field: SerializeField] public ModifiableUnitStats StatsModifiers { get; set; }
        
        public UnitStatsModifier(ModifiableUnitStats statsModifiers)
        {
            StatsModifiers = statsModifiers;
        }

        public UnitStatsModifier()
        {
            StatsModifiers = new ModifiableUnitStats();
        }
        
        public bool ApplyModifier(Unit unit)
        {
            if(unit.Modifiers.ContainsKey(this)) return false;
            unit.Modifiers.Add(this, StatsModifiers.UnitStats);
            unit.Stats.AddStats(StatsModifiers.UnitStats);
            return true;
        }

        public bool RemoveModifier(Unit unit)
        {
            if(!unit.Modifiers.ContainsKey(this)) return false;
            unit.Stats.SubtractStats(unit.Modifiers[this]);
            unit.Modifiers.Remove(this);
            return true;
        }
    }
}