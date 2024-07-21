
using UnityEngine;
using UnityEngine.UI;

public class ReloadingUI : MonoBehaviour
{
    [SerializeField] private Image loadingFill;

    private void Start()
    {
        ShootWeapon.ReloadingTimeChanged += SetReloadingFill;
    }
    private void OnDestroy()
    {
        ShootWeapon.ReloadingTimeChanged -= SetReloadingFill;
    }
    public void SetReloadingFill(float factor)
    {
        loadingFill.gameObject.SetActive(factor > 0.1);
        loadingFill.fillAmount = factor;
    }
}
