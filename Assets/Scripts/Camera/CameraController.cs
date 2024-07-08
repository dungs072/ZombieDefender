using Unity.Netcode;
using UnityEngine;
using Cinemachine;
public class CameraController : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public override void OnNetworkSpawn()
    {
        Invoke(nameof(InitialCameraSetUp), 1f);
    }
    private void InitialCameraSetUp()
    {
        var player = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }

}
