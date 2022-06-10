using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : Node
{
    private EnemyAi ai;
    private OverlayTile CurrentTile;

    public EndNode(EnemyAi ai, OverlayTile currentTile)
    {
        this.ai = ai;
        CurrentTile = currentTile;
    }

    public override NodeState Evaluate()
    {
        CurrentTile.character.hasAttack = true;
        return NodeState.SUCCESS;
    }
}
