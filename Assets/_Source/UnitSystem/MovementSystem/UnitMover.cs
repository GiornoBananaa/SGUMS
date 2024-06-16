using System;
using System.Collections.Generic;
using SelectionSystem;
using UnityEngine;
using NavMeshCallbackSystem;
using UnitFormationSystem;
using Unity.VisualScripting;
using UnityEngine.AI;

namespace UnitSystem.MovementSystem
{
    public class UnitMover: IDisposable
    {
        private readonly UnitSelection _unitSelection;
        private readonly PathCreator _pathCreator;
        private readonly FormationSetter _formationSetter;

        public UnitMover(UnitSelection unitSelection, PathCreator pathCreator, FormationSetter formationSetter)
        {
            _unitSelection = unitSelection;
            _pathCreator = pathCreator;
            _formationSetter = formationSetter;
            _pathCreator.OnPathCreate += MoveOnPath;
        }
        
        private void MoveOnPath(Path path)
        {
            path.Units = new List<Unit>();
            path.OnDestroy += StopOnPath;
            _formationSetter.EnterFormation(new[]
            {
                new Vector2(2,0), new Vector2(3,0), new Vector2(5,2), new Vector2(5,5),
                new Vector2(3,2), new Vector2(3,5), new Vector2(2,5), new Vector2(2,2),
                new Vector2(0,5), new Vector2(2,0)
            });
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
            unit.NavMeshAgent.SetDestination(unit.Path.PathPoints[unit.PathPointIndex] + new Vector3(unit.PathOffset.x, 0, unit.PathOffset.y));
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

        public void Dispose()
        {
            _pathCreator.OnPathCreate -= MoveOnPath;
        }
    }
}
