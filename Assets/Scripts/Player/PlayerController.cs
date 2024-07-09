using Unity.Netcode;
using UnityEngine;
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Movement movement;
    [SerializeField] private Fighter fighter;
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] private WeaponManager weaponManager;

    private void Start()
    {
        ((CustomNetworkManager)NetworkManager.Singleton).AddPlayer(this);
    }
    public override void OnDestroy()
    {
        ((CustomNetworkManager)NetworkManager.Singleton).RemovePlayer(this);
        base.OnDestroy();

    }
    public override void OnNetworkSpawn()
    {

    }
    public override void OnNetworkDespawn()
    {

    }
    private void Update()
    {
        if (!IsOwner) { return; }
        HandleRotation();
        HandleMovement();
        HandleSwitchWeapon();
        Fight();
    }
    private void HandleRotation()
    {
        Vector2 mouseScreenPosition = inputHandler.MousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 direction = (worldPosition - transform.position).normalized;
        direction.z = 0;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    private void HandleMovement()
    {
        if (inputHandler.MovementInput != Vector2.zero)
        {
            movement.Move(inputHandler.MovementInput);
        }
        animator.PlayLocomtionAnimation(inputHandler.MovementInput != Vector2.zero);
    }
    private void HandleSwitchWeapon()
    {

        if (inputHandler.MouseScrollY > 0)
        {
            weaponManager.ChangeUpWeapon();
        }
        if (inputHandler.MouseScrollY < 0)
        {
            weaponManager.ChangeDownWeapon();
        }
    }
    private void Fight()
    {
        if (inputHandler.CanAttack)
        {
            fighter.Attack();
        }
    }



}

