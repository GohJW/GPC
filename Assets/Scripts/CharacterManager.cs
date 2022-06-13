using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI selectText;
    public SpriteRenderer artworkSprite;

    public Button selectButton;

    private int selectedOption = 0;
    private int selectedIndex = 0;
    public int[] selectedOptionIndex = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }

        else
        {
            Load();
        }

         UpdateCharacter(selectedOption);
    }

    public void NextOption()
    {
        selectedOption++;

        if(selectedOption == characterDB.CharacterCount)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption);
        Save();
    }

    public void PreviousOption()
    {
        selectedOption--;
        
        if(selectedOption < 0)
        {
            selectedOption = characterDB.CharacterCount - 1;
        }

        UpdateCharacter(selectedOption);
        Save();
    }

    private void UpdateCharacter(int selectedOption)
    {
       CharacterInfo character = characterDB.GetCharacter(selectedOption);
       artworkSprite.sprite = character.characterSprite;
       nameText.text = character.characterName;

    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void SelectCharacter()
    {
        if (selectedIndex < 3)
        {
            selectedOptionIndex[selectedIndex] = selectedOption;
            selectedIndex++;
        }
        if (selectedIndex == 3) 
        {
            selectButton.gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }

}
