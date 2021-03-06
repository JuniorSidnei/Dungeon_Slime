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
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""db3a3e62-665d-4195-8c22-bacbecef64dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""d8b46b82-0b98-492d-98e2-ba229fdb0444"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""pause_game"",
                    ""type"": ""Button"",
                    ""id"": ""bdbd50cd-bc0b-4800-997e-4d9c229de8d5"",
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
                    ""name"": ""stick"",
                    ""id"": ""a2daa69e-7f92-4275-81f6-30a86fdaa6e8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1532c3e1-2970-4270-a9af-4bd6e0261c47"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7cabbbfc-f772-4875-a481-ad5b5886df32"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""948a19ff-3c44-4ebe-bcbc-71e0cc00f0e0"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b8807771-c33b-49d4-ae82-5e4afabe8e80"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""d-pad"",
                    ""id"": ""94966df4-e78d-4683-abc6-8bc3c82f3124"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""77939dd9-1d45-4f5f-aff7-a98e4e4e4bf8"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""87a40fcd-c777-4e9b-b124-3bd4ef7e2a99"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""90eff533-4737-4e71-9e76-19db5ebee53a"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b2ddd5fd-6499-4f77-a4e8-c94a8113e686"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
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
                    ""id"": ""cb2a4213-db0c-4d1b-9208-1302f467ad39"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""restart_game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f47269b4-7344-4cb7-b74f-32674c2a546b"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""beccfcfb-0729-4e5b-b2b5-ebd516dd72ae"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9971b45-51d4-4bc9-a743-967a4a3b4cf5"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ed4837e-de15-41a7-b99f-f2ec425327f5"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f047557c-b838-41c1-805b-579dc7266f49"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""pause_game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71f0ee30-6b94-4c45-b4a7-7e51a4a92ca0"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""pause_game"",
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
        },
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
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
        m_Slime_Submit = m_Slime.FindAction("Submit", throwIfNotFound: true);
        m_Slime_Cancel = m_Slime.FindAction("Cancel", throwIfNotFound: true);
        m_Slime_pause_game = m_Slime.FindAction("pause_game", throwIfNotFound: true);
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
    private readonly InputAction m_Slime_Submit;
    private readonly InputAction m_Slime_Cancel;
    private readonly InputAction m_Slime_pause_game;
    public struct SlimeActions
    {
        private @InputSource m_Wrapper;
        public SlimeActions(@InputSource wrapper) { m_Wrapper = wrapper; }
        public InputAction @movement => m_Wrapper.m_Slime_movement;
        public InputAction @restart_game => m_Wrapper.m_Slime_restart_game;
        public InputAction @Submit => m_Wrapper.m_Slime_Submit;
        public InputAction @Cancel => m_Wrapper.m_Slime_Cancel;
        public InputAction @pause_game => m_Wrapper.m_Slime_pause_game;
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
                @Submit.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnCancel;
                @pause_game.started -= m_Wrapper.m_SlimeActionsCallbackInterface.OnPause_game;
                @pause_game.performed -= m_Wrapper.m_SlimeActionsCallbackInterface.OnPause_game;
                @pause_game.canceled -= m_Wrapper.m_SlimeActionsCallbackInterface.OnPause_game;
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
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @pause_game.started += instance.OnPause_game;
                @pause_game.performed += instance.OnPause_game;
                @pause_game.canceled += instance.OnPause_game;
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
    private int m_GamePadSchemeIndex = -1;
    public InputControlScheme GamePadScheme
    {
        get
        {
            if (m_GamePadSchemeIndex == -1) m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
            return asset.controlSchemes[m_GamePadSchemeIndex];
        }
    }
    public interface ISlimeActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRestart_game(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPause_game(InputAction.CallbackContext context);
    }
}
