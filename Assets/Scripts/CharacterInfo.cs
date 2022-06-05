using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public int movementrange;
    public int attackrange;
    public float CharacterHP;
    public int Attack;
    public float Defense;
    public bool hasMoved = false;
    public bool hasAttack = false;
    public string characterName;
    public bool moving = false;
    public bool damaged = false;
    public bool attacking = false;


    //public Rigidbody2D rb;
    //Vector2 movement;
    public Animator animator;

    private void Update()
    {          
        animator.SetBool("Moving", moving);
        animator.SetBool("Damaged", damaged);
        animator.SetBool("Attacking", attacking);

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
