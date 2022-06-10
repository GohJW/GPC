using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAi : MonoBehaviour
{
    public OverlayTile CurrentTile;
    public OverlayTile AllyTile;
    public GameObject OverlayContainer;
    public List<OverlayTile> inAttackRange;
    private List<OverlayTile> path;
    private OverlayTile[] container;

    private Node topNode;

    void Start()
    {
        CurrentTile = gameObject.GetComponent<CharacterInfo>().activeTile;
        //OverlayTile[] container = OverlayContainer.GetComponentsInChildren<OverlayTile>();
        FindClosestAlly();
        FindAttackRange();
        ConstructBehaviourTree();

    }

    private void ConstructBehaviourTree()
    {
        InAttackrangeNode attackrangeNode = new InAttackrangeNode(this, inAttackRange, CurrentTile, AllyTile);
        AttackNode attackNode = new AttackNode(this, CurrentTile, AllyTile);
        MovementNode movementNode = new MovementNode(this, CurrentTile, path);
        PathfinderNode pathfinderNode = new PathfinderNode(this, CurrentTile, AllyTile, container, path);
        EndNode endNode = new EndNode(this, CurrentTile);

        //  Sequence FindAllyinRangeSequence = new Sequence(new List<Node> { attackrangeNode});
        Sequence AttackSequence = new Sequence(new List<Node> { attackrangeNode, attackNode });
        Sequence AttackSequenceaftermoving = new Sequence(new List<Node> { attackrangeNode, attackNode });

        //Selector FindAllyinRangeAfterMovingSelector = new Selector(new List<Node> { attackNode, endNode });
        Sequence MoveSequence = new Sequence(new List<Node> { pathfinderNode, movementNode, AttackSequenceaftermoving });

        topNode = new Selector(new List<Node> { AttackSequence, MoveSequence });
    }

    void Update()
    {
        if(!CurrentTile.character.hasAttack)
        {
            FindClosestAlly();
            FindAttackRange();
            topNode.Evaluate();
        }
        
    }

    public void FindClosestAlly()
    {
        OverlayTile[] container = OverlayContainer.GetComponentsInChildren<OverlayTile>();
        int shortestdistance = int.MaxValue;
        foreach (OverlayTile item in container)
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

    public void FindAttackRange()
    {
        inAttackRange.Clear();
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
                inAttackRangeTiles.Add(pair.Key);

            }
        }

        return inAttackRangeTiles.Distinct().ToList();
    }
}
