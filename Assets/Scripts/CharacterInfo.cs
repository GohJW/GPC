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
    public int Defense;
    public bool hasMoved = false;
    public bool hasAttack = false;
    public string characterName;
    public bool moving = false;


    //public Rigidbody2D rb;
    //Vector2 movement;
    public Animator animator;

    private void Update()
    {
        animator.SetBool("Moving", moving);
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
