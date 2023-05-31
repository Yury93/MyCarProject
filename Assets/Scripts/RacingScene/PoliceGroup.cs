using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceGroup : MonoBehaviour
{
    [SerializeField] private List<CarControllerPro> cars;

 
    private void OnTriggerEnter(Collider other)
    {
       
            StartCoroutine(CorDelay());
            IEnumerator CorDelay()
            {
                foreach (CarControllerPro car in cars)
                {
                    car.gameObject.SetActive(true);
                    yield return new WaitForEndOfFrame();
                }
            }
        

    }
}
