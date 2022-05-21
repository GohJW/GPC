using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUIManager : MonoBehaviour
{
    public OverlayTile currentSelectedTile;
    public CharacterInfo characterinfo;
    public Text CharacterName;
    public Text CharacterHP;
    public Text CharacterRange;

    private void Update()
    {
        CharacterName.text = currentSelectedTile.character.characterName;
        CharacterHP.text = "Health:" + currentSelectedTile.character.CharacterHP.ToString();
        CharacterRange.text = "Range:" + currentSelectedTile.character.range.ToString();
    }
}
