using System.Collections.Generic;
using UnityEngine;
using SelectionSystem;

namespace UnitSystem.MovementSystem
{
    public class PathCreator
    {
        private const float PATH_POINTS_DISTANCE = 0.2f;
        
        private readonly Dictionary<int, UnitPath> _paths = new();
        private readonly UnitSelection _unitSelection;
        private readonly PathView _pathView;
        private UnitPath _formingPath;
        private int _lastID;

        private PathCreator(UnitSelection unitSelection, PathView pathView)
        {
            _unitSelection = unitSelection;
            _pathView = pathView;
        }
        
        public void StartPathCreation()
        {
            _formingPath = new UnitPath();
        }
        
        public void AddPathPoint(Vector3 point)
        {
            if(Vector3.Distance(_formingPath.PathPoints[^1], point) < PATH_POINTS_DISTANCE) return;
            
            _formingPath.PathPoints.Add(point);
        }
        
        public void EndPathCreation()
        {
            _lastID += 1;
            _formingPath.Units = new List<Unit>(_unitSelection.Selected);
            _paths.Add(_lastID, _formingPath);
        }
    }
}
