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
                },
                {
                    ""name"": ""level_selection_back"",
                    ""type"": ""Button"",
                    ""id"": ""8625d91e-0102-4232-b9dc-4ff3ca9ee7cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""level_selection_done"",
                    ""type"": ""Button"",
                    ""id"": ""a6270d64-51df-4585-bab7-335d3048a7c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""advance_level_index"",
                    ""type"": ""Button"",
                    ""id"": ""2c32511e-e425-453c-8b1b-ea159e185e3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""return_level_index"",
                    ""type"": ""Button"",
                    ""id"": ""f28a87f2-1240-43f6-ae47-632b6d3b60bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""move_wasd"",
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
                    ""name"": ""move_arrow_keys"",
                    ""id"": ""9fc77a0b-6f01-4d27-b245-850e9db8692e"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4b2ab282-d434-4cfa-a6e5-82b57fd878d2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ef9c4e0e-1479-4be1-8e80-3b5ade390590"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""41946e56-3713-48da-9f02-6e02ca9c129b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""680d5629-ec7b-47a9-be7f-c3cc3fe44097"",
                    ""path"": ""<Keyboard>/rightArrow"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""76a25d29-64b7-4a56-b998-46e75e0cb414"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""level_selection_back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ba00128-a0d5-420a-868c-0417a7367f57"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""level_selection_back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bc387c7-74c8-47d4-b33b-a982829cedfe"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""level_selection_done"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""097d798d-024a-4d4d-95f2-168c61a63f6a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""level_selection_done"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0331e18-fac5-4584-b246-ea76607175be"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""advance_level_index"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b000f21-59a7-441c-a951-a5da50c65008"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""return_level_index"",
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
        m_Slime_level_selection_back = m_Slime.FindAction("level_selection_back", throwIfNotFound: true);
        m_Slime_level_selection_done = m_Slime.FindAction("level_selection_done", throwIfNotFound: true);
        m_Slime_advance_level_index = m_Slime.FindAction("advance_level_index", throwIfNotFound: true);
        m_Slime_return_level_index = m_Slime.FindAction("return_level_index", throwIfNotFound: true);
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
    private readonly InputAction m_Slime_level_selection_back;
    private readonly InputAction m_Slime_level_selection_done;
    private readonly InputAction m_Slime_advance_level_index;
    private readonly InputAction m_Slime_return_level_index;
    public struct SlimeActions
    {
        private @InputSource m_Wrapper;
        public SlimeActions(@InputSource wrapper) { m_Wrapper = wrapper; }
        public InputAction @movement => m_Wrapper.m_Slime_movement;
        public InputAction @restart_game => m_Wrapper.m_Slime_restart_game;
        public InputAction @level_selection_back => m_Wrapper.m_Slime_level_selection_back;
        public InputAction @level_selection_done => m_Wrapper.m_Slime_level_selection_done;
        public InputAction @advance_level_index => m_Wrapper.m_Slime_advance_level_index;
        public InputAction @return_level_index => m_Wrapper.m_Slime_return_level_index;
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
                @level_selection_back.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnLevel_selection_back;
                @level_selection_back.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnLevel_selection_back;
                @level_selection_back.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnLevel_selection_back;
                @level_selection_done.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnLevel_selection_done;
                @level_selection_done.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnLevel_selection_done;
                @level_selection_done.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnLevel_selection_done;
                @advance_level_index.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnAdvance_level_index;
                @advance_level_index.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnAdvance_level_index;
                @advance_level_index.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnAdvance_level_index;
                @return_level_index.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnReturn_level_index;
                @return_level_index.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnReturn_level_index;
                @return_level_index.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnReturn_level_index;
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
                @level_selection_back.started += instance.OnLevel_selection_back;
                @level_selection_back.performed += instance.OnLevel_selection_back;
                @level_selection_back.canceled += instance.OnLevel_selection_back;
                @level_selection_done.started += instance.OnLevel_selection_done;
                @level_selection_done.performed += instance.OnLevel_selection_done;
                @level_selection_done.canceled += instance.OnLevel_selection_done;
                @advance_level_index.started += instance.OnAdvance_level_index;
                @advance_level_index.performed += instance.OnAdvance_level_index;
                @advance_level_index.canceled += instance.OnAdvance_level_index;
                @return_level_index.started += instance.OnReturn_level_index;
                @return_level_index.performed += instance.OnReturn_level_index;
                @return_level_index.canceled += instance.OnReturn_level_index;
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
        void OnLevel_selection_back(InputAction.CallbackContext context);
        void OnLevel_selection_done(InputAction.CallbackContext context);
        void OnAdvance_level_index(InputAction.CallbackContext context);
        void OnReturn_level_index(InputAction.CallbackContext context);
    }
}
