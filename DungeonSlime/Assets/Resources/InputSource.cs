// GENERATED AUTOMATICALLY FROM 'Assets/Resources/InputSource.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputSource : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSource()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSource"",
    ""maps"": [
        {
            ""name"": ""Slime"",
            ""id"": ""5f829cc9-d294-47cc-ac2e-edead77f7c17"",
            ""actions"": [
                {
                    ""name"": ""movement"",
                    ""type"": ""Button"",
                    ""id"": ""37aa8250-da1a-4840-8a3f-423174ba96e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""restart_game"",
                    ""type"": ""Button"",
                    ""id"": ""c567c920-7555-458d-9526-8455b5260972"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""move"",
                    ""id"": ""d339f654-d2af-4659-89cd-ecd27dba5b6c"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f8c576c6-eb84-419c-b59c-18513eac7c84"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d3a7c865-bc38-4232-a471-cf79879d089d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b4d2d8e4-d71c-4e69-9898-daf3158ea3cc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""94e6263a-805f-4dc8-ada7-75c6f46a5a28"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2caf8adf-b2c3-4a27-b6a8-51fc27016b95"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""restart_game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Slime
        m_Slime = asset.FindActionMap("Slime", throwIfNotFound: true);
        m_Slime_movement = m_Slime.FindAction("movement", throwIfNotFound: true);
        m_Slime_restart_game = m_Slime.FindAction("restart_game", throwIfNotFound: true);
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

    // Slime
    private readonly InputActionMap m_Slime;
    private ISlimeActions m_SlimeActionsCallbackInterface;
    private readonly InputAction m_Slime_movement;
    private readonly InputAction m_Slime_restart_game;
    public struct SlimeActions
    {
        private @InputSource m_Wrapper;
        public SlimeActions(@InputSource wrapper) { m_Wrapper = wrapper; }
        public InputAction @movement => m_Wrapper.m_Slime_movement;
        public InputAction @restart_game => m_Wrapper.m_Slime_restart_game;
        public InputActionMap Get() { return m_Wrapper.m_Slime; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SlimeActions set) { return set.Get(); }
        public void SetCallbacks(ISlimeActions instance)
        {
            if (m_Wrapper.m_SlimeActionsCallbackInterface != null)
            {
                @movement.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnMovement;
                @movement.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnMovement;
                @movement.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnMovement;
                @restart_game.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnRestart_game;
                @restart_game.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnRestart_game;
                @restart_game.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnRestart_game;
            }
            m_Wrapper.m_SlimeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @movement.started += instance.OnMovement;
                @movement.performed += instance.OnMovement;
                @movement.canceled += instance.OnMovement;
                @restart_game.started += instance.OnRestart_game;
                @restart_game.performed += instance.OnRestart_game;
                @restart_game.canceled += instance.OnRestart_game;
            }
        }
    }
    public SlimeActions @Slime => new SlimeActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface ISlimeActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRestart_game(InputAction.CallbackContext context);
    }
}
