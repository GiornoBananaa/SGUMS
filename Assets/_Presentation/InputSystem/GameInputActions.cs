//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Presentation/InputSystem/GameInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputActions"",
    ""maps"": [
        {
            ""name"": ""GlobalActionMap"",
            ""id"": ""60e346b6-c438-46a2-9b28-9c48e08390d8"",
            ""actions"": [
                {
                    ""name"": ""GroupSelection"",
                    ""type"": ""Button"",
                    ""id"": ""27f9cb56-a0d4-4649-b6c7-16958736c29a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectUnit"",
                    ""type"": ""Button"",
                    ""id"": ""81cee999-6d97-4a2c-9bf9-126ebc09d8f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ddbed40b-b263-4dab-af78-d0f5024c9fb1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectUnit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7122ea2b-d4c3-4d10-a6af-73d9b07bffa5"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GroupSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // GlobalActionMap
        m_GlobalActionMap = asset.FindActionMap("GlobalActionMap", throwIfNotFound: true);
        m_GlobalActionMap_GroupSelection = m_GlobalActionMap.FindAction("GroupSelection", throwIfNotFound: true);
        m_GlobalActionMap_SelectUnit = m_GlobalActionMap.FindAction("SelectUnit", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GlobalActionMap
    private readonly InputActionMap m_GlobalActionMap;
    private List<IGlobalActionMapActions> m_GlobalActionMapActionsCallbackInterfaces = new List<IGlobalActionMapActions>();
    private readonly InputAction m_GlobalActionMap_GroupSelection;
    private readonly InputAction m_GlobalActionMap_SelectUnit;
    public struct GlobalActionMapActions
    {
        private @GameInputActions m_Wrapper;
        public GlobalActionMapActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @GroupSelection => m_Wrapper.m_GlobalActionMap_GroupSelection;
        public InputAction @SelectUnit => m_Wrapper.m_GlobalActionMap_SelectUnit;
        public InputActionMap Get() { return m_Wrapper.m_GlobalActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActionMapActions set) { return set.Get(); }
        public void AddCallbacks(IGlobalActionMapActions instance)
        {
            if (instance == null || m_Wrapper.m_GlobalActionMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GlobalActionMapActionsCallbackInterfaces.Add(instance);
            @GroupSelection.started += instance.OnGroupSelection;
            @GroupSelection.performed += instance.OnGroupSelection;
            @GroupSelection.canceled += instance.OnGroupSelection;
            @SelectUnit.started += instance.OnSelectUnit;
            @SelectUnit.performed += instance.OnSelectUnit;
            @SelectUnit.canceled += instance.OnSelectUnit;
        }

        private void UnregisterCallbacks(IGlobalActionMapActions instance)
        {
            @GroupSelection.started -= instance.OnGroupSelection;
            @GroupSelection.performed -= instance.OnGroupSelection;
            @GroupSelection.canceled -= instance.OnGroupSelection;
            @SelectUnit.started -= instance.OnSelectUnit;
            @SelectUnit.performed -= instance.OnSelectUnit;
            @SelectUnit.canceled -= instance.OnSelectUnit;
        }

        public void RemoveCallbacks(IGlobalActionMapActions instance)
        {
            if (m_Wrapper.m_GlobalActionMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGlobalActionMapActions instance)
        {
            foreach (var item in m_Wrapper.m_GlobalActionMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GlobalActionMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GlobalActionMapActions @GlobalActionMap => new GlobalActionMapActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    public interface IGlobalActionMapActions
    {
        void OnGroupSelection(InputAction.CallbackContext context);
        void OnSelectUnit(InputAction.CallbackContext context);
    }
}