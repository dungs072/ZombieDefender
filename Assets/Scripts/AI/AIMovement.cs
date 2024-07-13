using Pathfinding;
using Unity.Netcode;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AIPath aiPath;
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
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.z = 0;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

}
