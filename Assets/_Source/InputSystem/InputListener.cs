using System;
using SelectionSystem;
using SelectionSystem.AreaSelectionSystem;
using UnitGroupingSystem;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils;
using Zenject;

namespace InputSystem
{
    public class InputListener: MonoBehaviour
    {
        [SerializeField] private LayerMask _selectableLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;
        private GameInputActions _gameInput;
        private Camera _camera;
        private UnitSelection _unitSelection;
        private AreaSelector _areaSelector;
        private GroupSelection _groupSelection;
        private PathCreator _pathCreator;
        private UnitMover _unitMover;
        private bool _isMultiSelection;
        private bool _dragSelection;

        [Inject]
        public void Construct(UnitSelection unitSelection,GroupSelection groupSelection, 
            AreaSelector areaSelector, UnitMover unitMover, PathCreator pathCreator)
        {
            _unitSelection = unitSelection;
            _areaSelector = areaSelector;
            _groupSelection = groupSelection;
            _unitMover = unitMover;
            _pathCreator = pathCreator;
        }
        
        private void Awake()
        {
            _gameInput = new GameInputActions();
            _gameInput.Enable();
            EnableInput();
            _camera = Camera.main;
        }

        private void Update()
        {
            if(_dragSelection)
                DragSelection();
        }

        private void OnDestroy()
        {
            DisableInput();
        }

        public void EnableInput()
        {
            _gameInput.GlobalActionMap.SelectUnit.started += SelectUnit;
            _gameInput.GlobalActionMap.GroupSelection.started += EnableGroupSelection;
            _gameInput.GlobalActionMap.GroupSelection.canceled += DisableGroupSelection;
            _gameInput.GlobalActionMap.AreaSelection.started += StartAreaSelection;
            _gameInput.GlobalActionMap.AreaSelection.canceled += EndAreaSelection;
            _gameInput.GlobalActionMap.MoveUnitToPoint.canceled += MoveUnitsToPoint;
            _gameInput.Enable();
        }
        
        public void DisableInput()
        {
            _gameInput.GlobalActionMap.SelectUnit.started -= SelectUnit;
            _gameInput.GlobalActionMap.GroupSelection.started -= EnableGroupSelection;
            _gameInput.GlobalActionMap.GroupSelection.canceled -= DisableGroupSelection;
            _gameInput.GlobalActionMap.AreaSelection.started -= StartAreaSelection;
            _gameInput.GlobalActionMap.AreaSelection.canceled -= EndAreaSelection;
            _gameInput.GlobalActionMap.MoveUnitToPoint.canceled -= MoveUnitsToPoint;
            _gameInput.Disable();
        }
        
        private void EnableGroupSelection(InputAction.CallbackContext callbackContext) 
            => _isMultiSelection = true;
        
        private void DisableGroupSelection(InputAction.CallbackContext callbackContext) 
            => _isMultiSelection = false;
        
        private void SelectUnit(InputAction.CallbackContext callbackContext)
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit) || EventSystem.current.IsPointerOverGameObject()) return;
            
            if(!_isMultiSelection)
            {
                _unitSelection.DeselectAll();
                _groupSelection.DeselectAll();
            }
            if (_selectableLayerMask.ContainsLayer(hit.collider.gameObject.layer))
            {
                _unitSelection.Select(hit.collider.gameObject.GetComponent<Unit>());
            }
            else if (!_isMultiSelection)
            {
                _unitSelection.DeselectAll();
                _groupSelection.DeselectAll();
            }
        }
        
        private void StartAreaSelection(InputAction.CallbackContext callbackContext)
        {
            _areaSelector.StartSelection(Mouse.current.position.ReadValue());
            _dragSelection = true;
        }
        
        private void DragSelection()
        {
            _areaSelector.SetDragPoint(Mouse.current.position.ReadValue());
        }
        
        private void EndAreaSelection(InputAction.CallbackContext callbackContext)
        {
            _areaSelector.EndSelection();
            _dragSelection = false;
        }
        
        private void MoveUnitsToPoint(InputAction.CallbackContext callbackContext)
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _unitMover.MoveToPoint(hit.point);
        }
        
        private void StartPathDraw(InputAction.CallbackContext callbackContext)
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _pathCreator.StartPathCreation();
        }
        
        private void DrawPath(InputAction.CallbackContext callbackContext)
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _pathCreator.AddPathPoint(hit.point);
        }
        
        private void EndPathDraw(InputAction.CallbackContext callbackContext)
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _pathCreator.EndPathCreation();
        }
        
        private bool ReadObjectUnderMouse(out RaycastHit hit)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            if (Physics.Raycast(_camera.ScreenPointToRay(mousePosition), out hit))
                return true;
            return false;
        }
        
        private bool ReadObjectUnderMouse(out RaycastHit hit, LayerMask layerMask = default)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            if (Physics.Raycast(_camera.ScreenPointToRay(mousePosition), out hit, layerMask))
                return true;
            return false;
        }
    }
}
