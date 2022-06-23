using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnUIScript : MonoBehaviour
{
    public TMP_Text EnemyTurn;
    public TMP_Text PlayerTurn;
    public TMP_Text Win;
    public TMP_Text Lose;

    public void ShowEnemyTurn()
    {
        StartCoroutine(Delay((float)0.5));
        PlayerTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        Win.GetComponent<TextMeshProUGUI>().enabled = false;
        Lose.GetComponent<TextMeshProUGUI>().enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        EnemyTurn.GetComponent<TextMeshProUGUI>().enabled = true;   
    }

    public void ShowPlayerTurn()
    {
        StartCoroutine(Delay((float)0.5));
        EnemyTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        Win.GetComponent<TextMeshProUGUI>().enabled = false;
        Lose.GetComponent<TextMeshProUGUI>().enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        PlayerTurn.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void ShowWin()
    {
        StartCoroutine(Delay((float)1));
        EnemyTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        PlayerTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        Lose.GetComponent<TextMeshProUGUI>().enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        Win.GetComponent<TextMeshProUGUI>().enabled = true;
    }
    public void ShowLose()
    {
        StartCoroutine(Delay((float)1));
        EnemyTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        PlayerTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        Win.GetComponent<TextMeshProUGUI>().enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        Lose.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void Hide()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }
    public IEnumerator Delay(float x)
    {
        yield return new WaitForSeconds(x);
        Hide();
    }
}

