using System;
using System.Collections.Generic;
using SelectionSystem;
using UnitFormationSystem;
using UnitGroupingSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UnitSystem.MovementSystem
{
    public class UnitMover: IDisposable
    {
        private readonly UnitSelection _unitSelection;
        private readonly PathCreator _pathCreator;
        private readonly FormationSetter _formationSetter;
        private readonly GroupPlacer _groupPlacer;
        
        public UnitMover(UnitSelection unitSelection, PathCreator pathCreator, FormationSetter formationSetter, GroupPlacer groupPlacer)
        {
            _unitSelection = unitSelection;
            _pathCreator = pathCreator;
            _formationSetter = formationSetter;
            _groupPlacer = groupPlacer;
            _pathCreator.OnPathCreate += MoveOnPath;
        }
        
        private void MoveOnPath(Path path)
        {
            path.Units = new List<Unit>();
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
            
            _groupPlacer.PlaceFormations(selectedCrowds);
            
            foreach (var unit in _unitSelection.Selected)
            {
                if(unit.Path != null)
                {
                    unit.Path.Units.Remove(unit);
                    if (unit.Path.Units.Count == 0)
                    {
                        _pathCreator.DestroyPath(unit.Path);
                    }
                }
                unit.Path = path;
                path.Units.Add(unit);
                unit.PathPointIndex = -1;
                UpdateUnitPath(unit);
            }
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
            unit.PathPointIndex += 1;
            if ( unit.PathPointIndex >= unit.Path.PathPoints.Count)
            {
                unit.Path.Units.Remove(unit);
                if (unit.Path.Units.Count == 0)
                {
                    _pathCreator.DestroyPath(unit.Path);
                }
                unit.Path = null;
                
                return;
            }

            var offset = new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y) + new Vector3(unit.UnitCrowd.Offset.x, 0, unit.UnitCrowd.Offset.y);
            /*
            float offsetAngle = Vector3.SignedAngle(Vector3.forward, offset, Vector3.up);
            float rotationAngle = offsetAngle - Vector3.SignedAngle(Vector3.forward, unit.Path.PathPoints[unit.PathPointIndex] - unit.LastPathPoint, Vector3.up);
            offset = Quaternion.Euler(0, rotationAngle, 0) * offset;
            */
            unit.NavMeshAgent.SetDestination(unit.Path.PathPoints[unit.PathPointIndex] + offset);
            unit.OnDestinationReached += UpdateUnitPath;
            unit.StartNavigationTracking();
            unit.LastPathPoint = unit.Path.PathPoints[unit.PathPointIndex];
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
        
        public void MoveToPoint(Unit unit, Vector3 point)
        {
            unit.NavMeshAgent.SetDestination(point);
        }
        
        public void Dispose()
        {
            _pathCreator.OnPathCreate -= MoveOnPath;
        }
    }
}
