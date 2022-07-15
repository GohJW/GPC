using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile overlaytilePrefab;
    public GameObject overlayContainer;
    public GameObject obstacleContainer;

    public GameObject obstaclePrefab1;
    public GameObject obstaclePrefab2;
    public GameObject obstaclePrefab3;
    public GameObject obstaclePrefab4;
    private ObstacleInfo obstacle;

    public GameObject CharacterPrefab1;
    public GameObject CharacterPrefab2;

    public GameObject EnemyMelee;
    public GameObject EnemyRanged;
    public GameObject Barrel1;
    public GameObject Barrel2;

    private CharacterInfo character;

    public Canvas TurnUI;

    public TMP_Text TurnCounter;
    public int TurnNumber = 1;

    public Dictionary<Vector2Int, OverlayTile> map;
    public Dictionary<Vector3Int, int> obstacles;
    public Dictionary<Vector3Int, int> charspawns;

    public CharacterDatabase characterDB;
    [SerializeField]
    public int scenenumber;
    public  Vector3Int[] obstaclepositions;
    public Vector3Int[] charspawnpositions;
    public Vector3Int[] mobpositions;
    public int[] mobtype;


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
        Spawner(scenenumber);
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

        if (value == 3)
        {


            obstacle = Instantiate(obstaclePrefab3, obstacleContainer.transform).GetComponent<ObstacleInfo>();

            obstacle.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
            obstacle.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
            obstacle.activeTile = tile;

        }

        if (value == 4)
        {


            obstacle = Instantiate(obstaclePrefab4, obstacleContainer.transform).GetComponent<ObstacleInfo>();

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
                if (item.character.Burntimer > 0)
                {
                    Burn(item.character);
                    item.character.Burntimer--;
                }
                    if(item.character.Burntimer == 0)
                {
                    item.character.burnicon.SetActive(false);
                }
                if (item.character.GetComponent<SpriteRenderer>().enabled == true)
                {
                    item.character.hasMoved = false;
                    item.character.hasAttack = false;
                    if (item.isAlly)
                    {
                        item.character.Skill2cooldown--;
                    }
                }
            }
        }
        TurnNumber++;
        TurnCounter.text = "Turn " + TurnNumber.ToString();
        TurnUI.GetComponent<TurnUIScript>().ShowPlayerTurn();

    }

    public void CheckWin()
    {
        OverlayTile[] container = overlayContainer.GetComponentsInChildren<OverlayTile>();
        bool gameend = false;
        foreach(OverlayTile item in container)
        {
            if (item.isEnemy)
            {
                NewTurn();
                gameend = false;
                break;
            }
            else
            {
                gameend = true;
            }
        }
        if(gameend)
        {
            StartCoroutine(Nextstage((float)1.1, gameend));
            TurnUI.GetComponent<TurnUIScript>().ShowWin();   
        }

    }

    public void CheckLose()
    {
        OverlayTile[] container = overlayContainer.GetComponentsInChildren<OverlayTile>();
        bool gameend = false;
        foreach (OverlayTile item in container)
        {
            if (item.isAlly)
            {
                gameend = false;
                break;
            }
            else
            {
                gameend = true;
            }
        }
        if (gameend)
        {
            StartCoroutine(Restartstage((float)1.1, gameend));
            TurnUI.GetComponent<TurnUIScript>().ShowLose();
        }

    }
    public IEnumerator Nextstage(float x, bool gameend)
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameend = false;
    }

    public IEnumerator Restartstage(float x, bool gameend)
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        gameend = false;
    }

    public void SpawnCharacter(OverlayTile overlayTile, int i)
    {
        
        int selectedOption = CharacterManager.selectedOptionIndex[i];
        character = Instantiate(characterDB.GetCharacter(selectedOption)).GetComponent<CharacterInfo>();
        PositionCharacterOntile(overlayTile);
        overlayTile.character = character;
        character.activeTile.isAlly = true;
    }

    public void Burn(CharacterInfo character)
    {
        FindObjectOfType<AudioManager>().Play("Burn");
        character.CharacterHP -= 5;
        character.animator.SetTrigger("Damaged");
        if (character.CharacterHP <= 0)
        {
            character.GetComponent<SpriteRenderer>().enabled = false;
            character.burnicon.SetActive(false);
            character.activeTile.isEnemy = false;
            character.activeTile.isAlly = false;
            character.gameObject.SetActive(false);
        }
    }


    public void Spawner(int stagenumber)
    {
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, OverlayTile>();

        //generate a list of vectors to place obstacles on Grid
        obstacles = new Dictionary<Vector3Int, int>();
        obstacles.Add(obstaclepositions[0], 1);
        obstacles.Add(obstaclepositions[1], 2);
        obstacles.Add(obstaclepositions[2], 3);
        obstacles.Add(obstaclepositions[3], 4);

        charspawns = new Dictionary<Vector3Int, int>();
        charspawns.Add(charspawnpositions[0], 0);
        charspawns.Add(charspawnpositions[1], 1);
        charspawns.Add(charspawnpositions[2], 2);




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

                    if (charspawns.ContainsKey(overlayTile.gridLocation))
                    {
                        SpawnCharacter(overlayTile, charspawns[overlayTile.gridLocation]);
                    }

                    if(mobpositions.Contains(overlayTile.gridLocation))
                    {
                        for(int i = 0; i < mobpositions.Length; i++)
                        {
                            if (mobpositions[i] == overlayTile.gridLocation)
                            {
                                SpawnMob(overlayTile, mobtype[i]);
                                break;
                            }
                        }
                    }

                    //if (stagenumber == 1)
                    //{
                    //    if (overlayTile.gridLocation == new Vector3Int(2, 0, 0))
                    //    {
                    //        character = Instantiate(EnemyMelee).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }
                    //    if (overlayTile.gridLocation == new Vector3Int(2, -2, 0))
                    //    {
                    //        character = Instantiate(EnemyMelee).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;

                    //    }

                    //    if (overlayTile.gridLocation == new Vector3Int(1, 0, 0))
                    //    {
                    //        character = Instantiate(Barrel1).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isBarrel = true;
                    //    }
                    //}
                    //if (stagenumber == 2)
                    //{
                    //    if (overlayTile.gridLocation == new Vector3Int(-3, 1, 0))
                    //    {
                    //        character = Instantiate(EnemyMelee).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }
                    //    if (overlayTile.gridLocation == new Vector3Int(-4, 2, 0))
                    //    {
                    //        character = Instantiate(EnemyRanged).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }

                    //    if (overlayTile.gridLocation == new Vector3Int(-3, -2, 0))
                    //    {
                    //        character = Instantiate(Barrel1).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isBarrel = true;
                    //    }
                    //}
                    //if (stagenumber == 3)
                    //{
                    //    if (overlayTile.gridLocation == new Vector3Int(2, 0, 0))
                    //    {
                    //        character = Instantiate(EnemyRanged).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }
                    //    if (overlayTile.gridLocation == new Vector3Int(1, -3, 0))
                    //    {
                    //        character = Instantiate(EnemyMelee).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }
                    //    if (overlayTile.gridLocation == new Vector3Int(-1, 1, 0))
                    //    {
                    //        character = Instantiate(EnemyRanged).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }

                    //    if (overlayTile.gridLocation == new Vector3Int(-1, -1, 0))
                    //    {
                    //        character = Instantiate(Barrel1).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isBarrel = true;
                    //    }
                    //    if (overlayTile.gridLocation == new Vector3Int(-1, -4, 0))
                    //    {
                    //        character = Instantiate(Barrel1).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isBarrel = true;
                    //    }
                    //}
                    //if(stagenumber == 4)
                    //{
                    //    if (overlayTile.gridLocation == new Vector3Int(-5, 1, 0))
                    //    {
                    //        character = Instantiate(EnemyRanged).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }
                    //    if (overlayTile.gridLocation == new Vector3Int(-4, 0, 0))
                    //    {
                    //        character = Instantiate(EnemyMelee).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }
                    //    if (overlayTile.gridLocation == new Vector3Int(-4, 1, 0))
                    //    {
                    //        character = Instantiate(EnemyMelee).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isEnemy = true;
                    //        character.hasAttack = true;
                    //        character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
                    //    }

                    //    if (overlayTile.gridLocation == new Vector3Int(-3, -3, 0))
                    //    {
                    //        character = Instantiate(Barrel1).GetComponent<CharacterInfo>();
                    //        PositionCharacterOntile(overlayTile);
                    //        overlayTile.character = character;
                    //        character.activeTile.isBarrel = true;
                    //    }
                    //}

                }

            }
        }
    }
    private void SpawnMob(OverlayTile overlaytile, int mobtype)
    {
        if(mobtype == 1) //melee alien
        {
            character = Instantiate(EnemyMelee).GetComponent<CharacterInfo>();
            PositionCharacterOntile(overlaytile);
            overlaytile.character = character;
            character.activeTile.isEnemy = true;
            character.hasAttack = true;
            character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
        }
        if (mobtype == 2) //ranged alien
        {
            character = Instantiate(EnemyRanged).GetComponent<CharacterInfo>();
            PositionCharacterOntile(overlaytile);
            overlaytile.character = character;
            character.activeTile.isEnemy = true;
            character.hasAttack = true;
            character.GetComponent<EnemyAi>().OverlayContainer = overlayContainer;
        }
        if (mobtype == 3) // explosive barrel
        {
            character = Instantiate(Barrel1).GetComponent<CharacterInfo>();
            PositionCharacterOntile(overlaytile);
            overlaytile.character = character;
            character.activeTile.isBarrel = true;
        }
        if (mobtype == 4) // acid barrel
        {
            character = Instantiate(Barrel2).GetComponent<CharacterInfo>();
            PositionCharacterOntile(overlaytile);
            overlaytile.character = character;
            character.activeTile.isBarrel = true;
        }
    }
}