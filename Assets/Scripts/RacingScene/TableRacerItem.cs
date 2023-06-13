using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableRacerItem : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private TextMeshProUGUI nameText, placeText;
    [SerializeField] private int place;
    [SerializeField] private CarControllerPro carControllerPro;
    public string Name => nameText.text;
    public int Place => place;
    public CarControllerPro Car => carControllerPro;
    
    public void Init(CarControllerPro car)
    {
        this.name = car.name;
        nameText.text = car.name;
        carControllerPro = car;
    }
    public void SetPlace(int place, string name, CarControllerPro car)
    {
        nameText.text = name;
        this.place = place;
        placeText.text = place.ToString();
        carControllerPro = car;
    }
   
}
