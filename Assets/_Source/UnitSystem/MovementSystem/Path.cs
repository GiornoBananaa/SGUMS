using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class Path
    {
        public List<Vector3> PathPoints = new();
        private List<Unit> _units = new();
        
        public event Action<Path> OnDestroy;
        
        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
            if (unit.Path == this)
                unit.Path = null;
            if(_units.Count == 0)
            {
                DestroyPath();
            }
        }
        
        public void RemoveUnits(IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                RemoveUnit(unit);
            }
        }
        
        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
            unit.Path = this;
        }
        
        public void AddUnits(IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                AddUnit(unit);
            }
        }

        public void DestroyPath()
        {
            OnDestroy?.Invoke(this);
            _units.Clear();
        }
    }
}