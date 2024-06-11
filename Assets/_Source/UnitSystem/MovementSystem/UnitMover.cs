using System.Collections.Generic;
using SelectionSystem;
using UnityEngine;
using NavMeshCallbackSystem;

namespace UnitSystem.MovementSystem
{
    public class UnitMover
    {
        private readonly UnitSelection _unitSelection;
        
        public UnitMover(UnitSelection unitSelection)
        {
            _unitSelection = unitSelection;
        }

        public void MoveOnPath(Path path)
        {
            path.Units = new List<Unit>();
            foreach (var unit in _unitSelection.Selected)
            {
                unit.Path = path;
                unit.PathPointIndex = -1;
                UpdateUnitPath(unit);
                path.Units.Add(unit);
            }
        }
        
        private void UpdateUnitPath(Unit unit)
        {
            unit.OnDestinationReached -= UpdateUnitPath;
            unit.PathPointIndex += 1;
            if ( unit.PathPointIndex >= unit.Path.PathPoints.Count)
            {
                unit.Path = null;
                return;
            }
            unit.NavMeshAgent.SetDestination(unit.Path.PathPoints[unit.PathPointIndex]);
            unit.OnDestinationReached += UpdateUnitPath;
            unit.StartNavigationTracking();
        }
        
        public void MoveToPoint(Vector3 point)
        {
            foreach (var unit in _unitSelection.Selected)
            {
                unit.NavMeshAgent.SetDestination(point);
            }
        }
        
        public void MoveToPoint(Unit unit, Vector3 point)
        {
            unit.NavMeshAgent.SetDestination(point);
        }
    }
}
