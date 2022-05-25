using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float speed;
    private CharacterInfo character;
    private Attackinfo attack;
    public OverlayTile CurrentSelectedTile;
    public Canvas CharacterStatUI;

    private Pathfinder pathfinder;
    private Rangefinder rangefinder;
    private AttackRangefinder attackrangefinder;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> inAttackRangeTiles = new List<OverlayTile>();

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinder();
        rangefinder = new Rangefinder();
        attackrangefinder = new AttackRangefinder();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTile = GetFocusedOnTile();

        if (focusedTile.HasValue)
        {
            OverlayTile overlaytile = focusedTile.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlaytile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlaytile.GetComponent<SpriteRenderer>().sortingOrder;


            if (Input.GetMouseButtonDown(0))
            {
                CurrentSelectedTile = overlaytile;
                if (CurrentSelectedTile.isAlly == true || CurrentSelectedTile.isEnemy == true || CurrentSelectedTile.isBarrel == true)
                {
                    CharacterStatUI.GetComponent<CharacterStatUIManager>().enabled = true;
                    CharacterStatUI.GetComponent<CharacterStatUIManager>().currentSelectedTile = CurrentSelectedTile;
                    ShowCharacterUI();
                }
                else
                {
                    HideCharacterUI();
                }


                if (CurrentSelectedTile.isAlly == true)
                {
                    character = CurrentSelectedTile.character;
                    if (character.hasMoved == false)
                    {
                        GetInRangeTiles();
                    }
                    if (character.hasMoved == true && character.hasAttack == false)
                    {
                        GetInAttackRangeTiles();
                    }
                }
                if (inRangeTiles.Contains(CurrentSelectedTile))
                {
                    path = pathfinder.FindPath(character.activeTile, CurrentSelectedTile, inRangeTiles);
                }

                if(CurrentSelectedTile.isEnemy == true && inAttackRangeTiles.Contains(CurrentSelectedTile) && character.hasAttack == false) 
                {

                    Attack();
                    
                }

                if (CurrentSelectedTile.isBarrel == true && inAttackRangeTiles.Contains(CurrentSelectedTile) && character.hasAttack == false)
                {
                    Attack();

                }

            }

        }

        if(path.Count > 0)
        {
            character.activeTile.isAlly = false;
            MoveAlongPath();
            
        }
        

    }

    private void MoveAlongPath()
    {
        character.moving = true;
        var step = speed * Time.deltaTime;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);

        if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOntile(path[0]);
            path.RemoveAt(0);
        }

        if(path.Count == 0)
        { 
            character.activeTile.isAlly = true;
            character.activeTile.character = character;
            character.hasMoved = true;

            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }
            inRangeTiles.Clear();
            character.moving = false;
        }
    }

    private void PositionCharacterOntile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        character.activeTile = tile;
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
        foreach (var item in inAttackRangeTiles)
        {
            item.HideTile();
        }

        inRangeTiles = rangefinder.GetTilesInRange(character.activeTile, character.movementrange);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    public void HideCharacterUI()
    {
        CharacterStatUI.GetComponent<Canvas>().enabled = false;
    }

    public void ShowCharacterUI()
    {
        CharacterStatUI.GetComponent<Canvas>().enabled = true;
    }

    private void GetInAttackRangeTiles()
    {
        foreach (var item in inAttackRangeTiles)
        {
            item.HideTile();
        }
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }

        inAttackRangeTiles = attackrangefinder.GetTilesInAttackRange(character.activeTile, character.attackrange);//hardcoded 2 for now, attackinfo dont work

        foreach (var item in inAttackRangeTiles)
        {
            item.ShowTile();
        }
    }

    private void Attack()
    {
        CurrentSelectedTile.character.CharacterHP -= character.Attack;
        character.hasAttack = true;

        foreach (var item in inAttackRangeTiles)
        {
            item.HideTile();
        }
        inAttackRangeTiles.Clear();

        if (CurrentSelectedTile.character.CharacterHP <= 0 && CurrentSelectedTile.isEnemy)
        {
            CurrentSelectedTile.character.GetComponent<SpriteRenderer>().enabled = false;
            CurrentSelectedTile.isEnemy = false;
            HideCharacterUI();
        }

        if (CurrentSelectedTile.character.CharacterHP <= 0 && CurrentSelectedTile.isBarrel)
        {
            BarrelExplode();
            CurrentSelectedTile.character.GetComponent<SpriteRenderer>().enabled = false;
            CurrentSelectedTile.isBarrel = false;
            HideCharacterUI();
        }
    }

    private void BarrelExplode()
    {
        
    }
}
