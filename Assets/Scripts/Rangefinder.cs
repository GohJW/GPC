using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rangefinder 
{
  public List<OverlayTile> GetTilesInRange( OverlayTile startingTile, int range)
    {
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTiles.Add(startingTile);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while(stepCount < range)
        {
            var surroundingTiles = new List<OverlayTile>();

            foreach (var item in tileForPreviousStep)
            {
                if (item.isEnemy == true)
                {
                    continue;
                }
                if(item.isAlly == true && item != startingTile)
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
    
}
