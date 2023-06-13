using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMenu : MonoBehaviour
{
    public int indexCar;
    public bool IsAccessed;

    public const string ACCESSED = "ACCESSED";
    public int plPLayers;
    public void Init()
    {
        Debug.Log(ACCESSED + indexCar + "  Àﬁ◊  Œ“Œ–€… œŒÀ”◊¿≈Ã---  Ò‡Ï ËÌ‰ÂÍÒ ---"+Social1.PlayerPrefs.GetInt(ACCESSED + indexCar));
        plPLayers = Social1.PlayerPrefs.GetInt(ACCESSED + indexCar);
        if(plPLayers  == indexCar)
        {
            IsAccessed = true;
        }
    }
    public void SetIsAccessed(bool isAccessed)
    {
        IsAccessed = isAccessed;
        Social1.PlayerPrefs.SetInt(ACCESSED + indexCar, indexCar);
    }
  
}
