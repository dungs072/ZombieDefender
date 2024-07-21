using System;
using Unity.Netcode;
using UnityEngine;
public class PlayerController : NetworkBehaviour
{
    public static event Action<PlayerController> PlayerSpawned;
    public static event Action<PlayerController> PlayerDespawned;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Movement movement;
    [SerializeField] private Fighter fighter;
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] private WeaponManager weaponManager;
    public override void OnNetworkSpawn()
    {

        if (!IsOwner) return;

        inputHandler.WeaponReloaded += HandleReloadWeapon;
        Invoke(nameof(WaitToSetUp), 3f);
    }
    private void WaitToSetUp()
    {
        ((CustomNetworkManager)NetworkManager.Singleton).AddPlayer(this);
        PlayerSpawned?.Invoke(this);
        GetComponent<Health>().Reset();
    }
    public override void OnNetworkDespawn()
    {
        ((CustomNetworkManager)NetworkManager.Singleton).RemovePlayer(this);
        if (!IsOwner) return;

        PlayerDespawned?.Invoke(this);
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
        Quaternion neededRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * rotationSpeed);
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
    private void HandleReloadWeapon()
    {
        weaponManager.ReloadWeapon();
    }
    private void Fight()
    {
        if (inputHandler.CanAttack)
        {
            fighter.Attack();
        }
    }



}

