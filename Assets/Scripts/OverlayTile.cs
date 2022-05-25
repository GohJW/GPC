using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    public int G;
    public int H;

    public int F { get { return G + H; } }

    public bool isObstacle = false;
    public bool isEnemy = false;
    public bool isAlly = false;
    public bool isBarrel = false;
    public bool isAttack = false;
    public OverlayTile previous;
    public CharacterInfo character = null;
 

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
    // Update is called once per frame

    
    public void ShowTile()
    { 
        if(isEnemy == true || isBarrel == true)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        }
        else if(isAlly == true)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
