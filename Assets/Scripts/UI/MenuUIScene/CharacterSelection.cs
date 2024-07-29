using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private RectTransform characterSelectionPanel;
    [SerializeField] private GameObject blackout;

    [SerializeField] private List<CharacterUI> characterUIs;
    [SerializeField] private TMP_InputField nameInput;

    [SerializeField] private TMP_Text noteText;

    [Header("Avartar")]
    [SerializeField] private Image avatar;
    [SerializeField] private TMP_Text nameText;


    private void Start()
    {
        LoadAvatar();
    }

    private void LoadAvatar()
    {
        if (PlayerPrefs.HasKey("Id"))
        {
            int id = PlayerPrefs.GetInt("Id");
            foreach (CharacterUI characterUI in characterUIs)
            {
                if (characterUI.Id == id)
                {
                    avatar.sprite = characterUI.Sprite;
                    nameText.text = PlayerPrefs.GetString("Name");
                    break;
                }
            }
        }
    }

    public void SelectCharacter(CharacterUI character)
    {
        foreach (CharacterUI characterUI in characterUIs)
        {
            characterUI.ToggleSelectedRing(character == characterUI);
        }
        noteText.text = character.Note;
        PlayerPrefs.SetInt("Id", character.Id);
        PlayerPrefs.SetString("Name", nameInput.text);

    }

    public void ToggleCharacterSelectionPanel(bool state)
    {
        blackout.SetActive(state);
        if (state)
        {
            characterSelectionPanel.gameObject.SetActive(state);
            if (characterSelectionPanel.localScale != Vector3.zero)
            {
                characterSelectionPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(characterSelectionPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
            if (PlayerPrefs.HasKey("Id"))
            {
                int id = PlayerPrefs.GetInt("Id");
                LoadData(id);
            }

        }
        else
        {
            LeanTween.scale(characterSelectionPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                LoadAvatar();
                characterSelectionPanel.gameObject.SetActive(state);
            });

        }
    }
    public void LoadData(int id)
    {
        foreach (CharacterUI characterUI in characterUIs)
        {
            if (characterUI.Id == id)
            {
                SelectCharacter(characterUI);
                break;
            }
        }
        nameInput.text = PlayerPrefs.GetString("Name");
    }
}
