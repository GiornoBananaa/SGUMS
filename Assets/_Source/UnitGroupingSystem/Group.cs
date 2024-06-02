using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace UnitGroupingSystem
{
    public class Group
    {
        public HashSet<Unit> Units;

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
        
        public Group() => Units = new();
        
        public Group(IEnumerable<Unit> units)
        {
            Units = new HashSet<Unit>(units);
        }
    }
}