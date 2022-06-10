using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Node
{
    private EnemyAi ai;
    private OverlayTile CurrentTile;
    private OverlayTile AllyTile;

    public AttackNode(EnemyAi ai, OverlayTile currentTile, OverlayTile allyTile)
    {
        this.ai = ai;
        CurrentTile = currentTile;
        AllyTile = allyTile;
    }

    public override NodeState Evaluate()
    {
        Attack();
        Debug.Log("attacked");
        return NodeState.SUCCESS;
    }

    private void Attack()
    {
        CharacterInfo Attacker = CurrentTile.character;
        CharacterInfo Attacked = ai.AllyTile.character;

        Attacked.animator.SetTrigger("Damaged");
        Attacker.animator.SetTrigger("Attacking");

        Attacked.CharacterHP -= Attacker.Attack * (1 - Attacked.Defense / 100);
        Attacker.hasAttack = true;
        if (Attacked.CharacterHP <= 0)
        {
            Attacked.GetComponent<SpriteRenderer>().enabled = false;
            AllyTile.isAlly = false;
        }
    }
}
