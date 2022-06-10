using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementNode : Node
{
    private EnemyAi ai;
    private OverlayTile CurrentTile;
    private List<OverlayTile> path;

    public MovementNode(EnemyAi ai, OverlayTile currentTile, List<OverlayTile> path)
    {
        this.ai = ai;
        CurrentTile = currentTile;
        this.path = path;
    }

    public override NodeState Evaluate()
    {
        CurrentTile.isEnemy = false;
        MoveAlongPath();
        return NodeState.SUCCESS;
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
            CurrentTile.isEnemy = true;
            CurrentTile.character.hasMoved = true;

            //inRangeTiles.Clear();
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
