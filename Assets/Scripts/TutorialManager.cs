using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public TextMeshProUGUI instructionText;
    public GameObject instructionPicture;

    public Tutorial tutorial;

    public Button playButton;
    public Button continueButton;

    private Queue<string> instructions;
    private Queue<Sprite> pictures;

    void Start()
    {
        instructions = new Queue<string>();
        pictures = new Queue<Sprite>();
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

        foreach (Sprite picture in tutorial.pictures)
        {
            pictures.Enqueue(picture);
        }

        DisplayNextInstruction();
        DisplayNextPicture();
    }

    public void DisplayNextInstruction()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
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

        Sprite picture = pictures.Dequeue();

        instructionPicture.GetComponent<SpriteRenderer>().sprite = picture;

    }

    public void EndTutorial()
    {
        continueButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
