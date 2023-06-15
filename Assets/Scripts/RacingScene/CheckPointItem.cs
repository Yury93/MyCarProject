using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPointItem : MonoBehaviour
{

    [SerializeField] private  Cinemachine.CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject canvasFinal;
    [SerializeField] private Button continueButton;
    Cinemachine.CinemachineOrbitalTransposer transposer;
    public float nDistanceX, nDistanceY, nDistanceZ;
    public List<float> nDistChk = new List<float>();
    public int nCheckpointNumber;
    public bool isFinish;
    private void Start()
    {
        nDistChk.Clear();

        for (int i = 0; i <= TableRacers.instance.cars.Count; i++)
        {
            nDistChk.Add(0);
        }
        if(continueButton != null) 
        continueButton.onClick.AddListener(() => SceneManager.LoadScene(Loading.sceneName));
    }

    void Update()
    {
        for (int i = 0; i < TableRacers.instance.cars.Count; i++)
        {
            //чекпоинты которые поидеи надо обновлять           //номер чекпоинта 
            if (TableRacers.instance.CheckPoints.CarPositions[i].currentNumberCheckpoint == nCheckpointNumber)
            {
                nDistanceZ = this.GetComponent<Transform>().position.z - TableRacers.instance.CheckPoints.CarsGo[i].GetComponent<Transform>().position.z;
                nDistanceY = this.GetComponent<Transform>().position.y - TableRacers.instance.CheckPoints.CarsGo[i].GetComponent<Transform>().position.y;
                nDistanceX = this.GetComponent<Transform>().position.x - TableRacers.instance.CheckPoints.CarsGo[i].GetComponent<Transform>().position.x;
                nDistChk[i] = (float)(Math.Sqrt(Math.Pow(nDistanceX, 2) + Math.Pow(nDistanceY, 2) + Math.Pow(nDistanceZ, 2)));
                TableRacers.instance.CheckPoints.CarPositions[i].SetDistanceToPosition(nDistChk[i]);

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponentInParent<CarControllerPro>();
        if (car != null && car.fake == false)
        {
            var carIndex = TableRacers.instance.CheckPoints.CarsGo.IndexOf(car.gameObject);


         int currentPoint =  TableRacers.instance.CheckPoints.CarPositions[carIndex].GetNumberCheckPoint();

            if(car.IsAI == false)
            Debug.Log($"{nCheckpointNumber} == {currentPoint}  будущие точки");

            if (nCheckpointNumber == currentPoint || nCheckpointNumber == currentPoint+1)
          TableRacers.instance.CheckPoints.CarPositions[carIndex].SetNumberCheckPoint( nCheckpointNumber + 1);
       
        }
        if(isFinish && car.IsAI == false && nDistanceZ != 0)
        {
            ApplyFinish();
        }
    }

    private void ApplyFinish()
    {
        Debug.Log("finish!!!!!!!!!!!!!");
        
        Loading.sceneName = "SelectSeasons";
        StartCoroutine(CorLoadScene());
        IEnumerator CorLoadScene()
        {
            virtualCamera.m_Follow = GameManager.instance.player.transform;
            virtualCamera.LookAt = GameManager.instance.player.transform;
           
            virtualCamera.gameObject.SetActive(true);
            if(transposer != null) 
            transposer.gameObject.SetActive(true);

            GameManager.instance.virtualCamera.gameObject.SetActive(false);
            GameManager.instance.transposer.gameObject.SetActive(false);
           if(canvasFinal != null)
            canvasFinal.SetActive(true);
            TableRacers.IsFInish = true;
          
            SeasonRacing.CurrentSeason = SeasonRacing.CurrentSeason + 1; 

            TableRacers.instance.CheckPoints.CarsGo.ForEach(c => 
            {
              var car =  c.GetComponentInParent<CarControllerPro>();
              car.maxSpeed = 0;

                
                GameManager.instance.hBrake = true;

            });

            //var place = TableRacers.instance.GetPlayerByTable().Place;
            //Debug.Log(place + " место которое занял игрок ");


            //Social1.PlayerPrefs.SetInt("Season" + GameManager.instance.SeasonIndex, place);
        
        //while(TableRacers.instance.GetPlayerByTable().Place == 0)
        //  {
        //      yield return new WaitForSeconds(0.01f);
        //  }

        yield return new WaitForSeconds(30f);
          
            SceneManager.LoadScene(Loading.sceneName);
        }
    }
}
