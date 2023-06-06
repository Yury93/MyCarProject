using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFlag : MonoBehaviour
{
    [Serializable]
    public class FlagStart
    {
        public ParticleSystem particle1, particle2;

        public void SetActiveFlag(bool flag)
        {
            particle1.gameObject.SetActive(flag);
            particle2.gameObject.SetActive(flag);
        }
    }
    public List<FlagStart> flagStartList;
    public int currentActiveFlag;
    public bool isFinish;
    public void Active(bool flag)
    {
        if (flag == true && currentActiveFlag < flagStartList.Count) 
        {
            flagStartList[currentActiveFlag].SetActiveFlag(flag);
            currentActiveFlag++;
        }
        else if (flag == false && currentActiveFlag > 0)
        {
            flagStartList[currentActiveFlag].SetActiveFlag(flag);
            currentActiveFlag--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var car = other.gameObject.GetComponentInParent<CarControllerPro>();
        if (car != null) 
        {
        flagStartList.ForEach(c=>c.SetActiveFlag(true));
        }
    }

}
