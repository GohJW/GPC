using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float speed;
    public GameObject currentSelectedcharacter;
    private CharacterInfo character;
    public OverlayTile CurrentSelectedTile;

    private Pathfinder pathfinder;
    private Rangefinder rangefinder;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinder();
        rangefinder = new Rangefinder();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTile = GetFocusedOnTile();

        if (focusedTile.HasValue)
        {
            OverlayTile overlayTile = focusedTile.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;


            if (Input.GetMouseButtonDown(0))
            {
                if (character == null)
                {
                    character = Instantiate(currentSelectedcharacter).GetComponent<CharacterInfo>();
                    PositionCharacterOntile(overlayTile);
                    GetInRangeTiles();
                    character.activeTile.isAlly = true;
                }

                else
                {

                    path = pathfinder.FindPath(character.activeTile, overlayTile, inRangeTiles);
                }
                
            }


        }
    

        if(path.Count > 0)
        {
            character.activeTile.isObstacle = false;
            character.activeTile.isAlly = false;
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);

        if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOntile(path[0]);
            path.RemoveAt(0);
        }

        if(path.Count == 0)
        { 
            character.activeTile.isObstacle = true;
            GetInRangeTiles();
        }
    }

    private void PositionCharacterOntile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        character.activeTile = tile;
       character.activeTile.ShowTile();
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }

    private void GetInRangeTiles()
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }

        inRangeTiles = rangefinder.GetTilesInRange(character.activeTile, 3);

        foreach(var item in inRangeTiles)
        {                       
                item.ShowTile();            
        }
    }

}
