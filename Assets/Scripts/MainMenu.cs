using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public Slider slider;
    public AudioManager AudioManager;
    public Button Stagebutton;
    public TMP_Text Stagebuttontext;

    public void Start()
    {
        AudioManager = FindObjectOfType<AudioManager>();
        string path = Application.persistentDataPath + "/save.fun";
        //FindObjectOfType<SaveManager>().DataLoad(AudioManager); // Doesn't work
        if (File.Exists(path))
        {
            Stagebutton.interactable = true;
            Stagebutton.image.color = new Color(0, 0, 0, 1);
            Stagebuttontext.color = new Color(1, 1, 1, 1);
        }
    }

    public void PlayGame()
    {
        SaveSystem.NewGame();
        AudioManager.ResetLevels();
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        Application.Quit();
    }

    public void Stages()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        SceneManager.LoadScene("StageSelection");
    }
    public void SetVolume()
    {
        AudioManager.AudioMixer.SetFloat("Master", Mathf.Log10(slider.value) * 20);
    }
}
