// GENERATED AUTOMATICALLY FROM 'Assets/Controls/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""e9738692-e39d-4324-832f-e098c09a2663"",
            ""actions"": [
                {
                    ""name"": ""Ability"",
                    ""type"": ""Button"",
                    ""id"": ""4363f927-b0e2-4055-a0f0-5af8be65ecf9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""2df61e73-f875-49ef-8aed-3d4e59b61d99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""37481680-06fc-4422-9797-a4f899fa570f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""924506d6-1631-4180-b534-ea4cc97509c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""9ecc4e3e-68ba-4e43-8841-fc3fa938b5ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""073179d9-2f6b-4260-849c-c71e860ffa95"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard + Mouse"",
                    ""action"": ""Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ab4f82b-c6ad-4333-bfb2-da96aee97e24"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard + Mouse"",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8bea050-68ce-4f6a-8446-c5dd3ad3ae6e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard + Mouse"",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14a13ab8-2cae-4f4f-9d90-80d5c137eb21"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard + Mouse"",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2da92caf-30f3-42b5-b561-c369aaa6d1fe"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard + Mouse"",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoard + Mouse"",
            ""bindingGroup"": ""KeyBoard + Mouse"",
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
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_Ability = m_Main.FindAction("Ability", throwIfNotFound: true);
        m_Main_MoveLeft = m_Main.FindAction("MoveLeft", throwIfNotFound: true);
        m_Main_MoveRight = m_Main.FindAction("MoveRight", throwIfNotFound: true);
        m_Main_MoveUp = m_Main.FindAction("MoveUp", throwIfNotFound: true);
        m_Main_MoveDown = m_Main.FindAction("MoveDown", throwIfNotFound: true);
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

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_Ability;
    private readonly InputAction m_Main_MoveLeft;
    private readonly InputAction m_Main_MoveRight;
    private readonly InputAction m_Main_MoveUp;
    private readonly InputAction m_Main_MoveDown;
    public struct MainActions
    {
        private @Controls m_Wrapper;
        public MainActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Ability => m_Wrapper.m_Main_Ability;
        public InputAction @MoveLeft => m_Wrapper.m_Main_MoveLeft;
        public InputAction @MoveRight => m_Wrapper.m_Main_MoveRight;
        public InputAction @MoveUp => m_Wrapper.m_Main_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_Main_MoveDown;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @Ability.started -= m_Wrapper.m_MainActionsCallbackInterface.OnAbility;
                @Ability.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnAbility;
                @Ability.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnAbility;
                @MoveLeft.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveLeft;
                @MoveRight.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveRight;
                @MoveUp.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveUp;
                @MoveUp.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveUp;
                @MoveUp.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveUp;
                @MoveDown.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMoveDown;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Ability.started += instance.OnAbility;
                @Ability.performed += instance.OnAbility;
                @Ability.canceled += instance.OnAbility;
                @MoveLeft.started += instance.OnMoveLeft;
                @MoveLeft.performed += instance.OnMoveLeft;
                @MoveLeft.canceled += instance.OnMoveLeft;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @MoveUp.started += instance.OnMoveUp;
                @MoveUp.performed += instance.OnMoveUp;
                @MoveUp.canceled += instance.OnMoveUp;
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    private int m_KeyBoardMouseSchemeIndex = -1;
    public InputControlScheme KeyBoardMouseScheme
    {
        get
        {
            if (m_KeyBoardMouseSchemeIndex == -1) m_KeyBoardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyBoard + Mouse");
            return asset.controlSchemes[m_KeyBoardMouseSchemeIndex];
        }
    }
    public interface IMainActions
    {
        void OnAbility(InputAction.CallbackContext context);
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
    }
}
