using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentTileNode : Node
{
    private EnemyAi ai;
    private OverlayTile CurrentTile;
    private CharacterInfo character;

    public SetCurrentTileNode(EnemyAi ai, OverlayTile currentTile, CharacterInfo character)
    {
        this.ai = ai;
        this.CurrentTile = currentTile;
        this.character = character;
    }

    public override NodeState Evaluate()
    {
        ai.CurrentTile = character.activeTile;
        if(CurrentTile == null)
        {
            Debug.Log("currenttilenull");
            return NodeState.FAILURE;
        }
        return NodeState.SUCCESS;
    }
}

