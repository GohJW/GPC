using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveNode : Node
{
    private EnemyAi ai;
    private OverlayTile CurrentTile;
    private CharacterInfo Character;
    private OverlayTile AllyTile;
    private List<OverlayTile> path;
    private List<OverlayTile> containerlist;
    

    public MoveNode(EnemyAi ai, OverlayTile currentTile, OverlayTile allyTile, List<OverlayTile> path, List<OverlayTile> containerlist)
    {
        this.ai = ai;
        CurrentTile = currentTile;
        AllyTile = allyTile;
        this.path = path;
        this.containerlist = containerlist;
 
    }

    public override NodeState Evaluate()
    {
        //path.Clear(); // if i clear path error null, if i dont clear it doesnt reset the pathfinding
        path = FindPath(CurrentTile, AllyTile);
        path.Remove(AllyTile);

        if (path.Count == 0)
        {
            Character = CurrentTile.character;
            Debug.Log("Count0");
            path.Clear();
            Moveaway();
        }
        else
        {
            Character = CurrentTile.character;
            CurrentTile.isEnemy = false;
            MoveAlongPath();
            //CurrentTile.character.hasAttack = true;
        }
        return NodeState.SUCCESS;
    }

    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
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

            var neighbourTiles = GetNeighbourOverlayTiles(currentOverlayTile);

            foreach (var neighbour in neighbourTiles)
            {
                if (neighbour.isObstacle || closedList.Contains(neighbour) || neighbour.isEnemy || neighbour.isAlly || neighbour.isBarrel)
                {
                    if (neighbour != AllyTile)
                    {
                        continue;
                    }
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
           // currentTile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
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
        int originalpathlength = path.Count;
        CurrentTile.character.moving = true;
        CurrentTile.character.animator.SetBool("Moving", CurrentTile.character.moving);
        float step = 5 * Time.deltaTime;
        if (path.Count != Character.Attackrange - 1)
        {
            while (path.Count > originalpathlength - CurrentTile.character.movementrange && path.Count != CurrentTile.character.Attackrange - 1)
            {
                CurrentTile.character.transform.position = Vector2.MoveTowards(CurrentTile.character.transform.position, path[0].transform.position, step);


                if (Vector2.Distance(CurrentTile.character.transform.position, path[0].transform.position) < 0.0001f)
                {
                    Debug.Log("move");
                    PositionCharacterOntile(path[0]);
                    path.RemoveAt(0);
                }
            }
            CurrentTile = Character.activeTile;
            CurrentTile.character = Character;
            CurrentTile.isEnemy = true;
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

    private List<OverlayTile> GetNeighbourOverlayTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbours = new List<OverlayTile>();

        //right
        Vector2Int locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //left
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //top
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y + 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //bottom
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y - 1
        );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }
     IEnumerator Move(float step)
    {
        while (Vector2.Distance(CurrentTile.character.transform.position, path[0].transform.position) > 0.0001f)
        {
            CurrentTile.character.transform.position = Vector2.MoveTowards(CurrentTile.character.transform.position, path[0].transform.position, step);

            yield return null;
        }
    }

    public void Moveaway()
    {
        float step = 5 * Time.deltaTime;
        var map = MapManager.Instance.map;
        int value = 0;
        var neighbourtiles = GetNeighbourOverlayTiles(CurrentTile);
        foreach (OverlayTile item in neighbourtiles)
        {
            if (!item.isBarrel && !item.isAlly && !item.isEnemy && !item.isObstacle)
            {
                Debug.Log("pathadd");
                value++;
                path.Add(item);
            }
        }
        if (path.Count == 0)
        {
            CurrentTile.character.hasMoved = true;
            return;
        }
        else
        {
            CurrentTile.isEnemy = false;
            int index = Random.Range(0, path.Count - 1);
            Debug.Log(index);
            
            while (path.Count != 0)
            {
                CurrentTile.character.transform.position = Vector2.MoveTowards(CurrentTile.character.transform.position, path[index].transform.position, step);
                if (Vector2.Distance(CurrentTile.character.transform.position, path[index].transform.position) < 0.0001f)
                {
                    Debug.Log("move");
                    PositionCharacterOntile(path[index]);
                    path.Clear();
                }
            }

            CurrentTile = Character.activeTile;
            CurrentTile.character = Character;
            CurrentTile.isEnemy = true;
            CurrentTile.character.hasMoved = true;
            CurrentTile.character.moving = false;
            CurrentTile.character.animator.SetBool("Moving", CurrentTile.character.moving);
        }
    }
}
