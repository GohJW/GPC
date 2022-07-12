using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tutorial
{
    [TextArea(3, 10)]
    public string[] instructions;

    public Image[] pictures;
}
