using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainCollision : MonoBehaviour
{
   
    private void OnCollisionEnter(Collision collision)
    {
            var player = collision.gameObject.GetComponent<CarControllerPro>();
            if (player != null && player.IsAI == false)
            {

                Debug.Log("—ъехал с трассы");
            CanvasInfo.instance.SetInfoText("—ъехал с трассы");

        }
        
    }
  
}
