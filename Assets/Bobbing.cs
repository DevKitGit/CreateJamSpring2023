using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public bool shouldBob;
    public float waterHeight;
    public float bobAmount;
    
    void Update()
    {
        if (!shouldBob) return;
        var position = transform.position;
        transform.position = new Vector3(position.x, waterHeight + bobAmount * math.sin(Time.time), position.z);
    }
}
