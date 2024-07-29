using System;
using System.Collections.Generic;
using SelectionSystem;
using UnitFormationSystem;
using UnitGroupingSystem;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class PathStarter : IDisposable
    {
        private readonly UnitSelection _unitSelection;
        private readonly PathCreator _pathCreator;
        private readonly UnitMover _unitMover;
        private readonly GroupMover _groupMover;
        private readonly FormationSetter _formationSetter;
        private readonly FormationPlacer _formationPlacer;

        public PathStarter(UnitSelection unitSelection, PathCreator pathCreator,
            UnitMover unitMover, GroupMover groupMover, FormationSetter formationSetter, FormationPlacer formationPlacer)
        {
            _unitSelection = unitSelection;
            _pathCreator = pathCreator;
            _formationSetter = formationSetter;
            _formationPlacer = formationPlacer;
            _unitMover = unitMover;
            _groupMover = groupMover;
            _pathCreator.OnPathCreate += PutOnPath;
        }

        private void PutOnPath(Path path)
        {
            List<Unit> unitsWithoutGroup = new List<Unit>();
            HashSet<Crowd> selectedCrowds = new HashSet<Crowd>();

            foreach (var unit in _unitSelection.Selected)
            {
                if (unit.UnitCrowd is Group group)
                {
                    group.Offset = Vector2.zero;
                    selectedCrowds.Add(group);

                    if (group.Formation == null)
                    {
                        _formationSetter.EnterFormation(new[]
                        {
                            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)
                        }, group);
                    }
                }
                else
                {
                    unitsWithoutGroup.Add(unit);
                }
            }
            if (unitsWithoutGroup.Count > 0)
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
                if(crowd is Group group)
                    _groupMover.MoveOnPath(group, path);
                else
                    _unitMover.MoveOnPath(crowd, path);
            }
        }
        
        public void Dispose()
        {
            _pathCreator.OnPathCreate -= PutOnPath;
        }
    }
}