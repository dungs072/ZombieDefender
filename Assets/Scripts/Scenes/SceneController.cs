using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : NetworkBehaviour
{
    [SerializeField] private LoadingUI loadingUI;

    public LoadingUI LoadingUI { set { loadingUI = value; } }
    public bool IsLoadCurrentSceneFinished { get; private set; } = false;
    public static SceneController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
    }
    public override void OnNetworkDespawn()
    {
        //NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;
    }
    public IEnumerator StartMyServer(bool isHost, string sceneName)
    {
        // var success = false;
        if (isHost)
        {
            while (NetworkManager.Singleton.IsListening)
            {
                yield return null;
            }

            NetworkManager.Singleton.StartHost();
        }
        // else
        // {
        //     success = NetworkManager.Singleton.StartServer();
        // }
        IsLoadCurrentSceneFinished = false;
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        // if (success)
        // {


        // }


        //return success;
    }
    public void LoadScene(string sceneName)
    {
        NetworkManager.Singleton.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void StartMyClient(string sceneName)
    {
        var success = NetworkManager.Singleton.StartClient();
        if (success)
        {
            IsLoadCurrentSceneFinished = true;
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        //return success;
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        // Both client and server receive these notifications
        switch (sceneEvent.SceneEventType)
        {
            // Handle server to client Load Notifications
            case SceneEventType.Load:
                {
                    // This event provides you with the associated AsyncOperation
                    // AsyncOperation.progress can be used to determine scene loading progression
                    var asyncOperation = sceneEvent.AsyncOperation;
                    StartCoroutine(UpdateLoadingProgress(asyncOperation));
                    // Since the server "initiates" the event we can simply just check if we are the server here
                    if (IsServer)
                    {

                    }
                    else
                    {
                        // Handle client side load event related tasks here
                    }
                    break;
                }
            // Handle server to client unload notifications
            case SceneEventType.Unload:
                {
                    // You can use the same pattern above under SceneEventType.Load here
                    break;
                }
            // Handle client to server LoadComplete notifications
            case SceneEventType.LoadComplete:
                {
                    // This will let you know when a load is completed
                    // Server Side: receives thisn'tification for both itself and all clients
                    if (IsServer)
                    {
                        if (sceneEvent.ClientId == NetworkManager.LocalClientId)
                        {

                            //loadingUI.gameObject.SetActive(false);
                            // Handle server side LoadComplete related tasks here
                        }
                        else
                        {
                            // Handle client LoadComplete **server-side** notifications here
                        }
                    }
                    else // Clients generate thisn'tification locally
                    {
                        // Handle client side LoadComplete related tasks here
                    }

                    // So you can use sceneEvent.ClientId to also track when clients are finished loading a scene
                    break;
                }
            // Handle Client to Server Unload Complete Notification(s)
            case SceneEventType.UnloadComplete:
                {
                    // This will let you know when an unload is completed
                    // You can follow the same pattern above as SceneEventType.LoadComplete here

                    // Server Side: receives thisn'tification for both itself and all clients
                    // Client Side: receives thisn'tification for itself

                    // So you can use sceneEvent.ClientId to also track when clients are finished unloading a scene
                    break;
                }
            // Handle Server to Client Load Complete (all clients finished loading notification)
            case SceneEventType.LoadEventCompleted:
                {
                    // This will let you know when all clients have finished loading a scene
                    // Received on both server and clients
                    foreach (var clientId in sceneEvent.ClientsThatCompleted)
                    {
                        // Example of parsing through the clients that completed list
                        if (IsServer)
                        {
                            // Handle any server-side tasks here
                        }
                        else
                        {
                            // Handle any client-side tasks here
                        }
                    }
                    break;
                }
            // Handle Server to Client unload Complete (all clients finished unloading notification)
            case SceneEventType.UnloadEventCompleted:
                {
                    // This will let you know when all clients have finished unloading a scene
                    // Received on both server and clients
                    foreach (var clientId in sceneEvent.ClientsThatCompleted)
                    {
                        // Example of parsing through the clients that completed list
                        if (IsServer)
                        {
                            loadingUI.gameObject.SetActive(false);
                            // Handle any server-side tasks here
                        }
                        else
                        {
                            // Handle any client-side tasks here
                        }
                    }
                    break;
                }
        }
    }
    private IEnumerator UpdateLoadingProgress(AsyncOperation asyncOperation)
    {
        if (loadingUI != null)
        {
            loadingUI.gameObject.SetActive(true);
        }

        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);


            loadingUI.SetLoadingProgress(progress);
            if (asyncOperation.progress >= 0.9f)
            {
                loadingUI.SetLoadingText("Press space key to play");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    IsLoadCurrentSceneFinished = true;
                    asyncOperation.allowSceneActivation = true;

                }

            }

            yield return null;
        }
    }
}
