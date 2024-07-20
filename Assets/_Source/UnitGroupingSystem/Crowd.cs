using System.Collections.Generic;
using UnitFormationSystem;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnityEngine;

namespace UnitGroupingSystem
{
    public class Crowd
    {
        public HashSet<Unit> Units;
        public Path Path;
        public Formation Formation;
        public Vector2 Offset;
        
        public Crowd() => Units = new();
        
        public Crowd(IEnumerable<Unit> units)
        {
            Units = new();
            foreach (var unit in units)
            {
                Units.Add(unit);
                unit.UnitCrowd = this;
            }
        }
    }
}