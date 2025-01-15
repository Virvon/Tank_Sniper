//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Sources/Services/InputService/InputActionSheme.inputactions
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

public partial class @InputActionSheme: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActionSheme()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActionSheme"",
    ""maps"": [
        {
            ""name"": ""GameplayInput"",
            ""id"": ""054dcbaf-f8a7-4768-a885-f030c7a45fbc"",
            ""actions"": [
                {
                    ""name"": ""AimingButtonPressed"",
                    ""type"": ""Button"",
                    ""id"": ""c077a74d-5b82-42e5-8a2b-b897ed523fc8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UndoAimingButtonPressed"",
                    ""type"": ""Button"",
                    ""id"": ""dfa7a4ae-318b-42af-b9fd-7ed6f4a6d925"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fac27b23-53c3-4c81-8a9f-7c0ce612dadd"",
                    ""path"": ""<Keyboard>/#(1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimingButtonPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""950efc78-e0a2-479f-8408-f16d9eab8efa"",
                    ""path"": ""<Keyboard>/#(2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UndoAimingButtonPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""HandleInput"",
            ""id"": ""ccc8b13e-a32a-4e33-999e-dbdaac51b6ba"",
            ""actions"": [
                {
                    ""name"": ""HandleMove"",
                    ""type"": ""Value"",
                    ""id"": ""f503de89-ab1e-4aca-8b69-f70eca07200d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""One Modifier"",
                    ""id"": ""1de50bb9-b6fb-4fc1-94b3-e6f671da8877"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandleMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""a83b55af-d5a5-41e1-98b2-ecb47e4c930a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandleMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Binding"",
                    ""id"": ""65694cb1-8576-44eb-b90f-c5a84fa5198d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandleMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""One Modifier"",
                    ""id"": ""473c2751-68a7-409f-ada3-75c9f3ae92d6"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandleMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""bb4f99bf-c7cc-4501-9450-af21b1ed5da3"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandleMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""da398dc4-2085-42e9-96f1-9004a44e104a"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandleMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameplayInput
        m_GameplayInput = asset.FindActionMap("GameplayInput", throwIfNotFound: true);
        m_GameplayInput_AimingButtonPressed = m_GameplayInput.FindAction("AimingButtonPressed", throwIfNotFound: true);
        m_GameplayInput_UndoAimingButtonPressed = m_GameplayInput.FindAction("UndoAimingButtonPressed", throwIfNotFound: true);
        // HandleInput
        m_HandleInput = asset.FindActionMap("HandleInput", throwIfNotFound: true);
        m_HandleInput_HandleMove = m_HandleInput.FindAction("HandleMove", throwIfNotFound: true);
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

    // GameplayInput
    private readonly InputActionMap m_GameplayInput;
    private List<IGameplayInputActions> m_GameplayInputActionsCallbackInterfaces = new List<IGameplayInputActions>();
    private readonly InputAction m_GameplayInput_AimingButtonPressed;
    private readonly InputAction m_GameplayInput_UndoAimingButtonPressed;
    public struct GameplayInputActions
    {
        private @InputActionSheme m_Wrapper;
        public GameplayInputActions(@InputActionSheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @AimingButtonPressed => m_Wrapper.m_GameplayInput_AimingButtonPressed;
        public InputAction @UndoAimingButtonPressed => m_Wrapper.m_GameplayInput_UndoAimingButtonPressed;
        public InputActionMap Get() { return m_Wrapper.m_GameplayInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayInputActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayInputActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Add(instance);
            @AimingButtonPressed.started += instance.OnAimingButtonPressed;
            @AimingButtonPressed.performed += instance.OnAimingButtonPressed;
            @AimingButtonPressed.canceled += instance.OnAimingButtonPressed;
            @UndoAimingButtonPressed.started += instance.OnUndoAimingButtonPressed;
            @UndoAimingButtonPressed.performed += instance.OnUndoAimingButtonPressed;
            @UndoAimingButtonPressed.canceled += instance.OnUndoAimingButtonPressed;
        }

        private void UnregisterCallbacks(IGameplayInputActions instance)
        {
            @AimingButtonPressed.started -= instance.OnAimingButtonPressed;
            @AimingButtonPressed.performed -= instance.OnAimingButtonPressed;
            @AimingButtonPressed.canceled -= instance.OnAimingButtonPressed;
            @UndoAimingButtonPressed.started -= instance.OnUndoAimingButtonPressed;
            @UndoAimingButtonPressed.performed -= instance.OnUndoAimingButtonPressed;
            @UndoAimingButtonPressed.canceled -= instance.OnUndoAimingButtonPressed;
        }

        public void RemoveCallbacks(IGameplayInputActions instance)
        {
            if (m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayInputActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayInputActions @GameplayInput => new GameplayInputActions(this);

    // HandleInput
    private readonly InputActionMap m_HandleInput;
    private List<IHandleInputActions> m_HandleInputActionsCallbackInterfaces = new List<IHandleInputActions>();
    private readonly InputAction m_HandleInput_HandleMove;
    public struct HandleInputActions
    {
        private @InputActionSheme m_Wrapper;
        public HandleInputActions(@InputActionSheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @HandleMove => m_Wrapper.m_HandleInput_HandleMove;
        public InputActionMap Get() { return m_Wrapper.m_HandleInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HandleInputActions set) { return set.Get(); }
        public void AddCallbacks(IHandleInputActions instance)
        {
            if (instance == null || m_Wrapper.m_HandleInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_HandleInputActionsCallbackInterfaces.Add(instance);
            @HandleMove.started += instance.OnHandleMove;
            @HandleMove.performed += instance.OnHandleMove;
            @HandleMove.canceled += instance.OnHandleMove;
        }

        private void UnregisterCallbacks(IHandleInputActions instance)
        {
            @HandleMove.started -= instance.OnHandleMove;
            @HandleMove.performed -= instance.OnHandleMove;
            @HandleMove.canceled -= instance.OnHandleMove;
        }

        public void RemoveCallbacks(IHandleInputActions instance)
        {
            if (m_Wrapper.m_HandleInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IHandleInputActions instance)
        {
            foreach (var item in m_Wrapper.m_HandleInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_HandleInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public HandleInputActions @HandleInput => new HandleInputActions(this);
    public interface IGameplayInputActions
    {
        void OnAimingButtonPressed(InputAction.CallbackContext context);
        void OnUndoAimingButtonPressed(InputAction.CallbackContext context);
    }
    public interface IHandleInputActions
    {
        void OnHandleMove(InputAction.CallbackContext context);
    }
}
