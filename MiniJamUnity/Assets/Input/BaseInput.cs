//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/BaseInput.inputactions
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

public partial class @BaseInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @BaseInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BaseInput"",
    ""maps"": [
        {
            ""name"": ""CartInput"",
            ""id"": ""40c7f4c6-d063-4f3e-9401-1ef6f331a11a"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""bddc30aa-f643-4be6-8135-23bb895ed16a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""891aa0b9-8dcc-45d9-ae23-b9f22c9d5425"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c880cd29-cbbc-4150-8db2-05ae31d633bd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CartInput
        m_CartInput = asset.FindActionMap("CartInput", throwIfNotFound: true);
        m_CartInput_Shoot = m_CartInput.FindAction("Shoot", throwIfNotFound: true);
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

    // CartInput
    private readonly InputActionMap m_CartInput;
    private List<ICartInputActions> m_CartInputActionsCallbackInterfaces = new List<ICartInputActions>();
    private readonly InputAction m_CartInput_Shoot;
    public struct CartInputActions
    {
        private @BaseInput m_Wrapper;
        public CartInputActions(@BaseInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_CartInput_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_CartInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CartInputActions set) { return set.Get(); }
        public void AddCallbacks(ICartInputActions instance)
        {
            if (instance == null || m_Wrapper.m_CartInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CartInputActionsCallbackInterfaces.Add(instance);
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
        }

        private void UnregisterCallbacks(ICartInputActions instance)
        {
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
        }

        public void RemoveCallbacks(ICartInputActions instance)
        {
            if (m_Wrapper.m_CartInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICartInputActions instance)
        {
            foreach (var item in m_Wrapper.m_CartInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CartInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CartInputActions @CartInput => new CartInputActions(this);
    public interface ICartInputActions
    {
        void OnShoot(InputAction.CallbackContext context);
    }
}
