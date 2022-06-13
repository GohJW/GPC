using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{

    public CharacterInfo[] character;

    public int CharacterCount
    {
        get 
        { 
            return character.Length;
        }
    }

    public CharacterInfo GetCharacter(int index)
    {
        return character[index];
    }
}
