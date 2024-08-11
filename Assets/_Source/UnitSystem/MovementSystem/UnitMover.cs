using System.Collections.Generic;
using System.Threading.Tasks;
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

        public void FollowTargetEnemy(Unit unit, Transform target, float range)
        {
            if (unit.UnitCrowd is Group group)
            {
                group.LaggingUnits.Add(unit);
                unit.OnPathEnd += group.AddUnitReachedDestination;
            }
            unit.IsFollowingEnemy = true;
            unit.NavMeshAgent.updateRotation = false;
            unit.transform.LookAt(target);
            unit.TargetEnemy = target;
            Vector3 offset = (unit.transform.position - target.position).normalized * range;
            unit.TargetOffset = new Vector2(offset.x, offset.z);
            unit.EndNavigationTracking();
            FollowEnemyUpdate(unit);
        }
        
        public void UnFollowTargetEnemy(Unit unit)
        {
            bool updated = unit.IsFollowingEnemy;
            unit.TargetEnemy = null;
            if (!updated) return;
            unit.IsFollowingEnemy = false;
            unit.NavMeshAgent.updateRotation = true;
            if (unit.Path != null && unit.PathPointIndex - 1 >= 0)
            {
                unit.NavMeshAgent.SetDestination(unit.Path.PathPoints[unit.PathPointIndex-1] 
                                                 + new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y));
                unit.StartNavigationTracking();
            }
        }

        private async void FollowEnemyUpdate(Unit unit)
        {
            while (unit != null|| unit.IsFollowingEnemy || unit.TargetEnemy != null)
            {
                Vector3 destination = unit.TargetEnemy.transform.position +
                                      new Vector3(unit.TargetOffset.x, 0, unit.TargetOffset.y);
                unit.NavMeshAgent.SetDestination(destination);
                await Task.Delay(200);
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
            
            if(unit.Health.IsDead || unit.Path == null || unit.CombatMode) return;
            
            if (unit.PathPointIndex >= unit.Path.PathPoints.Count)
            {
                unit.Path.RemoveUnit(unit);
                return;
            }

            var offset = new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y) +
                         new Vector3(unit.UnitCrowd.Offset.x, 0, unit.UnitCrowd.Offset.y);
                
            Vector3 destination = unit.Path.PathPoints[unit.PathPointIndex] + offset;
            unit.PathPointIndex += 1;
            
            unit.NavMeshAgent.SetDestination(destination);
            unit.OnDestinationReached += UpdateUnitPath;
            unit.StartNavigationTracking();
        }
    }
}
