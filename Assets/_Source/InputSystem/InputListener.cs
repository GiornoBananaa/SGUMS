using SelectionSystem;
using SelectionSystem.AreaSelectionSystem;
using UnitFormationSystem;
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
        private FormationDrawer _formationDrawer;
        private bool _dragSelection;
        private bool _formationDrawing;
        private bool _pathDrawing;
        private bool _pathDrawingEnabled = true;
        private bool _formationDrawingEnabled = false;

        [Inject]
        public void Construct(UnitSelection unitSelection,GroupSelection groupSelection, 
            AreaSelector areaSelector, PathCreator pathCreator, FormationDrawer formationDrawer)
        {
            _unitSelection = unitSelection;
            _areaSelector = areaSelector;
            _groupSelection = groupSelection;
            _pathCreator = pathCreator;
            _formationDrawer = formationDrawer;
        }
        
        private void Awake()
        {
            _gameInput = new GameInputActions();
            _gameInput.Enable();
            EnableInput();
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            if(_dragSelection)
                DragSelection();
            if(_pathDrawing)
                DrawPath();
            if(_formationDrawing)
                DrawFormation();
        }
        
        
        private void OnDestroy()
        {
            DisableInput();
        }

        public void EnableInput()
        {
            _gameInput.GlobalActionMap.SelectUnit.started += SelectUnit;
            _gameInput.GlobalActionMap.GroupSelection.started += EnableMultiSelection;
            _gameInput.GlobalActionMap.GroupSelection.canceled += DisableMultiSelection;
            _gameInput.GlobalActionMap.AreaSelection.started += StartAreaSelection;
            _gameInput.GlobalActionMap.AreaSelection.canceled += EndAreaSelection;
            _gameInput.GlobalActionMap.DrawPath.started += StartPathDraw;
            _gameInput.GlobalActionMap.DrawPath.canceled += EndPathDraw;
            _gameInput.GlobalActionMap.DrawFormation.started += StartFormationDraw;
            _gameInput.GlobalActionMap.DrawFormation.canceled += EndFormationDraw;
            _gameInput.Enable();
        }
        
        public void DisableInput()
        {
            _gameInput.GlobalActionMap.SelectUnit.started -= SelectUnit;
            _gameInput.GlobalActionMap.GroupSelection.started -= EnableMultiSelection;
            _gameInput.GlobalActionMap.GroupSelection.canceled -= DisableMultiSelection;
            _gameInput.GlobalActionMap.AreaSelection.started -= StartAreaSelection;
            _gameInput.GlobalActionMap.AreaSelection.canceled -= EndAreaSelection;
            _gameInput.GlobalActionMap.DrawPath.started -= StartPathDraw;
            _gameInput.GlobalActionMap.DrawPath.canceled -= EndPathDraw;
            _gameInput.GlobalActionMap.DrawFormation.started -= StartFormationDraw;
            _gameInput.GlobalActionMap.DrawFormation.canceled -= EndFormationDraw;
            _gameInput.Disable();
        }
        
        private void EnableMultiSelection(InputAction.CallbackContext callbackContext)
        {
            _unitSelection.MultiSelection = true;
            _groupSelection.MultiSelection = true;
        }
        
        private void DisableMultiSelection(InputAction.CallbackContext callbackContext)
        {
            _unitSelection.MultiSelection = false;
            _groupSelection.MultiSelection = false;
        }
        
        private void SelectUnit(InputAction.CallbackContext callbackContext)
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit) || EventSystem.current.IsPointerOverGameObject()) return;
            
            if (_selectableLayerMask.ContainsLayer(hit.collider.gameObject.layer))
            {
                _unitSelection.Select(hit.collider.gameObject.GetComponent<Unit>());
            }
            else
            {
                if(!_groupSelection.MultiSelection)
                    _groupSelection.DeselectAll();
                if (!_unitSelection.MultiSelection)
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

        public void EnableFormationDrawing()
        {
            _formationDrawingEnabled = true;
            _pathDrawingEnabled = false;
        }
        
        public void DisableFormationDrawing()
        {
            _formationDrawingEnabled = false;
            _pathDrawingEnabled = true;
        }
        
        private void StartPathDraw(InputAction.CallbackContext callbackContext)
        {
            if(!_pathDrawingEnabled || _unitSelection.SelectedCount == 0) return;
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _pathCreator.StartPathCreation();
            _pathDrawing = true;
        }
        
        private void DrawPath()
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _pathCreator.AddPathPoint(hit.point);
        }
        
        private void EndPathDraw(InputAction.CallbackContext callbackContext)
        {
            if(!_pathDrawing) return;
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _pathDrawing = false;
            _pathCreator.EndPathCreation();
        }
        
        private void StartFormationDraw(InputAction.CallbackContext callbackContext)
        {
            if(!_formationDrawingEnabled) return;
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            if(_pathDrawingEnabled || _unitSelection.SelectedCount == 0) return;
            
            _formationDrawer.DrawStartPoint();
            _formationDrawing = true;
        }
        
        private void DrawFormation()
        {
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _formationDrawer.DrawPoint(hit.point);
        }
        
        private void EndFormationDraw(InputAction.CallbackContext callbackContext)
        {
            if(!_formationDrawing) return;
            if (!ReadObjectUnderMouse(out RaycastHit hit, _groundLayerMask) || EventSystem.current.IsPointerOverGameObject()) return;
            
            _formationDrawing = false;
            _formationDrawer.DrawLineEnd();
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
            if (Physics.Raycast(_camera.ScreenPointToRay(mousePosition), out hit, 1000, layerMask))
                return true;
            return false;
        }
    }
}
