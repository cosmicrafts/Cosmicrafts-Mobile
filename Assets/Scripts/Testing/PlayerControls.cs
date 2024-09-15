//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.9.0
//     from Assets/Scripts/Testing/PlayerControls.inputactions
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
using UnityEngine;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Move"",
            ""id"": ""15af5709-4446-4bcd-bac4-73b598e36f33"",
            ""actions"": [
                {
                    ""name"": ""Move action"",
                    ""type"": ""Value"",
                    ""id"": ""45ed2648-0845-4064-b60a-90d4254417ae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""956d07c3-a184-4b22-9524-909a5138353e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move action"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a473e44e-e45c-4b69-a696-62e28e6cea16"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8326a438-10c8-464f-a162-29375d083672"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""87e3dc7b-9e7d-4ec9-a383-262403b2e19e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cc5e0a98-e419-4b0e-b8e0-2e6f2ba8bd58"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Dash"",
            ""id"": ""f4091d77-564f-4b67-a1e5-d00696598bde"",
            ""actions"": [
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""9266d31e-d3ff-4b6a-b6f9-1679e5ae7f78"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""17357518-acd9-4a58-808c-5f429ee9fe98"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Shoot"",
            ""id"": ""9d42c080-63c5-4778-89f3-ac8acf6d0f3a"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""2a1a3948-9acb-4055-9efb-5be7bf4d135e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0c8e4664-57ed-46ec-9e01-25d4d06f7c8f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FireLaser"",
            ""id"": ""a24f4a5c-9100-4da4-89eb-c1e942b197b9"",
            ""actions"": [
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""7b6aaf89-9b1c-4209-a0d0-489a3b860fff"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""27a114d4-890d-4bcc-9133-d92547c14f29"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Look"",
            ""id"": ""545d015c-4c83-4fe6-ab44-065265c07922"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""a2173d71-ab60-459c-b42d-3d28760c33d4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""169ce124-7137-4987-af07-ff1c61fe1bb7"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Move
        m_Move = asset.FindActionMap("Move", throwIfNotFound: true);
        m_Move_Moveaction = m_Move.FindAction("Move action", throwIfNotFound: true);
        // Dash
        m_Dash = asset.FindActionMap("Dash", throwIfNotFound: true);
        m_Dash_Dash = m_Dash.FindAction("Dash", throwIfNotFound: true);
        // Shoot
        m_Shoot = asset.FindActionMap("Shoot", throwIfNotFound: true);
        m_Shoot_Shoot = m_Shoot.FindAction("Shoot", throwIfNotFound: true);
        // FireLaser
        m_FireLaser = asset.FindActionMap("FireLaser", throwIfNotFound: true);
        m_FireLaser_Skill = m_FireLaser.FindAction("Skill", throwIfNotFound: true);
        // Look
        m_Look = asset.FindActionMap("Look", throwIfNotFound: true);
        m_Look_Look = m_Look.FindAction("Look", throwIfNotFound: true);
    }

    ~@PlayerControls()
    {
        Debug.Assert(!m_Move.enabled, "This will cause a leak and performance issues, PlayerControls.Move.Disable() has not been called.");
        Debug.Assert(!m_Dash.enabled, "This will cause a leak and performance issues, PlayerControls.Dash.Disable() has not been called.");
        Debug.Assert(!m_Shoot.enabled, "This will cause a leak and performance issues, PlayerControls.Shoot.Disable() has not been called.");
        Debug.Assert(!m_FireLaser.enabled, "This will cause a leak and performance issues, PlayerControls.FireLaser.Disable() has not been called.");
        Debug.Assert(!m_Look.enabled, "This will cause a leak and performance issues, PlayerControls.Look.Disable() has not been called.");
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

    // Move
    private readonly InputActionMap m_Move;
    private List<IMoveActions> m_MoveActionsCallbackInterfaces = new List<IMoveActions>();
    private readonly InputAction m_Move_Moveaction;
    public struct MoveActions
    {
        private @PlayerControls m_Wrapper;
        public MoveActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Moveaction => m_Wrapper.m_Move_Moveaction;
        public InputActionMap Get() { return m_Wrapper.m_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveActions set) { return set.Get(); }
        public void AddCallbacks(IMoveActions instance)
        {
            if (instance == null || m_Wrapper.m_MoveActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MoveActionsCallbackInterfaces.Add(instance);
            @Moveaction.started += instance.OnMoveaction;
            @Moveaction.performed += instance.OnMoveaction;
            @Moveaction.canceled += instance.OnMoveaction;
        }

        private void UnregisterCallbacks(IMoveActions instance)
        {
            @Moveaction.started -= instance.OnMoveaction;
            @Moveaction.performed -= instance.OnMoveaction;
            @Moveaction.canceled -= instance.OnMoveaction;
        }

        public void RemoveCallbacks(IMoveActions instance)
        {
            if (m_Wrapper.m_MoveActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMoveActions instance)
        {
            foreach (var item in m_Wrapper.m_MoveActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MoveActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MoveActions @Move => new MoveActions(this);

    // Dash
    private readonly InputActionMap m_Dash;
    private List<IDashActions> m_DashActionsCallbackInterfaces = new List<IDashActions>();
    private readonly InputAction m_Dash_Dash;
    public struct DashActions
    {
        private @PlayerControls m_Wrapper;
        public DashActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Dash => m_Wrapper.m_Dash_Dash;
        public InputActionMap Get() { return m_Wrapper.m_Dash; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DashActions set) { return set.Get(); }
        public void AddCallbacks(IDashActions instance)
        {
            if (instance == null || m_Wrapper.m_DashActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DashActionsCallbackInterfaces.Add(instance);
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
        }

        private void UnregisterCallbacks(IDashActions instance)
        {
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
        }

        public void RemoveCallbacks(IDashActions instance)
        {
            if (m_Wrapper.m_DashActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDashActions instance)
        {
            foreach (var item in m_Wrapper.m_DashActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DashActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DashActions @Dash => new DashActions(this);

    // Shoot
    private readonly InputActionMap m_Shoot;
    private List<IShootActions> m_ShootActionsCallbackInterfaces = new List<IShootActions>();
    private readonly InputAction m_Shoot_Shoot;
    public struct ShootActions
    {
        private @PlayerControls m_Wrapper;
        public ShootActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Shoot_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Shoot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShootActions set) { return set.Get(); }
        public void AddCallbacks(IShootActions instance)
        {
            if (instance == null || m_Wrapper.m_ShootActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ShootActionsCallbackInterfaces.Add(instance);
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
        }

        private void UnregisterCallbacks(IShootActions instance)
        {
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
        }

        public void RemoveCallbacks(IShootActions instance)
        {
            if (m_Wrapper.m_ShootActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IShootActions instance)
        {
            foreach (var item in m_Wrapper.m_ShootActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ShootActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ShootActions @Shoot => new ShootActions(this);

    // FireLaser
    private readonly InputActionMap m_FireLaser;
    private List<IFireLaserActions> m_FireLaserActionsCallbackInterfaces = new List<IFireLaserActions>();
    private readonly InputAction m_FireLaser_Skill;
    public struct FireLaserActions
    {
        private @PlayerControls m_Wrapper;
        public FireLaserActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Skill => m_Wrapper.m_FireLaser_Skill;
        public InputActionMap Get() { return m_Wrapper.m_FireLaser; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FireLaserActions set) { return set.Get(); }
        public void AddCallbacks(IFireLaserActions instance)
        {
            if (instance == null || m_Wrapper.m_FireLaserActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_FireLaserActionsCallbackInterfaces.Add(instance);
            @Skill.started += instance.OnSkill;
            @Skill.performed += instance.OnSkill;
            @Skill.canceled += instance.OnSkill;
        }

        private void UnregisterCallbacks(IFireLaserActions instance)
        {
            @Skill.started -= instance.OnSkill;
            @Skill.performed -= instance.OnSkill;
            @Skill.canceled -= instance.OnSkill;
        }

        public void RemoveCallbacks(IFireLaserActions instance)
        {
            if (m_Wrapper.m_FireLaserActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IFireLaserActions instance)
        {
            foreach (var item in m_Wrapper.m_FireLaserActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_FireLaserActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public FireLaserActions @FireLaser => new FireLaserActions(this);

    // Look
    private readonly InputActionMap m_Look;
    private List<ILookActions> m_LookActionsCallbackInterfaces = new List<ILookActions>();
    private readonly InputAction m_Look_Look;
    public struct LookActions
    {
        private @PlayerControls m_Wrapper;
        public LookActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Look_Look;
        public InputActionMap Get() { return m_Wrapper.m_Look; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LookActions set) { return set.Get(); }
        public void AddCallbacks(ILookActions instance)
        {
            if (instance == null || m_Wrapper.m_LookActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_LookActionsCallbackInterfaces.Add(instance);
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
        }

        private void UnregisterCallbacks(ILookActions instance)
        {
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
        }

        public void RemoveCallbacks(ILookActions instance)
        {
            if (m_Wrapper.m_LookActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ILookActions instance)
        {
            foreach (var item in m_Wrapper.m_LookActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_LookActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public LookActions @Look => new LookActions(this);
    public interface IMoveActions
    {
        void OnMoveaction(InputAction.CallbackContext context);
    }
    public interface IDashActions
    {
        void OnDash(InputAction.CallbackContext context);
    }
    public interface IShootActions
    {
        void OnShoot(InputAction.CallbackContext context);
    }
    public interface IFireLaserActions
    {
        void OnSkill(InputAction.CallbackContext context);
    }
    public interface ILookActions
    {
        void OnLook(InputAction.CallbackContext context);
    }
}
