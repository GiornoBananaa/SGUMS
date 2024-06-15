using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class PathContainer
    {
        public List<Path> AllPaths { get; } = new();
    }
    
    public class PathCreator
    {
        private const float PATH_POINTS_DISTANCE = 0.2f;
        
        private readonly PathContainer _pathContainer;
        private readonly PathDrawer _pathDrawer;
        private readonly UnitMover _unitMover;
        private Path _formingPath;

        public Action<Path> OnPathCreate;
        
        private PathCreator(PathDrawer pathDrawer, PathContainer pathContainer)
        {
            _pathDrawer = pathDrawer;
            _pathContainer = pathContainer;
        }
        
        public void StartPathCreation()
        {
            _formingPath = new Path();
            _pathContainer.AllPaths.Add(_formingPath);
        }
        
        public void AddPathPoint(Vector3 point)
        {
            if (!_formingPath.PathPoints.Any())
            {
                _formingPath.PathPoints.Add(point);
                _pathDrawer.DrawStartPoint(_formingPath);
            }
            if(Vector3.Distance(_formingPath.PathPoints[^1], point) < PATH_POINTS_DISTANCE) return;
            
            _formingPath.PathPoints.Add(point);
            _pathDrawer.DrawPoint(point);
        }
        
        public void EndPathCreation()
        {
            _pathDrawer.DrawPathEnd(_formingPath.PathPoints[^1]);
            OnPathCreate?.Invoke(_formingPath);
        }

        public void DestroyPath(Path path)
        {
            path.OnDestroy?.Invoke(path);
            path.Units.Clear();
            _pathDrawer.DestroyPath(path);
            _pathContainer.AllPaths.Remove(path);
        }
    }
}
