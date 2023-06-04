using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    [SerializeField] private float speedRotateDetails;
    [SerializeField] private float speedMove;
    [SerializeField] private CarControllerPro player;
    [SerializeField] private GameObject light,rotateObj1,rotateObj2,helicopterGo;
    [SerializeField] private float lerpSpeed;
    public void Init(CarControllerPro car)
    {
        player = GameManager.instance.player;
    }
    private void Update()
    {

        if(player == null)
        player = GameManager.instance.player;

        rotateObj1.transform.Rotate(0, speedRotateDetails * Time.deltaTime, 0);
        rotateObj2.transform.Rotate(speedRotateDetails * Time.deltaTime, 0, 0);
        
        Vector3 myPos = new Vector3(player.transform.position.x + 1, player.transform.position.y + 12f, player.transform.position.z +2f);
        transform.position = Vector3.Lerp(transform.position,myPos,lerpSpeed * Time.deltaTime);
        helicopterGo.transform.LookAt(player.transform);
       
    }
}
