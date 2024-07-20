using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;

    public void HandleSinglePlayClick()
    {

        mainMenuUI.SetActive(false);
    }
}
