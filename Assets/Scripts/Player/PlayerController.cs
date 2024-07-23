using System;
using Unity.Netcode;
using UnityEngine;
public class PlayerController : NetworkBehaviour
{
    //public static event Action<PlayerController> PlayerSpawned;
    public static event Action<PlayerController> PlayerDespawned;
    [SerializeField] private int runEnergyAmount = 1;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Movement movement;
    [SerializeField] private Fighter fighter;
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PickupHandler pickupHandler;
    private Energy energy;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        energy = GetComponent<Energy>();
        inputHandler.WeaponReloaded += HandleReloadWeapon;
        HandleInputRegister();
        ((CustomNetworkManager)NetworkManager.Singleton).AddPlayer(this);
        Invoke(nameof(WaitToSetUp), Const.ReloadingTimeAdded);
    }
    private void HandleInputRegister()
    {
        inputHandler.RightDashed += HandleDashLeft;
        inputHandler.LeftDashed += HandleDashRight;
    }
    private void WaitToSetUp()
    {
        //PlayerSpawned?.Invoke(this);
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
        HandlePickup();
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
            bool canRun = inputHandler.CanRun;
            if (canRun)
            {
                canRun = energy.UseEnergy(runEnergyAmount);
            }
            movement.Move(inputHandler.MovementInput, canRun);
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
    private void HandlePickup()
    {
        pickupHandler.HandleHoldingPickup(inputHandler.IsPickupHolding);
    }
    private void HandleDashLeft()
    {
        //rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
    }
    private void HandleDashRight()
    {
        //rb.AddForce(transform.right * -1 * dashForce, ForceMode2D.Impulse);
    }
    private void Fight()
    {
        if (inputHandler.CanAttack)
        {
            fighter.Attack();
        }
    }

    public void HandleUpgradeSkill(UpgradeSkillType skillType)
    {
        if (skillType == UpgradeSkillType.Health)
        {
            GetComponent<Health>().AddMaxHealth(25);
        }
        else if (skillType == UpgradeSkillType.Energy)
        {
            energy.AddMaxEnergy(50);
        }
        else if (skillType == UpgradeSkillType.RunSpeed)
        {
            movement.AddRunningSpeed(1);
        }
        else if (skillType == UpgradeSkillType.ReloadingSpeed)
        {
            weaponManager.AddReduceReloadingTime(0.1f);
        }
    }



}

