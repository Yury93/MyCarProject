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
    public int place;
    public CarControllerPro Car;
    public CarPosition(string name,CarControllerPro car)
    {
        this.nameRacer = name;
        Car = car;  
    }
    public void SetNumberCheckPoint(int checkPoint)
    {
        currentNumberCheckpoint = checkPoint;
        //if(Car.isAI == false)
        //Debug.Log(currentNumberCheckpoint + " прибавляемый индекс car name: " + Car.gameObject.name);
        ///проблема в том, что машине назначается индекс точки, который больше, чем пологается
        ///точка должна назначаться по плюс одной, когда игрок роходит эти соответствующие точки
    }
    public int GetNumberCheckPoint() { return currentNumberCheckpoint; }
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
    private List<CarPosition> deltaLeaderBoardData;

    public void Init()
    {
        leaderBoardData = new List<CarPosition>();
        carPositions = new List<CarPosition>();
        carsGo = new List<GameObject>();
       

        for (int i = 0; i < TableRacers.instance.cars.Count; i++) 
        {
            carsGo.Add(TableRacers.instance.cars[i].gameObject);
            CarPositions.Add(new CarPosition(TableRacers.instance.cars[i].name, TableRacers.instance.cars[i]));
          
        }

        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].nCheckpointNumber = i;
        }
        deltaLeaderBoardData = carPositions;
    }
    public void Update()
    {
        leaderBoardData = CarPositions.OrderBy(c => c.distToPosition).OrderByDescending(c => c.currentNumberCheckpoint).ToList();
        for (int i = 0; i < leaderBoardData.Count; i++)
        {
            leaderBoardData[i].place = i;

        }
        for (int i = 0; i < leaderBoardData.Count; i++)
        {
            if (leaderBoardData[i].place != deltaLeaderBoardData[i].place)
            {
                TableRacers.instance.ShowPositionOnLeaderBoard(leaderBoardData);
                deltaLeaderBoardData = leaderBoardData;
                break;
            }
           
        }
        TableRacers.instance.ShowPositionOnFinishTable(leaderBoardData);


    }
}


