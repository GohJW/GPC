using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStatUIManager : MonoBehaviour
{
    public OverlayTile currentSelectedTile;
    public TMP_Text CharacterName;
    public TMP_Text Textbox1;
    public TMP_Text Textbox2;
    public GameObject SkipButton;
    public GameObject Skill1Button;
    public GameObject Skill2Button;
    public TMP_Text Cooldown;
    public TMP_Text Burn;
    public TMP_Text SkillDescription;

    private void Update()
    {
        CharacterName.text = currentSelectedTile.character.characterName;
        Textbox1.text = "Health:" + currentSelectedTile.character.CharacterHP.ToString();
        Textbox2.text = "Defense:" + currentSelectedTile.character.Defense.ToString();
        if (currentSelectedTile.character.Burntimer > 0 && (currentSelectedTile.isAlly || currentSelectedTile.isEnemy))
        {
            Burn.gameObject.SetActive(true);
            Burn.text = "Burning: " + currentSelectedTile.character.Burntimer.ToString() + " Turns Remaining";
        }
        else
        {
            Burn.gameObject.SetActive(false);
        }
        if (currentSelectedTile.isAlly)
        {
            SkipButton.gameObject.SetActive(true); 
            
            if (!currentSelectedTile.character.hasMoved)
            {
                Skill1Button.gameObject.SetActive(false);
                Skill2Button.gameObject.SetActive(false);
                SkipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Skip Movement Turn";
                Cooldown.gameObject.SetActive(false);
                SkillDescription.gameObject.SetActive(false);
            }
            else if (!currentSelectedTile.character.hasAttack)
            {
                Textbox1.text = "Range:" + currentSelectedTile.character.Attackrange.ToString();
                SkillDescription.text = currentSelectedTile.character.SkillDescription.ToString();
                SkillDescription.gameObject.SetActive(true);
                if (currentSelectedTile.character.Attack < 0)
                {
                    int healvalue = -currentSelectedTile.character.Attack;
                    Textbox2.text = "Heal:" + healvalue.ToString();
                }
                else
                {
                    Textbox2.text = "Attack:" + currentSelectedTile.character.Attack.ToString();
                }
                Skill1Button.gameObject.SetActive(true);
                Cooldown.gameObject.SetActive(true);
                if (currentSelectedTile.character.Skill2cooldown == 0)
                {
                    Cooldown.gameObject.SetActive(false);
                    Skill2Button.gameObject.SetActive(true);
                }
                else
                {
                    Cooldown.text = "Cooldown: " + currentSelectedTile.character.Skill2cooldown.ToString() + "Turns";
                    Cooldown.gameObject.SetActive(true);
                    Skill2Button.gameObject.SetActive(false);

                }
                Skill1Button.GetComponentInChildren<TextMeshProUGUI>().text = currentSelectedTile.character.Skill1;
                Skill2Button.GetComponentInChildren<TextMeshProUGUI>().text = currentSelectedTile.character.Skill2;
                SkipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Skip Attack Turn";
                
            }
            else
            {
                SkipButton.gameObject.SetActive(false);
                Skill1Button.gameObject.SetActive(false);
                Skill2Button.gameObject.SetActive(false);
                Cooldown.gameObject.SetActive(false);
                SkillDescription.gameObject.SetActive(false);
            }
        }
        else
        {
           
            SkipButton.gameObject.SetActive(false);
            Skill1Button.gameObject.SetActive(false);
            Skill2Button.gameObject.SetActive(false);
            Cooldown.gameObject.SetActive(false);
            SkillDescription.gameObject.SetActive(false);
        }
    }

    public void Skill1()
    {
        currentSelectedTile.character.Skillnumber = 1;
        currentSelectedTile.character.UpdateSkillinfo();
        currentSelectedTile.character.Attackrange = currentSelectedTile.character.Skill1attackrange;
        currentSelectedTile.character.Attack = currentSelectedTile.character.Skill1attack;
        
    }

    public void Skill2()
    {
        currentSelectedTile.character.Skillnumber = 2;
        currentSelectedTile.character.UpdateSkillinfo();
        currentSelectedTile.character.Attackrange = currentSelectedTile.character.Skill2attackrange;
        currentSelectedTile.character.Attack = currentSelectedTile.character.Skill2attack;
    }
}
