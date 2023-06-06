using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRacers : MonoBehaviour
{
    [SerializeField] public List<CarControllerPro> cars;
    [SerializeField] private TableRacerItem prefabRacerItem;
    [SerializeField] private List<TableRacerItem> finalTableItem; 
    [SerializeField] private Transform content;
    [SerializeField] private CheckPoints checkPoints;
    public List<CarControllerPro> Cars => cars;
    public CheckPoints CheckPoints => checkPoints;
    public List<TableRacerItem> racerItems;
    public static TableRacers instance;
    public CarControllerPro playerInstance;
    public static bool IsFInish = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //racerItems = new List<TableRacerItem>();
        //foreach (var car in cars)
        //{
            
        //        var item = Instantiate(prefabRacerItem, content);
        //        item.Init(car);
        //        racerItems.Add(item);
        //        Debug.Log("car");
            
        //}
    
    }

   public void ShowPositionOnLeaderBoard(List<CarPosition> carPositions)
    {
       
        for (int i = 0; i < carPositions.Count; i++)
        {
            racerItems[i].SetPlace(carPositions[i].place+ 1, carPositions[i].nameRacer);
        }
       
     
    }
    public void ShowPositionOnFinishTable(List<CarPosition> carPositions)
    {
        if (IsFInish == true)
        {
            for (int i = 0; i < racerItems.Count; i++)
            {
                finalTableItem[i].SetPlace(carPositions[i].place + 1, carPositions[i].nameRacer);
            }
            IsFInish = false;
        }
    }
    public void Init(CarControllerPro carPl)
    {
        racerItems = new List<TableRacerItem>();
        foreach (var car in cars)
        {

            var item = Instantiate(prefabRacerItem, content);
            item.Init(car);
            racerItems.Add(item);
           

        }

        var carPlayer = Instantiate(prefabRacerItem, content);
        cars.Add(carPl);
        carPlayer.Init(carPl);
        racerItems.Add(carPlayer);
        playerInstance = carPl;
        checkPoints.Init();


        int i = 0;
        foreach (var car in cars)
        {

            finalTableItem[i].Init(car);
            i++;

        }

    }
}
