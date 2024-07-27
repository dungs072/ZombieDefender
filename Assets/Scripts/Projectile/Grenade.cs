using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Effect explosionEffect;
    [SerializeField] private float forceAmount;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private Image timerUI;
    [SerializeField] private GameObject trail;
    [SerializeField] private bool canExplodeAfterCollision;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        if (timerUI != null)
        {
            LeanTween.value(gameObject, UpdateFillAmount, timerUI.fillAmount, 0, timeToDestroy);
        }

        Destroy(gameObject, timeToDestroy);
    }
    private void UpdateFillAmount(float value)
    {
        timerUI.fillAmount = value;
    }
    private void OnDestroy()
    {
        SpawnEffect();
    }
    protected virtual void SpawnEffect(Transform target = null)
    {
        var effectInstance = NetworkObjectPool.Singleton.
                                  GetNetworkObject(explosionEffect.gameObject,
                                      transform.position, transform.rotation);
        if (effectInstance.IsSpawned)
        {

            if (effectInstance.TryGetComponent(out Effect effect))
            {

                effect.ToggleGameObjectClientRpc(true);
                effect.SetPositionClientRpc(transform.position);
                effect.SetRotationClientRpc(transform.rotation);
            }

        }
        else
        {
            effectInstance.Spawn(true);
        }
    }

    public void Throw(Vector2 direction)
    {
        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
        StartCoroutine(StopMovement());
    }
    private IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(timeToDestroy * 0.05f);
        ResetRigidbody();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (canExplodeAfterCollision)
        {
            Destroy(gameObject);
        }
        else
        {
            ResetRigidbody();
        }

    }
    private void ResetRigidbody()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        trail.SetActive(false);
    }
}
