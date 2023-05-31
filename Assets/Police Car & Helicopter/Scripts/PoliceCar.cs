using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCar : MonoBehaviour
{
    [SerializeField] private Renderer lightRed, LightBlue;
    public Vector3 startPosition;
    public Quaternion startRotation;
    private AiCarController aiCarController;
    public bool isDebug;
    private void Awake()
    {
        aiCarController = GetComponent<AiCarController>();
    }
    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        StartCoroutine(CorActive());

    }

    private void FixedUpdate()
    {
        
        var distance =Vector3.Distance (GameManager.instance.player.transform.position , transform.position);
         if(isDebug) { Debug.Log(distance); }
        if(distance > 100f)
        {
            transform.position =  new Vector3( startPosition.x,startPosition.y,startPosition.z);
            transform.rotation = startRotation;
            gameObject.SetActive(false);
            if (isDebug) { Debug.Log("объект выключен"); }
        }
    }
    IEnumerator CorActive()
    {
        //startPosition = transform; 
        while (true)
        {
            
            lightRed.material.DisableKeyword("_EMISSION");
            LightBlue.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.3F);
            lightRed.material.EnableKeyword("_EMISSION");
            LightBlue.material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.3F);
        }
    }
}
