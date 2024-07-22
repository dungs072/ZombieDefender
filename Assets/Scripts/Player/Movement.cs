using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    [SerializeField] private float runningSpeed = 100f;
    public void Move(Vector2 direction, bool canRun)
    {
        float currentSpeed = canRun ? runningSpeed : speed;
        direction = direction * currentSpeed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + direction.x,
                                        transform.position.y + direction.y);
    }

}
