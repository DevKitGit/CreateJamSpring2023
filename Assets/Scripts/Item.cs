using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    public UnityEvent OnItemReceived;
    public int value = 1;
    public bool destroyOnReceive;
    private Vector3 startPos;
    private Transform _Starttransform;
    public bool hasBeenHit;
    public bool hasBeenBought;
    private void Start()
    {
        _Starttransform = transform;
        startPos = transform.position;
    }

    public void ReturnItem()
    {
        transform.parent = _Starttransform;
        transform.position = startPos;
    }
}
