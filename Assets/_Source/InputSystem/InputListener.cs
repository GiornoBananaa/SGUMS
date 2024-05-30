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
        private UnitSelector _unitSelector;
        private bool _groupSelection;

        [Inject]
        public void Construct(UnitSelector unitSelector)
        {
            _unitSelector = unitSelector;
        }
        
        private void Awake()
        {
            _gameInput = new GameInputActions();
            _gameInput.Enable();
            EnableInput();
            _camera = Camera.main;
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
            _gameInput.Enable();
        }
        
        public void DisableInput()
        {
            _gameInput.GlobalActionMap.SelectUnit.started -= SelectUnit;
            _gameInput.GlobalActionMap.GroupSelection.started -= EnableGroupSelection;
            _gameInput.GlobalActionMap.GroupSelection.canceled -= DisableGroupSelection;
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
                _unitSelector.DeselectAll();
            if (_selectableLayerMask.ContainsLayer(hit.collider.gameObject.layer))
            {
                _unitSelector.Select(hit.collider.gameObject.GetComponent<ISelectable>());
            }
            else if (_groupSelection)
            {
                _unitSelector.DeselectAll();
            }
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
