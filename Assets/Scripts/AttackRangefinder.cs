using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackRangefinder
{
    public List<OverlayTile> GetTilesInAttackRange(OverlayTile startingTile, int attackrange)
    {
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTiles.Add(startingTile);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while (stepCount < attackrange)
        {
            var surroundingTiles = new List<OverlayTile>();

            foreach (var item in tileForPreviousStep)
            {
                if (item.isEnemy == true)
                {
                    surroundingTiles.AddRange(MapManager.GetNeighbourTiles(item, new List<OverlayTile>())); ;
                }           

            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;

        }

        return inRangeTiles.Distinct().ToList();
    }

}