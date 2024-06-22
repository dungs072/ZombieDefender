using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;

    private void OnEnable()
    {
        Invoke("Deactivate", lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void Deactivate()
    {
        ProjectileManager.Instance.ReturnObject(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ProjectileManager.Instance.ReturnObject(this.gameObject);
    }
}
