using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndType { detainedByPolice, offTrack, Finish,none }
public class EndPoliceScene 
{
    public static EndPoliceScene instance;
    public Action<EndType> onEndPoliceScene;
    public EndType end = EndType.none;
    public EndPoliceScene()
    {
        instance = this;

    }
    public void SetTypeEnd(EndType endType)
    {
        if (end == EndType.none)
        {
            onEndPoliceScene?.Invoke(endType);
            end = endType;
        }
    }
}
