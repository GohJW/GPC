using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public int movementrange;
    public int attackrange;
    public int CharacterHP;
    public int Attack;
    public bool hasMoved = false;
    public bool hasAttack = false;
    public string characterName;
}
