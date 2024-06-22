
using UnityEngine;
using UnityEngine.Pool;
public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 20;

    private ObjectPool<GameObject> poolProjectile { get; set; }
    public static ProjectileManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        poolProjectile = new ObjectPool<GameObject>(
           createFunc: () => Instantiate(projectilePrefab, transform),
           actionOnGet: obj => obj.SetActive(true),
           actionOnRelease: obj => obj.SetActive(false),
           actionOnDestroy: Destroy,
           defaultCapacity: defaultCapacity,
           maxSize: maxSize
       );
    }
    public GameObject GetObject()
    {
        return poolProjectile.Get();
    }

    public void ReturnObject(GameObject obj)
    {
        poolProjectile.Release(obj);
    }

}
