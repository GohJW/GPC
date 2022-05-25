using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnUIScript : MonoBehaviour
{
    public TMP_Text EnemyTurn;
    public TMP_Text PlayerTurn;

    public void ShowEnemyTurn()
    {
        StartCoroutine(Delay((float)0.5));
        PlayerTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        EnemyTurn.GetComponent<TextMeshProUGUI>().enabled = true;   
    }

    public void ShowPlayerTurn()
    {
        StartCoroutine(Delay((float)0.5));
        EnemyTurn.GetComponent<TextMeshProUGUI>().enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
        PlayerTurn.GetComponent<TextMeshProUGUI>().enabled = true;
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

