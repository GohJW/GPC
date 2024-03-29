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
    private bool isInput = true;

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

                if (Input.GetMouseButtonDown(0) && isInput)
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
                          
                            if (character != null && character.Attack < 0 && inAttackRangeTiles.Contains(CurrentSelectedTile))
                            {
                                Heal();
                            }
                            ClearAllTiles();
                            character = CurrentSelectedTile.character;
                            character.Skillnumber = 1;
                            character.UpdateSkillinfo();
                            if (!character.hasMoved)
                            {
                               FindObjectOfType<AudioManager>().Play("ButtonClick");
                               GetInRangeTiles();
                            }
                            if (character.hasMoved && !character.hasAttack)
                            {
                               FindObjectOfType<AudioManager>().Play("ButtonClick");
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

                        if ((CurrentSelectedTile.isEnemy || CurrentSelectedTile.isBarrel) && inAttackRangeTiles.Contains(CurrentSelectedTile) && !character.hasAttack && character.Attack >= 0)
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
        isInput = false;
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
            isInput = true;

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
        FindObjectOfType<AudioManager>().Play(Attacker.characterName + Attacker.Skillnumber);
        Attacker.animator.SetTrigger("Attacking");     

        Attacked.CharacterHP -= Attacker.Attack * (1 - Attacked.Defense/100);
        Attacker.hasAttack = true;

        if (Attacked.CharacterHP <= 0 && CurrentSelectedTile.isEnemy)
        {
            Attacked.GetComponent<SpriteRenderer>().enabled = false;
            Attacked.gameObject.SetActive(false);
            CurrentSelectedTile.isEnemy = false;
            HideCharacterUI();
            if (Attacked.characterName == "Left Weapon" || Attacked.characterName == "Right Weapon")
            {
                GameObject.Find("AlienBossHead(Clone)").GetComponent<AlienBossScript>().CheckWeapons();
            }
        }
        else if (CurrentSelectedTile.character.CharacterHP <= 0 && CurrentSelectedTile.isBarrel)
        {
            CurrentSelectedTile.character.animator.SetTrigger("Explode");
            StartCoroutine(Delay((float)0.5, Attacked));
            BarrelExplode(CurrentSelectedTile);
            //Attacked.GetComponent<SpriteRenderer>().enabled = false;
            //Attacked.gameObject.SetActive(false);
            //CurrentSelectedTile.isBarrel = false;
            HideCharacterUI();
        }

        if ( Attacker.Attack == Attacker.Skill2attack && Attacker.Skill2cooldown == 0)
        {
            Attacker.Skill2cooldown += 3;
            if(Attacker.Skill2 == "Hose" || Attacker.Skill2 == "Blast")
            { 
                Push(Attacked, Attacker);
            }
            if (Attacker.Skill2 == "Nade")
            {
                BarrelExplode(Attacked.activeTile);
                Debug.Log("Grenade");
            }
        }
        if (Attacker.burn)
        {
            Attacked.Burntimer += 2;
            Attacked.burnicon.SetActive(true);
        }
        foreach (OverlayTile item in inAttackRangeTiles)
        {
            item.HideTile();
        }
        inAttackRangeTiles.Clear();
       
       
    }

    private void BarrelExplode(OverlayTile Barrel)
    {
        List<OverlayTile> explosion = attackrangefinder.GetTilesInAttackRange(Barrel, 1);
        foreach (OverlayTile item in explosion)
        {
            if (item.isEnemy || item.isAlly || item.isBarrel)
            {
                if(Barrel.character.characterName == ("Acid Barrel"))
                {
                    item.character.animator.SetTrigger("Damaged");
                    item.character.CharacterHP -= 5 * (1 - item.character.Defense / 100);
                    item.character.Burntimer += 2;
                    item.character.burnicon.SetActive(true);

                }
                else 
                {
                    item.character.animator.SetTrigger("Damaged");
                    item.character.CharacterHP -= 20 * (1 - item.character.Defense / 100);
                }
                if (item.character.CharacterHP <= 0)
                {
                    item.character.GetComponent<SpriteRenderer>().enabled = false;
                    item.isEnemy = false;
                    item.isAlly = false;
                    if(item.isBarrel)
                    {
                        FindObjectOfType<AudioManager>().Play("BarrelExplode");
                        BarrelExplode(item);
                        item.isBarrel = false;
                    }
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

    public void Heal()
    {
        CharacterInfo healer = character;
        CharacterInfo healed = CurrentSelectedTile.character;
        FindObjectOfType<AudioManager>().Play(healer.characterName + healer.Skillnumber);
        healed.CharacterHP -= healer.Attack;
        healed.animator.SetTrigger("Healed");
        if(healed.CharacterHP > healed.MaxHP)
        {
            healed.CharacterHP = healed.MaxHP;
        }
        healer.hasAttack = true;

        if (healer.Attack == healer.Skill2attack && healer.Skill2cooldown == 0)
        {
            healer.Skill2cooldown += 3;
        }

    }

    public void Push(CharacterInfo Attacked, CharacterInfo Attacker)
    {
        var map = MapManager.Instance.map;
        Vector2Int AttackerPosition = Attacker.activeTile.grid2DLocation;
        Vector2Int AttackedPosition = Attacked.activeTile.grid2DLocation;
        Vector2Int Difference = AttackedPosition - AttackerPosition;
        if(Attacker.characterName == "Gardener")
        {
            Difference = Difference * 2;
        }
        Vector2Int NewPosition = AttackedPosition + Difference;
        Vector2Int locationToCheck = new Vector2Int(NewPosition.x, NewPosition.y);
        Debug.Log("Push");
        if (map.ContainsKey(locationToCheck))
        {
            Debug.Log("containskey");

            if (!map[locationToCheck].isAlly && !map[locationToCheck].isEnemy && !map[locationToCheck].isObstacle && !map[locationToCheck].isBarrel)
            {
                Attacked.activeTile.isEnemy = false;
                Attacked.transform.position = new Vector3(map[locationToCheck].transform.position.x, map[locationToCheck].transform.position.y, map[locationToCheck].transform.position.z);
                Attacked.GetComponent<SpriteRenderer>().sortingOrder = map[locationToCheck].GetComponent<SpriteRenderer>().sortingOrder + 1;
                Attacked.activeTile = map[locationToCheck];
                map[locationToCheck].character = Attacked;
                Attacked.activeTile.isEnemy = true;
                Attacked.GetComponent<EnemyAi>().CurrentTile = Attacked.activeTile;
                Debug.Log("2nd");
                return;
            }
        }
        Difference = Difference / 2;
        NewPosition = AttackedPosition + Difference;
        locationToCheck = new Vector2Int(NewPosition.x, NewPosition.y);
        if (map.ContainsKey(locationToCheck))
        {
            Debug.Log("containskey");

            if (!map[locationToCheck].isAlly && !map[locationToCheck].isEnemy && !map[locationToCheck].isObstacle && !map[locationToCheck].isBarrel)
            {
                Attacked.activeTile.isEnemy = false;
                Attacked.transform.position = new Vector3(map[locationToCheck].transform.position.x, map[locationToCheck].transform.position.y, map[locationToCheck].transform.position.z);
                Attacked.GetComponent<SpriteRenderer>().sortingOrder = map[locationToCheck].GetComponent<SpriteRenderer>().sortingOrder + 1;
                Attacked.activeTile = map[locationToCheck];
                map[locationToCheck].character = Attacked;
                Attacked.activeTile.isEnemy = true;
                Attacked.GetComponent<EnemyAi>().CurrentTile = Attacked.activeTile;
                Debug.Log("1st");
                return;
            }
        }
    }

    IEnumerator Delay(float x, CharacterInfo Attacked)
    {
        yield return new WaitForSeconds(x);
        FindObjectOfType<AudioManager>().Play("BarrelExplode");
        Attacked.GetComponent<SpriteRenderer>().enabled = false;
        Attacked.gameObject.SetActive(false);
        CurrentSelectedTile.isBarrel = false;
    }
}
