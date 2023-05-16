using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CarPosition
{
    public string nameRacer;
    public int currentNumberCheckpoint;
    public float distToPosition;
    public CarPosition(string name)
    {
        this.nameRacer = name;
    }
    public void SetNumberCheckPoint(int checkPoint)
    {
        currentNumberCheckpoint = checkPoint;
    }
    public void SetDistanceToPosition(float distance)
    {
        distToPosition = distance;
    }
}
public class CheckPoints : MonoBehaviour
{
    [SerializeField] private List<CheckPointItem> checkPoints;
    [SerializeField] private List<GameObject> carsGo;
    private List<CarPosition> carPositions;
    public List<CarPosition> CarPositions => carPositions;
    public List<GameObject> CarsGo => carsGo;

    [Header("LeaderBoard")]
    [SerializeField] private List<CarPosition> leaderBoardData;
    public List<CarPosition> LeaderBoardData => leaderBoardData;

    public void Init()
    {
        leaderBoardData = new List<CarPosition>();
        carPositions = new List<CarPosition>();
        carsGo = new List<GameObject>();

        for (int i = 0; i < TableRacers.instance.cars.Count; i++) 
        {
            carsGo.Add(TableRacers.instance.cars[i].gameObject);
            CarPositions.Add(new CarPosition(TableRacers.instance.cars[i].name));
          
        }
    }
    public void Update()
    {
        leaderBoardData = CarPositions.OrderBy(c=>c.distToPosition).OrderByDescending(c => c.currentNumberCheckpoint).ToList();
    }
}


