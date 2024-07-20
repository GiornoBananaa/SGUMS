using System;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace UnitGroupingSystem
{
    public class Group : Crowd
    {
        public Quaternion Rotation;
        public Unit PivotUnit;
        public bool Rotatable = true;
        
        private HashSet<Unit> _unitsStartedMove = new();
        
        public event Action<Group> OnMoveStart;
        
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

        public void AddUnitStartedMove(Unit unit)
        {
            _unitsStartedMove.Add(unit);
            if(_unitsStartedMove.Count >= Units.Count)
            {
                _unitsStartedMove.Clear();
                OnMoveStart?.Invoke(this);
            }
        }
        
        public void Disband()
        {
            Units.Clear();
            OnDisband?.Invoke();
        }
    }
}