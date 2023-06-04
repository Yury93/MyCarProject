using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMenu : MonoBehaviour
{
    public int indexCar;
    public bool IsAccessed;

    public const string ACCESSED = "ACCESSED";
    private void Awake()
    {
        if(Social1.PlayerPrefs.GetInt(ACCESSED + indexCar)  == indexCar)
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
