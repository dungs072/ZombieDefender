using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    public void Move(Vector2 direction)
    {
        direction = direction * speed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + direction.x, 
                                        transform.position.y + direction.y);
    }
    
}
