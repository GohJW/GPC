using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAttackrangeAfterMovingNode : Node
{
    private EnemyAi ai;
    private List<OverlayTile> inAttackRange;
    private OverlayTile CurrentTile;

    public InAttackrangeAfterMovingNode(EnemyAi ai, List<OverlayTile> inAttackRange, OverlayTile currentTile)
    {
        this.ai = ai;
        this.inAttackRange = inAttackRange;
        CurrentTile = currentTile;
    }

    public override NodeState Evaluate()
    {
        foreach(OverlayTile item in inAttackRange)
        {
            if (item.isAlly)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}



