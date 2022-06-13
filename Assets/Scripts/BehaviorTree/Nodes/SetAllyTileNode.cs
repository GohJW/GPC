using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAllyTileNode : Node
{
    private EnemyAi ai;
    private OverlayTile AllyTile;
    private OverlayTile CurrentTile;
    private GameObject OverlayContainer;

    public SetAllyTileNode(EnemyAi ai, OverlayTile allyTile, OverlayTile currentTile, GameObject overlayContainer)
    {
        this.ai = ai;
        AllyTile = allyTile;
        CurrentTile = currentTile;
        OverlayContainer = overlayContainer;
    }

    public override NodeState Evaluate()
    {
        FindClosestAlly();
        ai.AllyTile = AllyTile;
        return NodeState.SUCCESS;
    }

    public void FindClosestAlly()
    {
        AllyTile = null;
        OverlayTile[] container = OverlayContainer.GetComponentsInChildren<OverlayTile>();
        int shortestdistance = int.MaxValue;
        foreach (var item in container)
        {
            if (item.isAlly)
            {
                int distance = GetManhattenDistance(CurrentTile, item);
                if (distance < shortestdistance)
                {
                    shortestdistance = distance;
                    AllyTile = item;
                }

            }

        }
    }
    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }
}
