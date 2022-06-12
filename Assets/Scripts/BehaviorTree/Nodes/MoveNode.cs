using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveNode : Node
{
    private EnemyAi ai;
    private OverlayTile CurrentTile;
    private OverlayTile AllyTile;
    private List<OverlayTile> path;
    private List<OverlayTile> inRange;
    private List<OverlayTile> containerlist;

    public MoveNode(EnemyAi ai, OverlayTile currentTile, OverlayTile allyTile, List<OverlayTile> path, List<OverlayTile> inRange, List<OverlayTile> containerlist)
    {
        this.ai = ai;
        CurrentTile = currentTile;
        AllyTile = allyTile;
        this.path = path;
        this.inRange = inRange;
        this.containerlist = containerlist;
    }

    public override NodeState Evaluate()
    {
        //inRange = GetTilesInRange(CurrentTile, CurrentTile.character.movementrange);
        path = FindPath(CurrentTile, AllyTile, containerlist);
        MoveAlongPath();
        CurrentTile.character.hasAttack = true;
        return NodeState.SUCCESS;
    }

    public List<OverlayTile> GetTilesInRange(OverlayTile startingTile, int range)
    {
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTiles.Add(startingTile);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while (stepCount < range)
        {
            var surroundingTiles = new List<OverlayTile>();

            foreach (var item in tileForPreviousStep)
            {
                //excludes nearby enemy and ally tiles
                if (item.isEnemy || item.isBarrel)
                {
                    continue;
                }
                if (item.isAlly && item != startingTile)
                {
                    continue;
                }
                surroundingTiles.AddRange(MapManager.GetNeighbourTiles(item, new List<OverlayTile>()));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;

        }

        return inRangeTiles.Distinct().ToList();
    }

    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchableTiles)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
            {
                //finalise path
                return GetFinishedList(start, end);
            }

            //error here
            var neighbourTiles = MapManager.GetNeighbourTiles(currentOverlayTile, searchableTiles);

            foreach (var neighbour in neighbourTiles)
            {
                if (neighbour.isObstacle || closedList.Contains(neighbour) || neighbour.isEnemy || neighbour.isAlly || neighbour.isBarrel)
                {
                    continue;
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previous = currentOverlayTile;

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }

            }

        }
        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();

        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    private void MoveAlongPath()
    {
        CurrentTile.character.moving = true;
        CurrentTile.character.animator.SetBool("Moving", CurrentTile.character.moving);
        var step = 5 * Time.deltaTime;

        CurrentTile.character.transform.position = Vector2.MoveTowards(CurrentTile.character.transform.position, path[0].transform.position, step);

        if (Vector2.Distance(CurrentTile.character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOntile(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            CurrentTile.isAlly = true;
            //character.activeTile.character = character;
            CurrentTile.character.hasMoved = true;
            CurrentTile.character.moving = false;
            CurrentTile.character.animator.SetBool("Moving", CurrentTile.character.moving);
        }
    }
    private void PositionCharacterOntile(OverlayTile tile)
    {
        CurrentTile.character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        CurrentTile.character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        CurrentTile.character.activeTile = tile;
    }

}
