using UnitSystem;
using UnityEngine;
using Utils;
using Zenject;

namespace SelectionSystem.AreaSelectionSystem
{
    public class AreaSelector : IAreaSelector
    {
        private Vector2 _areaStart;
        private Vector2 _dragPoint;
        private IAreaSelectionView _areaSelectionView;
        private UnitSelection _unitSelection;
        private UnitContainer _unitContainer;
        private UnitSelectionDataSO _unitSelectionData;

        [Inject]
        public void Construct(IAreaSelectionView areaSelectionView, UnitSelection unitSelection, 
            UnitContainer unitContainer, UnitSelectionDataSO unitSelectionData)
        {
            _areaSelectionView = areaSelectionView;
            _unitSelection = unitSelection;
            _unitContainer = unitContainer;
            _unitSelectionData = unitSelectionData;
        }
        
        public void StartSelection(Vector2 startPoint)
        {
            _areaStart = startPoint;
            _areaSelectionView.EnableView(startPoint);
        }
        
        public void SetDragPoint(Vector2 dragPoint)
        {
            _dragPoint = dragPoint;
            _areaSelectionView.SetDragPoint(dragPoint);
        }
        
        public void EndSelection()
        {
            _areaSelectionView.DisableView();
            if(_dragPoint != _areaStart)
                SelectAllInArea();
        }

        private void SelectAllInArea()
        {
            Camera camera = Camera.main;
            
            Physics.Raycast(camera.ScreenPointToRay(_areaStart),out RaycastHit hit1, 
                1000, _unitSelectionData.LayersOfAreaSelectionRayCastTarget);
            Physics.Raycast(camera.ScreenPointToRay(new Vector3(_areaStart.x,_dragPoint.y,0)),out RaycastHit hit2, 
                1000, _unitSelectionData.LayersOfAreaSelectionRayCastTarget);
            Physics.Raycast(camera.ScreenPointToRay(new Vector3(_dragPoint.x, _areaStart.y,0)),out RaycastHit hit3, 
                1000, _unitSelectionData.LayersOfAreaSelectionRayCastTarget);
            Physics.Raycast(camera.ScreenPointToRay(_dragPoint),out RaycastHit hit4, 
                1000, _unitSelectionData.LayersOfAreaSelectionRayCastTarget);
            
            foreach (var unit in _unitContainer.PlayerUnits)
            {
                if (MathUtils.TriangleContainsPoint(new Vector2(unit.transform.position.x,unit.transform.position.z) ,new Vector2(hit1.point.x,hit1.point.z) , 
                                                    new Vector2(hit4.point.x,hit4.point.z),new Vector2(hit2.point.x, hit2.point.z) ) 
                    || MathUtils.TriangleContainsPoint(new Vector2(unit.transform.position.x,unit.transform.position.z), new Vector2(hit1.point.x,hit1.point.z), 
                                                    new Vector2(hit4.point.x,hit4.point.z), new Vector2(hit3.point.x, hit3.point.z)))
                {
                    _unitSelection.Select(unit);
                }
            }
        }
    }
}
