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
    private List<OverlayTile> path;
    private OverlayTile[] container;
    private List<OverlayTile> containerlist;

    private Node topNode;

    void Start()
    {
        CurrentTile = gameObject.GetComponent<CharacterInfo>().activeTile;
        //OverlayTile[] container = OverlayContainer.GetComponentsInChildren<OverlayTile>();
        FindClosestAlly();
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree()
    {
        InAttackrangeNode attackrangeNode = new InAttackrangeNode(this, inAttackRange, CurrentTile, AllyTile);
        MoveNode move = new MoveNode(this, CurrentTile, AllyTile,path, inRange, containerlist);

        topNode = new Selector(new List<Node> { attackrangeNode });
    }

    void Update()
    {
        if(!CurrentTile.character.hasAttack)
        {
            FindClosestAlly();          
            topNode.Evaluate();
        }
        
    }

    public void FindClosestAlly()
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
    }
    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    
}
