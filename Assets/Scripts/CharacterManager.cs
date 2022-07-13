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
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI ATKText;
    public TextMeshProUGUI DEFText;
    public TextMeshProUGUI RANGEText;
    public TextMeshProUGUI SKILL1Text;
    public TextMeshProUGUI SKILL2Text;
    public SpriteRenderer artworkSprite;
    public TextMeshProUGUI selectedText;

    public Button selectButton;
    public Button playButton;

    private int selectedOption = 0;
    private int selectedIndex = 0;
    public static int[] selectedOptionIndex = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        selectedIndex = 0;
        playButton.gameObject.SetActive(false);
        ResetCharacters();

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
       nameText.color = Color.white;
       if (character.hasSelected)
        {
            nameText.text = "Selected";
            nameText.color = Color.green;
        }
        HPText.text = character.MaxHP.ToString();
        ATKText.text = character.Attack.ToString();
        DEFText.text = character.Defense.ToString();
        RANGEText.text = character.movementrange.ToString();
        SKILL1Text.text = character.Skill1;
        SKILL2Text.text = character.Skill2;

    }
    private void ResetCharacters()
    {
        foreach(CharacterInfo character in characterDB.character)
        {
           character.hasSelected = false;
        }
        for (int i = 0; i < 3; i++)
        {
            selectedOptionIndex[i] = -1;
        }
    }

    public void DeselectCharacter()
    {
        CharacterInfo character = characterDB.GetCharacter(selectedOption);
        if (character.hasSelected)
        {
            character.hasSelected = false;
            selectedIndex--;
            selectedText.text = selectedIndex.ToString();
            selectedOptionIndex[selectedIndex] = -1;
            UpdateCharacter(selectedOption);
        }
        if (selectedIndex != 3)
        {
            selectButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

    public void ChangeScene(string sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void SelectCharacter()
    {
        CharacterInfo character = characterDB.GetCharacter(selectedOption);
        if (selectedIndex < 3 && !character.hasSelected)
        {
            character.hasSelected = true;
            selectedOptionIndex[selectedIndex] = selectedOption;
            selectedIndex++;
            selectedText.text = selectedIndex.ToString();
            UpdateCharacter(selectedOption);
        }
        if (selectedIndex == 3) 
        {
            selectButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
            UpdateCharacter(selectedOption);

        }
        else
        {
            return;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
