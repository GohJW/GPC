using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{

    public bool Stage2Played;
    public bool Stage3Played;
    public bool Stage4Played;
    public bool Stage5Played;
    public bool Stage6Played;
    public bool Stage7Played;
    public bool Stage8Played;

    public StageData (AudioManager audiomanager)
    {
        Stage2Played = audiomanager.Stage2Played;
        Stage3Played = audiomanager.Stage3Played;
        Stage4Played = audiomanager.Stage4Played;
        Stage5Played = audiomanager.Stage5Played;
        Stage6Played = audiomanager.Stage6Played;
        Stage7Played = audiomanager.Stage7Played;
        Stage8Played = audiomanager.Stage8Played;
    }
}
