using System;
using System.Collections.Generic;
using UnitSystem.MovementSystem;
using UnityEngine;
using Unit = UnitSystem.Unit;

namespace UnitGroupingSystem
{
    public class Group : Crowd, IMoving
    {
        public bool Rotatable = true;
        public HashSet<Unit> LaggingUnits = new();
        private HashSet<Unit> _unitsReachedDestination = new();
        
        
        public Path Path { get; set; }
        public bool UpdatePath { get; set; }
        public int PathPointIndex { get; set; }
        
        public Action<Group> OnMoveStart;
        public event Action<Group> OnDestinationReached;
        
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
        
        public void AddUnitReachedDestination(Unit unit)
        {
            unit.OnDestinationReached -= AddUnitReachedDestination;
            _unitsReachedDestination.Add(unit);
            if (LaggingUnits.Contains(unit))
                LaggingUnits.Remove(unit);
            if (_unitsReachedDestination.Count < Units.Count - LaggingUnits.Count) return;
            _unitsReachedDestination.Clear();
            OnDestinationReached?.Invoke(this);
        }
        
        public void Disband()
        {
            Units.Clear();
            OnDisband?.Invoke();
        }
    }
}