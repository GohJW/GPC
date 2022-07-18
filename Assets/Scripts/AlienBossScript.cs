using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBossScript : MonoBehaviour
{
    public GameObject Left;
    public GameObject Right;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<EnemyAi>().enabled = false;
        Left = GameObject.Find("AlienBossLeft(Clone)");
        Right = GameObject.Find("AlienBossRight(Clone)");

    }
    public void CheckWeapons()
    {
        Debug.Log("check");
        if(!Left.activeSelf && !Right.activeSelf)
        {
            this.GetComponent<CharacterInfo>().activeTile.isObstacle = false;
            this.GetComponent<CharacterInfo>().activeTile.isEnemy = true;
            this.GetComponent<EnemyAi>().enabled = true;
        }
    }
}
