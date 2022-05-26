using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStatUIManager : MonoBehaviour
{
    public OverlayTile currentSelectedTile;
    public TMP_Text CharacterName;
    public TMP_Text CharacterHP;
    public TMP_Text CharacterRange;
    public TMP_Text CharacterAttack;
    public TMP_Text CharacterDefense;
    public GameObject Button;

    private void Update()
    {
        CharacterName.text = currentSelectedTile.character.characterName;
        CharacterHP.text = "Health:" + currentSelectedTile.character.CharacterHP.ToString();
        CharacterRange.text = "Range:" + currentSelectedTile.character.movementrange.ToString();
        CharacterAttack.text = "Attack:" + currentSelectedTile.character.Attack.ToString();
        CharacterDefense.text = "Defense:" + currentSelectedTile.character.Defense.ToString();
        if (currentSelectedTile.isAlly == true)
        {
            Button.gameObject.SetActive(true);
            if (currentSelectedTile.character.hasMoved == false)
            {
                Button.GetComponentInChildren<TextMeshProUGUI>().text = "Skip Movement Turn";
            }
            else if (currentSelectedTile.character.hasAttack == false)
            {
                Button.GetComponentInChildren<TextMeshProUGUI>().text = "Skip Attack Turn";
            }
            else
            {
                Button.gameObject.SetActive(false);
            }
        }else
        {
            Button.gameObject.SetActive(false);
        }
    }
}
