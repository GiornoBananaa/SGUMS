using System;
using UnitSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Zenject;

namespace InputSystem
{
    public class InputListener: MonoBehaviour
    {
        [SerializeField] private LayerMask _selectableLayerMask;
        private GameInputActions _gameInput;
        private Camera _camera;
        private UnitSelection _unitSelection;
        private AreaSelector _areaSelector;
        private bool _groupSelection;
        private bool _dragSelection;

        [Inject]
        public void Construct(UnitSelection unitSelection, AreaSelector areaSelector)
        {
            _unitSelection = unitSelection;
            _areaSelector = areaSelector;
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
            _gameInput.Enable();
        }
        
        public void DisableInput()
        {
            _gameInput.GlobalActionMap.SelectUnit.started -= SelectUnit;
            _gameInput.GlobalActionMap.GroupSelection.started -= EnableGroupSelection;
            _gameInput.GlobalActionMap.GroupSelection.canceled -= DisableGroupSelection;
            _gameInput.GlobalActionMap.AreaSelection.started -= StartAreaSelection;
            _gameInput.GlobalActionMap.AreaSelection.canceled -= EndAreaSelection;
            _gameInput.Disable();
        }

        private void EnableGroupSelection(InputAction.CallbackContext callbackContext) 
            => _groupSelection = true;
        
        private void DisableGroupSelection(InputAction.CallbackContext callbackContext) 
            => _groupSelection = false;
        
        private void SelectUnit(InputAction.CallbackContext callbackContext)
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit)) return;
            
            if(!_groupSelection) 
                _unitSelection.DeselectAll();
            if (_selectableLayerMask.ContainsLayer(hit.collider.gameObject.layer))
            {
                _unitSelection.Select(hit.collider.gameObject.GetComponent<Unit>());
            }
            else if (!_groupSelection)
            {
                _unitSelection.DeselectAll();
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
        
        private bool ReadObjectUnderMouse(out RaycastHit hit)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            if (Physics.Raycast(_camera.ScreenPointToRay(mousePosition), out hit))
                return true;
            return false;
        }
    }
}
