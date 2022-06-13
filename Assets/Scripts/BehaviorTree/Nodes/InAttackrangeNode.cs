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

    public InAttackrangeNode(EnemyAi ai, List<OverlayTile> inAttackRange, OverlayTile currentTile, OverlayTile allyTile)
    {
        this.ai = ai;
        this.inAttackRange = inAttackRange;
        CurrentTile = currentTile;
        AllyTile = allyTile;
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
    public void FindAttackRange()
    {
        inAttackRange.Clear();
        CurrentTile.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
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
                surroundingTiles.AddRange(MapManager.GetNeighbourTiles(item, new List<OverlayTile>()));
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
                pair.Key.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                inAttackRangeTiles.Add(pair.Key);

            }
        }

        return inAttackRangeTiles.Distinct().ToList();
    }
}



