using System.Collections.Generic;
using UnitGroupingSystem;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace UnitSystem.MovementSystem
{
    public interface IPathMover<in T>
    {
        void MoveOnPath(T obj, Path path);
        void StopOnPath(T obj);
    }
    
    public class UnitMover : IPathMover<Crowd>
    {
        public void MoveOnPath(Crowd crowd, Path path)
        {
            foreach (var unit in crowd.Units)
            {
                unit.Path?.RemoveUnit(unit);
                path.AddUnit(unit);
                unit.PathPointIndex = 0;
                UpdateUnitPath(unit);
            }
        }
        
        public void MoveToPoint(IEnumerable<Unit> units, Vector3 point)
        {
            foreach (var unit in units)
            {
                unit.NavMeshAgent.SetDestination(point 
                                                 + new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y) 
                                                 + new Vector3(unit.UnitCrowd.Offset.x, 0, unit.UnitCrowd.Offset.y));
            }
        }

        public void FollowTarget(Unit unit, Transform transform, float range)
        {
            if (unit.UnitCrowd is Group group)
            {
                group.LaggingUnits.Add(unit);
                unit.OnPathEnd += group.AddUnitReachedDestination;
            }
            unit.CombatMode = true;
            unit.Target = transform;
            unit.TargetOffset = (unit.transform.position - transform.position).normalized * range;
            UpdateUnitPath(unit);
        }
        
        public void UnFollowTarget(Unit unit)
        {
            bool updated = unit.CombatMode;
            unit.Target = null;
            if (!updated) return;
            unit.CombatMode = false;
            if (unit.Path != null && unit.PathPointIndex - 1 >= 0)
            {
                unit.NavMeshAgent.SetDestination(unit.Path.PathPoints[unit.PathPointIndex-1] 
                                                 + new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y));
                unit.StartNavigationTracking();
            }
        }
        
        public void StopOnPath(Crowd crowd)
        {
            foreach (var unit in crowd.Units)
            {
                unit.OnDestinationReached -= UpdateUnitPath;
                unit.NavMeshAgent.ResetPath();
            }
        }
        
        private void UpdateUnitPath(Unit unit)
        {
            unit.OnDestinationReached -= UpdateUnitPath;
            if(unit.Health.IsDead) return;
            Vector3 destination;
            if (unit.Target != null)
            {
                destination = unit.Target.transform.position + (Vector3)unit.TargetOffset;
            }
            else if(unit.Path == null || unit.CombatMode)
            {
                return;
            }
            else
            {
                if (unit.PathPointIndex >= unit.Path.PathPoints.Count)
                {
                    unit.Path.RemoveUnit(unit);
                    return;
                }

                var offset = new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y) +
                             new Vector3(unit.UnitCrowd.Offset.x, 0, unit.UnitCrowd.Offset.y);
                
                destination = unit.Path.PathPoints[unit.PathPointIndex] + offset;
                unit.PathPointIndex += 1;
            }
            
            unit.NavMeshAgent.SetDestination(destination);
            unit.OnDestinationReached += UpdateUnitPath;
            unit.StartNavigationTracking();
        }
    }
}
