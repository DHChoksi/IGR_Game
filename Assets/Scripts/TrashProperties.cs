using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Constants;

[Serializable]
public class TrashData 
{
    public TrashProperties[] TrashProperties;
}

[Serializable]
public class TrashProperties
{
    
    public Trash _Trash 
    {
        get { return TrashScript; }
        set { value = TrashScript; }
    }

    [SerializeField]
    private Trash TrashScript;

    public TrashType _TrashType
    {
        get { return TrashType; }
        set { value = TrashType; }
    }

    [SerializeField]
    private TrashType TrashType;

    public float _TrashDamage
    {
        get { return TrashDamage; }
        set { value = TrashDamage; }
    }

    [SerializeField]
    private float TrashDamage = 1f;

 }
