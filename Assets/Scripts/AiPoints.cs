using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPoints : MonoBehaviour
{
    public CarControllerPro controllerPro;
    public List<Point> points;


    [ContextMenu("SetMaxDrag")]
    public void SetMaxDrag()
    {
        foreach (var item in points)
        {
            item.Drag = controllerPro.GetComponent<Rigidbody>().drag;
        }
    }
}
