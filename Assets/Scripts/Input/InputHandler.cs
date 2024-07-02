using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : NetworkBehaviour
{
    private Actions actions;
    public Vector2 MovementInput { get; private set; }
    public Vector2 MousePosition { get { return Mouse.current.position.ReadValue(); } }
    public bool CanAttack { get; private set; }
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

}
