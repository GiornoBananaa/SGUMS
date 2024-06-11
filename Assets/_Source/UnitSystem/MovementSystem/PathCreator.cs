using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class PathCreator
    {
        private const float PATH_POINTS_DISTANCE = 0.2f;
        
        private readonly Dictionary<int, Path> _paths = new();
        private readonly PathDrawer _pathDrawer;
        private readonly UnitMover _unitMover;
        private Path _formingPath;
        private int _lastID;
        
        private PathCreator(PathDrawer pathDrawer, UnitMover unitMover)
        {
            _pathDrawer = pathDrawer;
            _unitMover = unitMover;
        }
        
        public void StartPathCreation()
        {
            _formingPath = new Path();
        }
        
        public void AddPathPoint(Vector3 point)
        {
            if (!_formingPath.PathPoints.Any())
            {
                _formingPath.PathPoints.Add(point);
                _pathDrawer.DrawStartPoint(point);
            }
            if(Vector3.Distance(_formingPath.PathPoints[^1], point) < PATH_POINTS_DISTANCE) return;
            
            _formingPath.PathPoints.Add(point);
            _pathDrawer.DrawPoint(point);
        }
        
        public void EndPathCreation()
        {
            if(_formingPath.Units.Any())
                _lastID += 1;
            _paths.Add(_lastID, _formingPath);
            _pathDrawer.DrawEndPoint(_formingPath.PathPoints[^1]);
            _unitMover.MoveOnPath(_formingPath);
        }
    }
}
