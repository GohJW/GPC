using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public int movementrange;   
    public float CharacterHP;
    public int Attackrange;
    public int Attack;
    public float Defense;
    public bool hasMoved = false;
    public bool hasAttack = false;
    public string characterName;
    public bool moving = false;
    //public bool damaged = false;
    //public bool attacking = false;

    public int Skillnumber = 1;
    public int Skill1attackrange;
    public int Skill1attack;
    public string Skill1;

    public int Skill2attackrange;
    public int Skill2attack;
    public string Skill2;


    public Sprite characterSprite;
    //public Rigidbody2D rb;
    //Vector2 movement;
    public Animator animator;

    public void UpdateSkillinfo()
    {          
        //animator.SetBool("Moving", moving);
        //animator.SetBool("Damaged", damaged);
        //animator.SetBool("Attacking", attacking);

        if (Skillnumber == 1)
        {
            Attackrange = Skill1attackrange;
            Attack = Skill1attack;
        }
        if(Skillnumber == 2)
        {
            Attackrange = Skill2attackrange;
            Attack = Skill2attack;
        }



        //   movement.x = Input.mousePosition.x;
        //    movement.y = Input.mousePosition.y;

        //    animator.SetFloat("Horizontal", movement.x);
        //    animator.SetFloat("Vertical", movement.y);
        //    animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    //private void FixedUpdate()
    //{
    //    rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    //}
}
