using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
 
    public Button Stage2Button;
    public Button Stage3Button;
    public Button Stage4Button;

    public AudioManager audiomanager;

    private void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        if (audiomanager.Stage2Played == false)
        {
            Stage2Button.interactable = false;
            Stage3Button.interactable = false;
            Stage4Button.interactable = false;
        }

        else if (audiomanager.Stage3Played == false)
        {
            Stage3Button.interactable = false;
            Stage4Button.interactable = false;
        }

        else if (audiomanager.Stage2Played == false)
        {
            Stage4Button.interactable = false;
        }
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Stage1()
    {
        SceneManager.LoadScene("CharacterSelect 1");
    }
    public void Stage2()
    {
        SceneManager.LoadScene("CharacterSelect 2");
    }
    public void Stage3()
    {
        SceneManager.LoadScene("CharacterSelect 3");
    }
    public void Stage4()
    {
        SceneManager.LoadScene("CharacterSelect 4");
    }

}
