using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class Path
    {
        public List<Vector3> PathPoints = new();
        private List<Unit> _units = new();
        
        public IEnumerable<Unit> Units => _units;
        public int UnitsCount => _units.Count;
        
        public event Action<Path> OnDestroy;
        
        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
            if(_units.Count == 0)
            {
                DestroyPath();
            }
        }

        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
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