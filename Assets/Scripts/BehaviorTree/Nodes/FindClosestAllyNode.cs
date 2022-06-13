using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestAllyNode : Node
{
    private EnemyAi ai;
    private OverlayTile AllyTile;
    private OverlayTile CurrentTile;
    private GameObject OverlayContainer;

    public FindClosestAllyNode(EnemyAi ai, OverlayTile allyTile, OverlayTile currentTile, GameObject overlayContainer)
    {
        this.ai = ai;
        this.AllyTile = allyTile;
        this.CurrentTile = currentTile;
        this.OverlayContainer = overlayContainer;
    }

    public override NodeState Evaluate()
    {
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
        if(AllyTile == null)
        {
            Debug.Log("allytilenull");
            return NodeState.FAILURE;
        }
        return NodeState.SUCCESS;
    }
    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }
}
