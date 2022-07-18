using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAllNode : Node
{
    private EnemyAi ai;
    private OverlayTile CurrentTile;
    private OverlayTile AllyTile;
    private Animator animator;

    public AttackAllNode(EnemyAi ai, OverlayTile currentTile, OverlayTile allyTile, Animator animator)
    {
        this.ai = ai;
        CurrentTile = currentTile;
        AllyTile = allyTile;
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        if(AllyTile == null)
        {
            return NodeState.FAILURE;
        }
        Attack();
        return NodeState.SUCCESS;
    }

    private void Attack()
    {
        CharacterInfo Attacker = CurrentTile.character;
        CharacterInfo Attacked = ai.AllyTile.character;

        GameObject.FindObjectOfType<AudioManager>().Play(CurrentTile.character.characterName + CurrentTile.character.Attackrange);
        Attacked.animator.SetTrigger("Damaged");
        Attacker.animator.SetTrigger("Attacking");

        Attacked.CharacterHP -= Attacker.Attack * (1 - Attacked.Defense / 100);
        Attacker.hasAttack = true;
        if (Attacker.burn)
        {
            Attacked.Burntimer += 2;
            Attacked.burnicon.SetActive(true);
        }
        if (Attacked.CharacterHP <= 0)
        {
            Attacked.GetComponent<SpriteRenderer>().enabled = false;
            AllyTile.isAlly = false;
            Attacked.gameObject.SetActive(false);
                

        }
    }
}
