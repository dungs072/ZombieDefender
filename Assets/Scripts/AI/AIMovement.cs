using Pathfinding;
using Unity.Netcode;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AIPath aiPath;

    public AIPath AIPath { get { return aiPath; } }
    public void MoveToTarget(Vector3 targetPosition)
    {
        aiPath.isStopped = false;
        aiPath.destination = targetPosition;
    }
    public void ToggleStop(bool state)
    {
        aiPath.isStopped = state;
    }
    public void SetCanMove(bool state)
    {
        aiPath.canMove = state;
    }
    public void RotateToTarget(Vector3 targetPosition)
    {
        Quaternion neededRotation = Quaternion.LookRotation(Vector3.forward, targetPosition - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * rotationSpeed);
    }

}
