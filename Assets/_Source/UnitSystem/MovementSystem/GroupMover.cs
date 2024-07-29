using System.Linq;
using UnitGroupingSystem;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class GroupMover : IPathMover<Group>
    {
        public void MoveOnPath(Group group, Path path)
        {

            if (group.Path != null)
            {
                group.Path.RemoveUnits(group.Units);
                group.UpdatePath = true;
            }
            path.AddUnits(group.Units);
            group.Path = path;
            group.PathPointIndex = 0;
            UpdateGroupPath(group);
        }
            
        public void StopOnPath(Group group)
        {
            group.OnDestinationReached -= UpdateGroupPath;
            foreach (var unit in group.Units)
            {
                unit.NavMeshAgent.ResetPath();
            }
        }

        private bool CheckLaggingUnits(Group group)
        {
            foreach (var unit in group.LaggingUnits.ToList().Where(unit => !unit.IsMoving && unit.CombatMode))
            {
                group.LaggingUnits.Remove(unit);
            }
            return group.LaggingUnits.Count > 0;
        }
        
        private void UpdateGroupPath(Group group)
        {
            group.OnDestinationReached -= UpdateGroupPath;
            if (group.Path == null) return;
            CheckLaggingUnits(group);
            if (group.PathPointIndex >= group.Path.PathPoints.Count)
            {
                if(group.LaggingUnits.Count != 0)
                    return;
                group.Path.RemoveUnits(group.Units);
                group.Path = null;
                return;
            }
            Vector3 destination = group.Path.PathPoints[group.PathPointIndex];
            Quaternion rotation = Quaternion.identity;
            if (group.Rotatable)
            {
                Vector3 pivot = group.PathPointIndex == 0 ? group.GroupCenter : group.Path.PathPoints[group.PathPointIndex - 1];
                    
                rotation = Quaternion.Euler(0, Quaternion.LookRotation(
                    pivot - destination).eulerAngles.y, 0);
            }
            
            int formationPosition = 0;
            foreach (var unit in group.Units)
            {
                Vector3 pathOffset = group.Formation.Positions[formationPosition];
                formationPosition++;
                var offset = new Vector3(pathOffset.x, 0, pathOffset.y) +
                             new Vector3(group.Offset.x, 0, group.Offset.y);
                offset = rotation * offset;
                Vector3 unitDestination = destination + offset;
                unit.PathOffset = offset;
                unit.PathPointIndex = group.PathPointIndex+1;
                if(group.LaggingUnits.Contains(unit) && !group.UpdatePath) continue;
                unit.NavMeshAgent.SetDestination(unitDestination);
                unit.StartNavigationTracking();
                unit.OnDestinationReached += group.AddUnitReachedDestination;
            }

            if (group.UpdatePath) group.UpdatePath = false;
            group.OnDestinationReached += UpdateGroupPath;
            group.OnMoveStart?.Invoke(group);
            group.PathPointIndex += 1;
        }
    }
}