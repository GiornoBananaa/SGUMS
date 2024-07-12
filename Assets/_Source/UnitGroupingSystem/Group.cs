using System;
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
    
    public class Group : Crowd
    {
        public Quaternion Rotation;
        public bool Rotatable;
        
        public Vector3 GroupCenter
        {
            get
            {
                // TODO: rewrite temporary solution
                Vector3 sum = Vector3.zero;
                
                foreach (var unit in Units)
                {
                    sum += unit.transform.position;
                }
                
                return sum/Units.Count;
            }
        }

        public Action OnDisband;
        
        public void Disband()
        {
            Units.Clear();
            OnDisband?.Invoke();
        }
    }
}