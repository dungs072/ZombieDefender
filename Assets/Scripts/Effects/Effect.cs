using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Effect : NetworkBehaviour
{
    [SerializeField] private float lifetime = 4;
    [SerializeField] private ReferenceItself referenceItself;
    [SerializeField] private Animator animator;

    [SerializeField] private bool isExploded;

    [SerializeField] float maxShakeIntensity = 1f;
    [SerializeField] float maxShakeRadius = 10f;

    [Header("Sounds")]
    [SerializeField] private AudioClip hitSound;
    private AudioSource audioSource;

    private NetworkObjectPool pool;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pool = NetworkObjectPool.Singleton;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (lifetime == -1)
        {
            lifetime = stateInfo.length;
        }

    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        if (isExploded)
        {
            TriggerExplosion();
        }
        Invoke("Deactivate", lifetime);
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }
        CancelInvoke();
    }
    private void OnEnable()
    {
        if (!IsServer) { return; }
        if (isExploded)
        {
            TriggerExplosion();
        }

        Invoke("Deactivate", lifetime);
    }

    private void OnDisable()
    {
        if (!IsServer) { return; }
        CancelInvoke();
    }
    private void Deactivate()
    {
        pool.ReturnNetworkObject(GetComponent<NetworkObject>(), referenceItself.Prefab);
        ToggleGameObjectClientRpc(false);
    }

    public void TriggerExplosion()
    {
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        GenerateImpulseBasedOnDistance();
    }

    private void GenerateImpulseBasedOnDistance()
    {

        float distance = Vector2.Distance(transform.position, CameraController.Instance.transform.position);
        var impulseSource = CameraController.Instance.GetComponent<CinemachineImpulseSource>();
        float intensity = Mathf.Clamp01(1 - (distance / maxShakeRadius)) * maxShakeIntensity;
        if (intensity > 0)
        {
            impulseSource.GenerateImpulseAt(transform.position, new Vector3(intensity, intensity, intensity));
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void ToggleGameObjectClientRpc(bool state)
    {
        if (IsServer) { return; }
        gameObject.SetActive(state);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SetPositionClientRpc(Vector3 position)
    {
        if (IsServer) { return; }
        transform.position = position;
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void SetRotationClientRpc(Quaternion rotation)
    {
        if (IsServer) { return; }
        transform.rotation = rotation;
    }
}
