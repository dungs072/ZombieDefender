using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.Netcode;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private AIPath aiPath;
    private int targetIndex;

    private Transform target;
    private void Start()
    {
        Seeker seeker = GetComponent<Seeker>();
        //Invoke(nameof(FindPlayer), 2f);
    }
    private void FindPlayer()
    {
        target = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer.transform;
    }

    private void Update()
    {
        if (target == null)
        {
            var player = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
            if (player == null)
            {
                return;
            }
            target = player.transform;
            return;
        }
        aiPath.destination = target.position;
        aiPath.SearchPath();


    }

}
