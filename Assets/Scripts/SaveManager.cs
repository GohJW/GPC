using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

    public Button Stage2Button;
    public Button Stage3Button;
    public Button Stage4Button;
    public Button Stage5Button;
    public Button Stage6Button;
    public Button Stage7Button;
    public Button Stage8Button;

    public AudioManager audiomanager;

    private void Start()
    {
        audiomanager = FindObjectOfType<AudioManager>();
        DataLoad(audiomanager);
        if (audiomanager.Stage2Played == false)
        {
            Stage2Button.interactable = false;
            Stage3Button.interactable = false;
            Stage4Button.interactable = false;
            Stage5Button.interactable = false;
            Stage6Button.interactable = false;
            Stage7Button.interactable = false;
            Stage8Button.interactable = false;
        }

        else if (audiomanager.Stage3Played == false)
        {
            Stage3Button.interactable = false;
            Stage4Button.interactable = false;
            Stage5Button.interactable = false;
            Stage6Button.interactable = false;
            Stage7Button.interactable = false;
            Stage8Button.interactable = false;
        }

        else if (audiomanager.Stage4Played == false)
        {
            Stage4Button.interactable = false;
            Stage5Button.interactable = false;
            Stage6Button.interactable = false;
            Stage7Button.interactable = false;
            Stage8Button.interactable = false;
        }

        else if (audiomanager.Stage5Played == false)
        {
            Stage5Button.interactable = false;
            Stage6Button.interactable = false;
            Stage7Button.interactable = false;
            Stage8Button.interactable = false;
        }

        else if (audiomanager.Stage6Played == false)
        {
            Stage6Button.interactable = false;
            Stage7Button.interactable = false;
            Stage8Button.interactable = false;
        }

        else if (audiomanager.Stage7Played == false)
        {
            Stage7Button.interactable = false;
            Stage8Button.interactable = false;
        }

        else if (audiomanager.Stage8Played == false)
        {
            Stage8Button.interactable = false;
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
    public void Stage5()
    {
        SceneManager.LoadScene("CharacterSelect 5");
    }
    public void Stage6()
    {
        SceneManager.LoadScene("CharacterSelect 6");
    }
    public void Stage7()
    {
        SceneManager.LoadScene("CharacterSelect 7");
    }
    public void Stage8()
    {
        SceneManager.LoadScene("CharacterSelect 8");
    }

    public void DataLoad(AudioManager audiomanager)
    {
        StageData data = SaveSystem.LoadStage(audiomanager);
        audiomanager.Stage2Played = data.Stage2Played;
        audiomanager.Stage3Played = data.Stage3Played;
        audiomanager.Stage4Played = data.Stage4Played;
        audiomanager.Stage5Played = data.Stage5Played;
        audiomanager.Stage6Played = data.Stage6Played;
        audiomanager.Stage7Played = data.Stage7Played;
        audiomanager.Stage8Played = data.Stage8Played;

    }
}
