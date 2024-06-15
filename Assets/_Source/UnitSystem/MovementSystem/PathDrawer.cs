using System;
using System.Collections.Generic;
using System.Linq;
using SelectionSystem;
using UnitGroupingSystem;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace UnitSystem.MovementSystem
{
    public class PathDrawer : IDisposable
    {
        private const float LINE_HEIGHT = 0.2f;
        private readonly Dictionary<Path, PathView> _pathsViews = new();
        private readonly ObjectPool<PathView> _pathViewsPool;
        private readonly Material _lineMaterial;
        private readonly Material _endProjectorMaterial;
        private readonly LayerMask _projectorIgnoreLayers;
        private readonly UnitSelection _unitSelection;
        private readonly GroupSelection _groupSelection;
        private PathView _currentPathView;

        [Inject]
        public PathDrawer(PathDataSO pathDataSO, UnitSelection unitSelection, GroupSelection groupSelection)
        {
            _lineMaterial = pathDataSO.PathMaterial;
            _endProjectorMaterial = pathDataSO.EndPointMaterial;
            _projectorIgnoreLayers = pathDataSO.ProjectorIgnoreLayers;
            _unitSelection = unitSelection;
            _groupSelection = groupSelection;
            _pathViewsPool = new ObjectPool<PathView>(CreatePathLine, collectionCheck:false, defaultCapacity:5, maxSize: 1000);
            _unitSelection.OnUnitSelect += OnUnitSelection;
            _unitSelection.OnUnitDeselect += OnUnitDeselection;
            _groupSelection.OnGroupSelect += OnGroupSelection;
            _groupSelection.OnGroupDeselect += OnGroupDeselection;
        }
        
        public void DrawStartPoint(Path path)
        {
            _currentPathView = _pathViewsPool.Get();
            _currentPathView.LineRenderer.positionCount = 0;
            _currentPathView.EndProjector.gameObject.SetActive(false);
            _currentPathView.LineRenderer.gameObject.SetActive(true);
            _pathsViews.Add(path, _currentPathView);
        }
        
        public void DrawPoint(Vector3 point)
        {
            var newPositionsCount = _currentPathView.LineRenderer.positionCount + 1;
            _currentPathView.LineRenderer.positionCount = newPositionsCount;
            _currentPathView.LineRenderer.SetPosition(newPositionsCount - 1, point + new Vector3(0, LINE_HEIGHT, 0));
            
            _currentPathView.EndProjector.transform.position =
                _currentPathView.LineRenderer.GetPosition(_currentPathView.LineRenderer.positionCount - 1);
        }

        public void DrawPathEnd(Vector3 point)
        {
            _currentPathView.EndProjector.transform.position =
                point + new Vector3(0, LINE_HEIGHT, 0);
            _currentPathView.EndProjector.gameObject.SetActive(true);
        }
        
        private void HidePath(Path path)
        {
            if(!_pathsViews.TryGetValue(path, out PathView pathView)) return;
            pathView.LineRenderer.gameObject.SetActive(false);
        }
        
        private void ShowPath(Path path)
        {
            if(!_pathsViews.TryGetValue(path, out PathView pathView)) return;
            pathView.LineRenderer.gameObject.SetActive(true);
        }

        public void DestroyPath(Path path)
        {
            if(!_pathsViews.TryGetValue(path, out PathView pathView)) return;
            _pathsViews.Remove(path);
            pathView.LineRenderer.gameObject.SetActive(false);
            _pathViewsPool.Release(pathView);
        }
        
        public bool PathIsShowed(Path path)
        {
            if (!_pathsViews.TryGetValue(path, out PathView pathView)) return false;
            return pathView.LineRenderer.gameObject.activeInHierarchy;
        }
        
        private PathView CreatePathLine()
        {
            PathView pathView = new PathView();
            
            LineRenderer line = new GameObject().AddComponent<LineRenderer>();
            line.positionCount = 0;
            line.textureMode = LineTextureMode.Tile;
            line.textureScale = new Vector2(0.005f, 1);
            line.material = _lineMaterial;
            pathView.LineRenderer = line;

            Projector endProjector = new GameObject().AddComponent<Projector>();
            endProjector.transform.parent = line.transform;
            endProjector.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            endProjector.orthographic = true;
            endProjector.orthographicSize = 1.6f;
            endProjector.material = _endProjectorMaterial;
            endProjector.ignoreLayers = _projectorIgnoreLayers;
            pathView.EndProjector = endProjector;

            return pathView;
        }

        private void OnUnitSelection(Unit unit)
        {
            if(unit.Path == null) return;
            ShowPath(unit.Path);
        }
        
        private void OnUnitDeselection(Unit unit)
        {
            if(unit.Path == null) return;
            HidePath(unit.Path);
        }
        
        private void OnGroupSelection(Group group)
        {
            if(group.Path == null) return;
            ShowPath(group.Path);
        }
        
        private void OnGroupDeselection(Group group)
        {
            if(group.Path == null) return;
            HidePath(group.Path);
        }

        public void Dispose()
        {
            _unitSelection.OnUnitSelect -= OnUnitSelection;
            _unitSelection.OnUnitDeselect -= OnUnitDeselection;
            _groupSelection.OnGroupSelect -= OnGroupSelection;
            _groupSelection.OnGroupDeselect -= OnGroupDeselection;
            _pathViewsPool?.Dispose();
        }
    }
}
