using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using System;
using System.Collections;
public class CameraController : NetworkBehaviour
{
    public static CameraController Instance;
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public override void OnNetworkSpawn()
    {
        StartCoroutine(InitialCameraSetUp());
    }
    private IEnumerator InitialCameraSetUp()
    {
        var player = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        while (player == null) yield return null;
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }

}
