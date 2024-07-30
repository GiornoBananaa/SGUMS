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
        private const float PATH_SCREEN_POINTS_DISTANCE = 30f;
        private const float PATH_POINTS_DISTANCE = 1f;
        
        private readonly PathContainer _pathContainer;
        private readonly PathDrawer _pathDrawer;
        private readonly UnitMover _unitMover;
        private Path _formingPath;
        private Vector2 _lastScreenPoint;

        public Action<Path> OnPathCreate;
        
        private PathCreator(PathDrawer pathDrawer, PathContainer pathContainer)
        {
            _pathDrawer = pathDrawer;
            _pathContainer = pathContainer;
        }
        
        public void StartPathCreation()
        {
            _formingPath = new Path();
            _formingPath.OnDestroy += DestroyPath;
            _pathContainer.AllPaths.Add(_formingPath);
        }
        
        public void AddPathPoint(Vector3 point)
        {
            if(_formingPath == null) return;
            if (!_formingPath.PathPoints.Any())
            {
                _formingPath.PathPoints.Add(point);
                _pathDrawer.DrawStartPoint(_formingPath);
            }
            if(Vector2.Distance(_lastScreenPoint, Camera.main.WorldToScreenPoint(point)) < PATH_SCREEN_POINTS_DISTANCE
               && Vector3.Distance(_formingPath.PathPoints[^1], point) < PATH_POINTS_DISTANCE) return;
            _lastScreenPoint = Camera.main.WorldToScreenPoint(point);
            _formingPath.PathPoints.Add(point);
            _pathDrawer.DrawPoint(point);
        }
        
        public void EndPathCreation()
        {
            if(_formingPath == null) return;
            _pathDrawer.DrawPathEnd(_formingPath.PathPoints[^1]);
            OnPathCreate?.Invoke(_formingPath);
            _formingPath = null;
        }

        public void DestroyPath(Path path)
        {
            path.OnDestroy -= DestroyPath;
            _pathDrawer.DestroyPath(path);
            _pathContainer.AllPaths.Remove(path);
            path.DestroyPath();
        }
    }
}
