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

    private void Update()
    {
        CharacterName.text = currentSelectedTile.character.characterName;
        CharacterHP.text = "Health:" + currentSelectedTile.character.CharacterHP.ToString();
        CharacterRange.text = "Movement Range:" + currentSelectedTile.character.movementrange.ToString();
    }
}
