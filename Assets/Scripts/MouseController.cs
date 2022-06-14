using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float speed;
    private CharacterInfo character;
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
        if (!PauseMenu.isPaused)
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
                    if (CurrentSelectedTile.isAlly || CurrentSelectedTile.isEnemy || CurrentSelectedTile.isBarrel)
                    {
                        //UpdateStats();
                        CharacterStatUI.GetComponent<CharacterStatUIManager>().enabled = true;
                        CharacterStatUI.GetComponent<CharacterStatUIManager>().currentSelectedTile = CurrentSelectedTile;
                        ShowCharacterUI();
                    }
                    else
                    {
                        HideCharacterUI();
                        //HideAllTiles();
                    }
                    if (CurrentSelectedTile.isAlly)
                    {
                        ClearAllTiles();
                        character = CurrentSelectedTile.character;
                        character.Skillnumber = 1;
                        character.UpdateSkillinfo();
                        if (!character.hasMoved)
                        {
                            GetInRangeTiles();
                        }
                        if (character.hasMoved && !character.hasAttack)
                        {
                            GetInAttackRangeTiles();
                        }


                    }
                    if (inRangeTiles.Contains(CurrentSelectedTile) && !character.moving)
                    {
                        //if (!character.moving)
                        //{
                            path = pathfinder.FindPath(character.activeTile, CurrentSelectedTile, inRangeTiles);
                        //}
                    }

                    else if (!CurrentSelectedTile.isBarrel && !CurrentSelectedTile.isEnemy && !CurrentSelectedTile.isAlly && !character.moving)
                    {
                        ClearAllTiles();
                    }

                    if ((CurrentSelectedTile.isEnemy || CurrentSelectedTile.isBarrel) && inAttackRangeTiles.Contains(CurrentSelectedTile) && !character.hasAttack)
                    {

                        Attack();

                    }

                }

            }

            if (path.Count > 0 && inRangeTiles.Contains(CurrentSelectedTile))
            {               
                character.activeTile.isAlly = false;
                MoveAlongPath();
            }

        }
    }

    private void MoveAlongPath()
    {
        character.moving = true;
        character.animator.SetBool("Moving", character.moving);
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

            foreach (OverlayTile item in inRangeTiles)
            {
                item.HideTile();
            }
            inRangeTiles.Clear();
            character.moving = false;
            character.animator.SetBool("Moving", character.moving);
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
        foreach (OverlayTile item in inRangeTiles)
        {
            item.HideTile();
        }
        foreach (OverlayTile item in inAttackRangeTiles)
        {
            item.HideTile();
        }

        inRangeTiles = rangefinder.GetTilesInRange(character.activeTile, character.movementrange);

        foreach (OverlayTile item in inRangeTiles)
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

    public void GetInAttackRangeTiles()
    {
        foreach (OverlayTile item in inAttackRangeTiles)
        {
            item.HideTile();
        }
        foreach (OverlayTile item in inRangeTiles)
        {
            item.HideTile();
        }

        inAttackRangeTiles = attackrangefinder.GetTilesInAttackRange(character.activeTile, character.Attackrange);

        foreach (OverlayTile item in inAttackRangeTiles)
        {
            item.ShowTile();
        }
    }

    private void Attack()
    {

        CharacterInfo Attacked = CurrentSelectedTile.character;
        CharacterInfo Attacker = character;

        //Attacked.damaged = true;
        //StartCoroutine(DamagedAnimationDelay(Attacked));
        //Attacker.attacking = true;
        //StartCoroutine(AttackingAnimationDelay(Attacker));

        //Attacked.damaged = true;
        Attacked.animator.SetTrigger("Damaged");
        //Attacker.attacking = true;
        Attacker.animator.SetTrigger("Attacking");

        Attacked.CharacterHP -= Attacker.Attack * (1 - Attacked.Defense/100);
        Attacker.hasAttack = true;


        foreach (OverlayTile item in inAttackRangeTiles)
        {
            item.HideTile();
        }
        inAttackRangeTiles.Clear();

        if (Attacked.CharacterHP <= 0 && CurrentSelectedTile.isEnemy)
        {
            Attacked.GetComponent<SpriteRenderer>().enabled = false;
            CurrentSelectedTile.isEnemy = false;
            HideCharacterUI();
        }else if (CurrentSelectedTile.character.CharacterHP <= 0 && CurrentSelectedTile.isBarrel)
        {
            BarrelExplode(CurrentSelectedTile);
            Attacked.GetComponent<SpriteRenderer>().enabled = false;
            CurrentSelectedTile.isBarrel = false;
            HideCharacterUI();
        }
    }

    private void BarrelExplode(OverlayTile Barrel)
    {
        List<OverlayTile> explosion = attackrangefinder.GetTilesInAttackRange(Barrel, 1);
        explosion.Remove(Barrel); 
        foreach (OverlayTile item in explosion)
        {
            if (item.isEnemy || item.isAlly)
            {
                //item.character.damaged = true;
                item.character.animator.SetTrigger("Damaged");
                //StartCoroutine(DamagedAnimationDelay(item.character));
                item.character.CharacterHP -= 10 * (1 - item.character.Defense/100);
                if (item.character.CharacterHP <= 0)
                {
                    item.character.GetComponent<SpriteRenderer>().enabled = false;
                    item.isEnemy = false;
                    item.isAlly = false;
                }
            }    
        }       
    }
    public void ClearAllTiles()
    {
        foreach(OverlayTile item in inAttackRangeTiles)
        {
            item.HideTile();
        }
        foreach (OverlayTile item in inRangeTiles)
        {
            item.HideTile();
        }
        inRangeTiles.Clear();
        inAttackRangeTiles.Clear();
    }

    private void HideAllTiles()
    {
        foreach (OverlayTile item in inAttackRangeTiles)
        {
            item.HideTile();
        }
        foreach (OverlayTile item in inRangeTiles)
        {
            item.HideTile();
        }
    }

    public void SkipButton()
    {
        if (CurrentSelectedTile.isAlly == true && CurrentSelectedTile.character.hasMoved == false)
        {
            CurrentSelectedTile.character.hasMoved = true;
            HideAllTiles();
            inRangeTiles.Clear();
            GetInAttackRangeTiles();
        }else
        if (CurrentSelectedTile.isAlly == true && CurrentSelectedTile.character.hasAttack == false)
        {
            CurrentSelectedTile.character.hasAttack = true;
            ClearAllTiles();
            HideCharacterUI();
        }
    }

   public void Clearinfo()
    {
        character = null;
    }
}
