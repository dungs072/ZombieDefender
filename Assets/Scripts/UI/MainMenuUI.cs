using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private LoadingUI loadingUI;
    private void Start()
    {
        SceneController.Instance.LoadingUI = loadingUI;
    }
    public void HandleSinglePlayClick()
    {

        mainMenuUI.SetActive(false);
    }
}
