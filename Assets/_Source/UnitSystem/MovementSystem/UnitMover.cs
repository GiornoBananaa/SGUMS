using System;
using System.Collections.Generic;
using SelectionSystem;
using UnitFormationSystem;
using UnitGroupingSystem;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UnitSystem.MovementSystem
{
    public class UnitMover: IDisposable
    {
        private readonly UnitSelection _unitSelection;
        private readonly PathCreator _pathCreator;
        private readonly FormationSetter _formationSetter;
        private readonly FormationPlacer _formationPlacer;
        private readonly GroupSpeedEqualizer _groupSpeedEqualizer;

        public UnitMover(UnitSelection unitSelection, PathCreator pathCreator, FormationSetter formationSetter,
            FormationPlacer formationPlacer, GroupSpeedEqualizer groupSpeedEqualizer)
        {
            _unitSelection = unitSelection;
            _pathCreator = pathCreator;
            _formationSetter = formationSetter;
            _formationPlacer = formationPlacer;
            _groupSpeedEqualizer = groupSpeedEqualizer;
            _pathCreator.OnPathCreate += MoveOnPath;
        }
        
        private void MoveOnPath(Path path)
        {
            path.OnDestroy += StopOnPath;
            
            List<Unit> unitsWithoutGroup = new List<Unit>();
            HashSet<Crowd> selectedCrowds = new HashSet<Crowd>();
            
            foreach (var unit in _unitSelection.Selected)
            {
                if (unit.UnitCrowd is not Group)
                {
                    unitsWithoutGroup.Add(unit);
                }
                else
                {
                    unit.UnitCrowd.Offset = Vector2.zero;
                    selectedCrowds.Add(unit.UnitCrowd);
                    
                    if(unit.UnitCrowd.Formation == null)
                    {
                        _formationSetter.EnterFormation(new[]
                        {
                            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)
                        }, unit.UnitCrowd);
                    }
                }
            }
            
            if(unitsWithoutGroup.Count > 0)
            {
                Crowd crowd = new Crowd(unitsWithoutGroup);
                _formationSetter.EnterFormation(new[]
                {
                    new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)
                }, crowd);
                selectedCrowds.Add(unitsWithoutGroup[0].UnitCrowd);
            }
            
            _formationPlacer.PlaceFormations(selectedCrowds);
            foreach (var crowd in selectedCrowds)
            {
                foreach (var unit in crowd.Units)
                {
                    PutOnPath(unit,path);
                }
            }

            foreach (var unit in unitsWithoutGroup)
            {
                PutOnPath(unit,path);
            }
        }

        private void PutOnPath(Unit unit, Path path)
        {
            if(unit.Path != null)
            {
                unit.Path.RemoveUnit(unit);
                if (unit.Path.UnitsCount == 0)
                {
                    _pathCreator.DestroyPath(unit.Path);
                }
            }
            unit.Path = path;
            path.AddUnit(unit);
            unit.PathPointIndex = 0;
            UpdateUnitPath(unit);
        }
        
        private void StopOnPath(Path path)
        {
            path.OnDestroy -= StopOnPath;
            foreach (var unit in path.Units)
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
            else if(unit.Path == null)
            {
                return;
            }
            else
            {
                if (unit.PathPointIndex >= unit.Path.PathPoints.Count)
                {
                    unit.Path.RemoveUnit(unit);
                    if (unit.Path.UnitsCount == 0)
                    {
                        _pathCreator.DestroyPath(unit.Path);
                    }

                    unit.Path = null;
                    return;
                }

                var offset = new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y) +
                             new Vector3(unit.UnitCrowd.Offset.x, 0, unit.UnitCrowd.Offset.y);
                
                if (unit.UnitCrowd is Group { Rotatable: true } group && unit != group.PivotUnit)
                {
                    Vector3 pivot = group.PivotUnit.Path != null ? 
                        group.PivotUnit.Path.PathPoints[group.PivotUnit.PathPointIndex - 1] 
                        : group.PivotUnit.LastPathPoint;
                    
                    group.Rotation = Quaternion.Euler(0, Quaternion.LookRotation(
                        pivot - group.PivotUnit.transform.position).eulerAngles.y, 0);
                    offset = group.Rotation * offset;
                }
                
                destination = unit.Path.PathPoints[unit.PathPointIndex] + offset;
                unit.LastPathPoint = unit.Path.PathPoints[unit.PathPointIndex];
                unit.PathPointIndex += 1;
            }

            unit.NavMeshAgent.speed = unit.Stats.Speed;
            unit.NavMeshAgent.SetDestination(destination);
            unit.OnDestinationReached += UpdateUnitPath;
            unit.StartNavigationTracking();
            
            if (unit.UnitCrowd is Group group2)
            {
                group2.AddUnitStartedMove(unit);
            }
        }
        
        public void MoveToPoint(Vector3 point)
        {
            foreach (var unit in _unitSelection.Selected)
            {
                unit.NavMeshAgent.SetDestination(point 
                                                 + new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y) 
                                                 + new Vector3(unit.UnitCrowd.Offset.x, 0, unit.UnitCrowd.Offset.y));
            }
        }
        
        public void Dispose()
        {
            _pathCreator.OnPathCreate -= MoveOnPath;
        }

        public void FollowTarget(Unit unit, Transform transform, float range)
        {
            unit.CombatMode = true;
            unit.Target = transform;
            unit.TargetOffset = (unit.transform.position - transform.position).normalized * range;
            UpdateUnitPath(unit);
        }
        
        public void UnFollowTarget(Unit unit)
        {
            bool updated = unit.Target != null;
            unit.Target = null;
            if(updated)
            {
                unit.CombatMode = false;
                UpdateUnitPath(unit);
            }
        }
    }
}
