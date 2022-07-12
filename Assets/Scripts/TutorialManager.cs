using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public TextMeshProUGUI instructionText;
    public Image instructionPicture;

    public Tutorial tutorial;

    public Button playButton;
    public Button continueButton;

    private Queue<string> instructions;
    private Queue<Image> pictures;

    void Start()
    {
        instructions = new Queue<string>();
        pictures = new Queue<Image>();
        playButton.gameObject.SetActive(false);
        StartTutorial(tutorial);
    }

    public void StartTutorial(Tutorial tutorial)
    {
        instructions.Clear();
        pictures.Clear();
        
        foreach (string instruction in tutorial.instructions)
        {
            instructions.Enqueue(instruction);
        }

        foreach (Image picture in tutorial.pictures)
        {
            pictures.Enqueue(picture);
        }

        DisplayNextInstruction();
        DisplayNextPicture();
    }

    public void DisplayNextInstruction()
    {
        if (instructions.Count == 0)
        {
            EndTutorial();
            return;
        }

        string instruction = instructions.Dequeue();
        instructionText.text = instruction;

    }

    public void DisplayNextPicture()
    {
        if (pictures.Count == 0)
        {
            EndTutorial();
            return;
        }

        Image picture = pictures.Dequeue();
        instructionPicture = picture;

    }

    public void EndTutorial()
    {
        continueButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
