using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile overlaytilePrefab;
    public GameObject overlayContainer;
    public GameObject obstacleContainer;

    public GameObject obstaclePrefab1;
    public GameObject obstaclePrefab2;
    private ObstacleInfo obstacle;

    public GameObject CharacterPrefab1;
    public GameObject CharacterPrefab2;
    public GameObject Enemy;
    public GameObject Barrel1;

    private CharacterInfo character;

    public Canvas TurnUI;

    public Dictionary<Vector2Int, OverlayTile> map;
    public Dictionary<Vector3Int, int> obstacles;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, OverlayTile>();

        //generate a list of vectors to place obstacles on Grid
        obstacles = new Dictionary<Vector3Int, int>();
        obstacles.Add(new Vector3Int(-1, 0, 0), 1);
        obstacles.Add(new Vector3Int(-4, -2, 0), 2);

        BoundsInt bounds = tileMap.cellBounds;
    
        //generate grid map
        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tileLocation = new Vector3Int(x, y, 0);
                var tileKey = new Vector2Int(x, y);
                if (tileMap.HasTile(tileLocation))
                {

                    var overlayTile = Instantiate(overlaytilePrefab, overlayContainer.transform);
                    var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder + 1;
                    overlayTile.gridLocation = tileLocation;
                    map.Add(tileKey, overlayTile);

                    if (obstacles.ContainsKey(overlayTile.gridLocation))
                    {
                        PositionObstacleOntile(overlayTile, obstacles[overlayTile.gridLocation]);
                        overlayTile.isObstacle = true;
                    }          
                    
                    if(overlayTile.gridLocation == new Vector3Int(-6,-3,0))
                    {
                        character = Instantiate(CharacterPrefab1).GetComponent<CharacterInfo>();
                        PositionCharacterOntile(overlayTile);
                        overlayTile.character = character;
                        character.activeTile.isAlly = true;
                      
                    }
                    if (overlayTile.gridLocation == new Vector3Int(-6, -1, 0))
                    {
                        character = Instantiate(CharacterPrefab2).GetComponent<CharacterInfo>();
                        PositionCharacterOntile(overlayTile);
                        overlayTile.character = character;
                        character.activeTile.isAlly = true;
                       
                    }

                    if (overlayTile.gridLocation == new Vector3Int(2, 0, 0))
                    {
                        character = Instantiate(Enemy).GetComponent<CharacterInfo>();
                        PositionCharacterOntile(overlayTile);
                        overlayTile.character = character;
                        character.activeTile.isEnemy = true;
                       
                       
                    }

                    if (overlayTile.gridLocation == new Vector3Int(1, 0, 0))
                    {
                        character = Instantiate(Barrel1).GetComponent<CharacterInfo>();
                        PositionCharacterOntile(overlayTile);
                        overlayTile.character = character;
                        character.activeTile.isBarrel = true;
                    }

                }

            }
        }

        //NewTurn();
    }
    public static List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles)
    {
        Dictionary<Vector2Int, OverlayTile> TiletoSearch = new Dictionary<Vector2Int, OverlayTile>();
        var map = MapManager.Instance.map;

        if (searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                TiletoSearch.Add(item.grid2DLocation, item);
            }
        } else
        {
            TiletoSearch = map;
        }


        List<OverlayTile> neighbours = new List<OverlayTile>();

        //check Top
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            if (!TiletoSearch[locationToCheck].isObstacle)
            {
                neighbours.Add(TiletoSearch[locationToCheck]);
            }
        }

        //check Bottom
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            if (!TiletoSearch[locationToCheck].isObstacle)
            {
                neighbours.Add(TiletoSearch[locationToCheck]);
            }
        }

        //check Right
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            if (!TiletoSearch[locationToCheck].isObstacle)
            {
                neighbours.Add(TiletoSearch[locationToCheck]);
            }
        }

        //check Left
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);
        if (TiletoSearch.ContainsKey(locationToCheck))
        {
            if (!TiletoSearch[locationToCheck].isObstacle)
            {
                neighbours.Add(TiletoSearch[locationToCheck]);
            }
        }
        return neighbours;
    }
    private void PositionObstacleOntile(OverlayTile tile, int value)
    {
        if (value == 1)
        {


            obstacle = Instantiate(obstaclePrefab1, obstacleContainer.transform).GetComponent<ObstacleInfo>();

            obstacle.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
            obstacle.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            obstacle.activeTile = tile;
            
        }

        if (value == 2)
        {


            obstacle = Instantiate(obstaclePrefab2, obstacleContainer.transform).GetComponent<ObstacleInfo>();

            obstacle.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
            obstacle.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
            obstacle.activeTile = tile;
          
        }

    }

    private void PositionCharacterOntile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        character.activeTile = tile;     
    }


    //private bool AllMoved(OverlayTile[] container)
    //{
    //    foreach (var item in container)
    //    {
    //        if (item.isAlly == true && item.character.hasMoved == false)
    //        {
    //            return false;

    //        }
    //    }
    //    return true;
    //}

    public void NewTurn()
    {
        //TurnUI.GetComponent<TurnUIScript>().ShowEnemyTurn();
        OverlayTile[] container = overlayContainer.GetComponentsInChildren<OverlayTile>();
        foreach (var item in container)
        {
            if (item.isEnemy && !item.character.hasMoved)
            {
                //Execute Enemy AI actions here or create a new function for enemy turn movements
            }
        }

        foreach (var item in container)
        {
            item.HideTile();
            if (item.isAlly || item.isEnemy)
            {
                item.character.hasMoved = false;
                item.character.hasAttack = false;
            }
        }

       TurnUI.GetComponent<TurnUIScript>().ShowPlayerTurn();

    }
}
