using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using System;
using System.Threading.Tasks;

public class RelayManager : MonoBehaviour
{
    // async void Start()
    // {
    //     //await InitializeUnityServices();
    //     // Your code to use Relay Services here
    // }

    // private async Task InitializeUnityServices()
    // {
    //     try
    //     {
    //         await UnityServices.InitializeAsync();
    //         Debug.Log("Unity Services initialized successfully.");

    //         // Optional: Sign in anonymously to use Relay Services.
    //         if (!AuthenticationService.Instance.IsSignedIn)
    //         {
    //             await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //             Debug.Log("Signed in anonymously.");
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Failed to initialize Unity Services: {e}");
    //     }
    // }
}
