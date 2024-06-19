using System;
using UnityEngine;
using Zenject;

namespace UnitFormationSystem
{
    public class FormationDrawer
    {
        private const float LINE_HEIGHT = 0.2f;
        private const float MIN_POINT_DISTANCE = 0.5f;
        private readonly Material _lineMaterial;
        private LineRenderer _lineRenderer;
        private Vector3 _lastPoint;

        public Action<LineRenderer> OnLineDrawn;
        
        [Inject]
        public FormationDrawer(FormationSettingDataSO formationSettingData)
        {
            _lineMaterial = formationSettingData.FormationDrawerMaterial;
        }
        
        public void DrawStartPoint()
        {
            CreateLineComponents();
            ShowLine();
        }
        
        public void DrawPoint(Vector3 point)
        {
            var newPositionsCount = _lineRenderer.positionCount + 1;
            if(newPositionsCount != 0 && Vector3.Distance(point, _lastPoint) < MIN_POINT_DISTANCE) return;
            _lastPoint = point;
            _lineRenderer.positionCount = newPositionsCount == 1 ? 2 : newPositionsCount;
            
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 2, point + new Vector3(0, LINE_HEIGHT, 0));
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _lineRenderer.GetPosition(0));
        }

        public void DrawLineEnd()
        {
            HideLine();
            _lineRenderer.positionCount -= 1;
            OnLineDrawn?.Invoke(_lineRenderer);
        }
        
        private void HideLine()
        {
            _lineRenderer.gameObject.SetActive(false);
        }
        
        private void ShowLine()
        {
            _lineRenderer.gameObject.SetActive(true);
        }
        
        private void CreateLineComponents()
        {
            if(_lineRenderer == null) 
                _lineRenderer = new GameObject().AddComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
            _lineRenderer.textureMode = LineTextureMode.Tile;
            _lineRenderer.textureScale = new Vector2(0.005f, 1);
            _lineRenderer.material = _lineMaterial;
        }
    }
}