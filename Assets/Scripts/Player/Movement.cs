using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    [SerializeField] private float runningSpeed = 100f;
    [SerializeField] private AudioClip walkingSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Move(Vector2 direction, bool canRun)
    {
        float currentSpeed = canRun ? runningSpeed : speed;
        direction = direction * currentSpeed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + direction.x,
                                        transform.position.y + direction.y);
        if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(walkingSound);
    }
    public void AddRunningSpeed(float speed)
    {
        runningSpeed += speed;
    }

}
