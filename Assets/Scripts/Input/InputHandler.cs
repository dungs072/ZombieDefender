using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    private Actions actions;
    public Vector2 MovementInput { get; private set; }
    public Vector2 MousePosition { get { return Mouse.current.position.ReadValue(); } }
    public bool CanAttack { get; private set; }
    private void Awake()
    {
        actions = new Actions();

    }
    private void OnEnable()
    {
        actions.Enable();

        actions.Player.Attack.performed += OnAttack;
        actions.Player.Attack.canceled += OnAttack;
    }
    private void OnDisable()
    {
        actions.Disable();
    }
    private void Update()
    {
        MovementInput = actions.Player.Movement.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        CanAttack = context.performed;
    }

}
