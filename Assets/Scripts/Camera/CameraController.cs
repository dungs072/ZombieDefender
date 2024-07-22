using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using System;
public class CameraController : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public override void OnNetworkSpawn()
    {
        Invoke(nameof(InitialCameraSetUp), Const.ReloadingTimeAdded);
    }
    private void InitialCameraSetUp()
    {
        var player = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        Debug.Log(player);
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }

}
