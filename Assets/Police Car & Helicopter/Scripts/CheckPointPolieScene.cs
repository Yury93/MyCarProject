using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointPolieScene : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
    //    var player = other.gameObject.GetComponent<CarControllerPro>();
    //    if (player != null && player.IsAI == false)
    //    {

    //        Debug.Log("Финиш");
    //        CanvasInfo.instance.SetInfoText("Финиш");

    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject;
        if (player )
        {

            Debug.Log("Финиш");
            CanvasInfo.instance.SetInfoText("Финиш");

        }
    }
}
