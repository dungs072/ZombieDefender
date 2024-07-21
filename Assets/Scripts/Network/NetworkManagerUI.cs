using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    // private CustomNetworkManager m_NetworkManager;

    // private void Awake()
    // {
    //     m_NetworkManager = GetComponent<CustomNetworkManager>();
    // }

    public void LeaveMatch()
    {
        if (NetworkManager.Singleton != null)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.Shutdown();
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
