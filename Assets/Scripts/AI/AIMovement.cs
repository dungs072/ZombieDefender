using Pathfinding;
using Unity.Netcode;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private AIPath aiPath;
    public void MoveToTarget(Vector3 targetPosition)
    {
        aiPath.destination = targetPosition;
        aiPath.SearchPath();
    }

}
