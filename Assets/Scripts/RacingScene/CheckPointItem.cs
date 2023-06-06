using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointItem : MonoBehaviour
{

    [SerializeField] private  Cinemachine.CinemachineVirtualCamera virtualCamera;
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
        if (car != null)
        {
            var index = TableRacers.instance.CheckPoints.CarsGo.IndexOf(car.gameObject);
            TableRacers.instance.CheckPoints.CarPositions[index].SetNumberCheckPoint( nCheckpointNumber + 1);
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
            TableRacers.instance.CheckPoints.CarsGo.ForEach(c => 
            {
              var car =  c.GetComponentInParent<CarControllerPro>();
              car.maxSpeed = 0;

                
                GameManager.instance.hBrake = true;

            });
           
              yield return new WaitForSeconds(30f);
          
            SceneManager.LoadScene(Loading.sceneName);
        }
    }
}
