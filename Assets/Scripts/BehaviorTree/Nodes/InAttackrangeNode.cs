using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InAttackrangeNode : Node
{
    private EnemyAi ai;
    private List<OverlayTile> inAttackRange;
    private OverlayTile CurrentTile;
    private OverlayTile AllyTile;
    private Animator animator;

    public InAttackrangeNode(EnemyAi ai, List<OverlayTile> inAttackRange, OverlayTile currentTile, OverlayTile allyTile, Animator animator)
    {
        this.ai = ai;
        this.inAttackRange = inAttackRange;
        CurrentTile = currentTile;
        AllyTile = allyTile;
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        FindAttackRange();
        if (inAttackRange.Contains(AllyTile))
        {
            Attack();
            Debug.Log("attacked");
            Debug.Log("Success");
            return NodeState.SUCCESS;
        }
        Debug.Log("Failure");
        if(CurrentTile.character.hasMoved)
        {
            CurrentTile.character.hasAttack = true;
        }
        return NodeState.FAILURE;

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
        if (Attacked.CharacterHP <= 0)
        {
            Attacked.GetComponent<SpriteRenderer>().enabled = false;
            AllyTile.isAlly = false;
            Attacked.gameObject.SetActive(false);
        }
    }
    public void FindAttackRange()
    {
        inAttackRange.Clear();
        //CurrentTile.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        inAttackRange = GetTilesInAttackRange(CurrentTile, CurrentTile.character.Attackrange);
    }
    public Dictionary<OverlayTile, int> tiles;
    public List<OverlayTile> GetTilesInAttackRange(OverlayTile startingTile, int attackrange)
    {
        var inRangeTiles = new List<OverlayTile>();
        var inAttackRangeTiles = new List<OverlayTile>();
        int stepCount = 0;
        tiles = new Dictionary<OverlayTile, int>();


        inRangeTiles.Add(startingTile);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while (stepCount <= attackrange)
        {
            var surroundingTiles = new List<OverlayTile>();
            //for each iteration, add tile into dictionary if not found in dictionary yet
            foreach (var item in tileForPreviousStep)
            {
                if (!tiles.ContainsKey(item))
                {
                    tiles.Add(item, stepCount);
                }
                surroundingTiles.AddRange(GetNeighbourTilesforAttack(item, new List<OverlayTile>()));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;

        }
        //for tiles in dictionary with same value as attack range, add to list inAttackRangeTiles
        foreach (KeyValuePair<OverlayTile, int> pair in tiles)
        {
            if (pair.Value == attackrange)
            {
                //pair.Key.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                inAttackRangeTiles.Add(pair.Key);

            }
        }

        return inAttackRangeTiles.Distinct().ToList();
    }
    public static List<OverlayTile> GetNeighbourTilesforAttack(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles)
    {
        Dictionary<Vector2Int, OverlayTile> TiletoSearch = new Dictionary<Vector2Int, OverlayTile>();
        var map = MapManager.Instance.map;

        if (searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                TiletoSearch.Add(item.grid2DLocation, item);
            }
        }
        else
        {
            TiletoSearch = map;
        }


        List<OverlayTile> neighbours = new List<OverlayTile>();

        //check Top
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(TiletoSearch[locationToCheck]);
        }

        //check Bottom
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(TiletoSearch[locationToCheck]);
        }

        //check Right
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(TiletoSearch[locationToCheck]);
        }

        //check Left
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(TiletoSearch[locationToCheck]);
        }
        return neighbours;
    }

}



