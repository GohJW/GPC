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
    public List<OverlayTile> inRange;
    public List<OverlayTile> path;
    private OverlayTile[] container;
    private List<OverlayTile> containerlist;
    private Animator animator;

    private Node topNode;
    private Node AttackNode;

    void Start()
    {
        CurrentTile = gameObject.GetComponent<CharacterInfo>().activeTile;
        animator = CurrentTile.character.GetComponent<Animator>();
        //////OverlayTile[] container = OverlayContainer.GetComponentsInChildren<OverlayTile>();
        FindClosestAlly();
        //ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree()
    {
        SetCurrentTileNode setCurrentTileNode = new SetCurrentTileNode(this, CurrentTile, CurrentTile.character);
        FindClosestAllyNode findClosestAllyNode = new FindClosestAllyNode(this, AllyTile, CurrentTile, OverlayContainer);
        InAttackrangeNode attackrangeNode = new InAttackrangeNode(this, inAttackRange, CurrentTile, AllyTile);
        MoveNode moveNode = new MoveNode(this, CurrentTile, AllyTile, path, containerlist);
        SetAllyTileNode setAllyTileNode = new SetAllyTileNode(this, AllyTile, CurrentTile, OverlayContainer);

        Sequence CheckFirstAttackrangeSequence = new Sequence(new List<Node> { attackrangeNode });
        Sequence moveSequence = new Sequence(new List<Node> { findClosestAllyNode, moveNode, setCurrentTileNode, setAllyTileNode, findClosestAllyNode, attackrangeNode});

        topNode = new Selector(new List<Node> {attackrangeNode, moveSequence});
    }

    void Update()
    {
        if(!CurrentTile.character.hasAttack)
        {
            FindClosestAlly();
            ConstructBehaviourTree();
            topNode.Evaluate();
            if (CurrentTile.character.hasMoved)
            {
                AttackNode = new InAttackrangeNode(this, inAttackRange, CurrentTile, AllyTile);
                AttackNode.Evaluate();
            }
        }
        
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
