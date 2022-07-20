using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Slider slider;
    public AudioManager AudioManager;
    public Button Stagebutton;
    public TMP_Text Stagebuttontext;

    public void Start()
    {
        AudioManager = FindObjectOfType<AudioManager>();
        if(AudioManager.Stage2Played == true)
        {
            Stagebutton.interactable = true;
            Stagebuttontext.color = new Color(1, 1, 1, 1);
        }
    }

    public void PlayGame()
    {
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
