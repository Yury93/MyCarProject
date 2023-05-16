using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRacers : MonoBehaviour
{
    [SerializeField] public List<CarControllerPro> cars;
    [SerializeField] private TableRacerItem prefabRacerItem;
    [SerializeField] private Transform content;
    [SerializeField] private CheckPoints checkPoints;
    public List<CarControllerPro> Cars => cars;
    public CheckPoints CheckPoints => checkPoints;
    public List<TableRacerItem> racerItems;
    public static TableRacers instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        racerItems = new List<TableRacerItem>();
        foreach (var car in cars)
        {
            var item = Instantiate(prefabRacerItem, content);
            item.Init(car);
            racerItems.Add(item);
        }
        checkPoints.Init();
    }
    private void Update()
    {
  
    }
    public void CreateTableItemByPlayer(CarControllerPro car)
    {
        var item = Instantiate(prefabRacerItem, content);
        cars.Add(car);
        item.Init(car);
        racerItems.Add(item);
    }
}
