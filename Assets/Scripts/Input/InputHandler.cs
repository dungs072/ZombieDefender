using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : NetworkBehaviour
{
    public event Action WeaponReloaded;
    public event Action ItemThrown;
    public event Action LeftDashed;
    public event Action RightDashed;
    private Actions actions;
    public Vector2 MovementInput { get; private set; }
    public Vector2 MousePosition { get { return Mouse.current.position.ReadValue(); } }
    public float MouseScrollY { get; private set; }
    public bool CanAttack { get; private set; }
    public bool CanRun { get; private set; }
    public bool IsPickupHolding { get; private set; }
    public bool IsAiming { get; private set; }


    private void Awake()
    {
        actions = new Actions();

    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        actions.Enable();

        actions.Player.Attack.performed += OnAttack;
        actions.Player.Attack.canceled += OnAttack;
        actions.Player.Run.performed += OnRun;
        actions.Player.Run.canceled += OnRun;
        actions.Player.Pickup.performed += OnPickup;
        actions.Player.Pickup.canceled += OnPickup;
        actions.Player.Reload.performed += OnReload;
        actions.Player.Throw.performed += OnThrowItem;
        actions.Player.Aim.performed += OnAiming;
        actions.Player.Aim.canceled += OnAiming;
        actions.Player.DashLeft.performed += OnDashLeft;
        actions.Player.DashRight.performed += OnDashRight;

        actions.Player.MouseScrollY.performed += (x) => MouseScrollY = x.ReadValue<float>();
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
        actions.Disable();
    }

    private void Update()
    {
        if (!IsOwner) { return; }
        MovementInput = actions.Player.Movement.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        CanAttack = context.performed;
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        CanRun = context.performed;
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            WeaponReloaded?.Invoke();
        }
    }
    public void OnPickup(InputAction.CallbackContext context)
    {
        IsPickupHolding = context.performed;
    }
    public void OnThrowItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ItemThrown?.Invoke();
        }
    }
    public void OnAiming(InputAction.CallbackContext context)
    {
        IsAiming = context.performed;
    }
    public void OnDashLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftDashed?.Invoke();
        }
    }
    public void OnDashRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RightDashed?.Invoke();
        }
    }


}
