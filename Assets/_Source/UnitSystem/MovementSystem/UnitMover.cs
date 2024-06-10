using SelectionSystem;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class UnitMover
    {
        private readonly UnitSelection _unitSelection;
        
        public UnitMover(UnitSelection unitSelection)
        {
            _unitSelection = unitSelection;
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
