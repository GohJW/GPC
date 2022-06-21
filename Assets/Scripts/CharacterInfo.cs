using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public int movementrange;
    public float CharacterHP;
    public float MaxHP;
    public int Attackrange;
    public int Attack;
    public bool burn;
    public float Defense;
    public bool hasSelected = false;
    public bool hasMoved = false;
    public bool hasAttack = false;
    public string characterName;
    public bool moving = false;
    public int Skill2cooldown = 0;
    public int Burntimer = 0;

    public int Skillnumber = 1;
    public int Skill1attackrange;
    public int Skill1attack;
    public string Skill1;
    public bool Skill1burn;

    public int Skill2attackrange;
    public int Skill2attack;
    public string Skill2;
    public bool Skill2burn;


    public Sprite characterSprite;
    //public Rigidbody2D rb;
    //Vector2 movement;
    public Animator animator;

    public void UpdateSkillinfo()
    {
        if (Skillnumber == 1)
        {
            Attackrange = Skill1attackrange;
            Attack = Skill1attack;
            burn = Skill1burn;

        }
        if(Skillnumber == 2)
        {
            Attackrange = Skill2attackrange;
            Attack = Skill2attack;
            burn = Skill2burn;
        }
        if(Skill2cooldown < 0)
        {
            Skill2cooldown = 0;
        }
    }


}
