using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonRacing : MonoBehaviour
{
    public static int CurrentSeason 
    {
        get{ return Social1.PlayerPrefs.GetInt("currentSeason"); }
        set{ Social1.PlayerPrefs.SetInt("currentSeason", value); }
    }
    
}
