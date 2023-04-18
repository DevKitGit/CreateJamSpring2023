using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TransformSetters : MonoBehaviour
{
    public void SetLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }
    public void SetLocalRotation(Vector3 rot)
    {
        transform.localRotation = Quaternion.Euler(rot);
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetRotation(Vector3 rot)
    {
        transform.rotation = Quaternion.Euler(rot);
    }

    public void ResetLocalPositionAndRotation()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);        
    }
    public void ResetPositionAndRotation()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void SetLeftHandLocal()
    {
        transform.localPosition = new Vector3(-0.01f,0.02f, 0.01f);
        transform.localRotation = Quaternion.Euler(new Vector3(256.9f, 167.8f,10.7f));
        transform.localScale = Vector3.one* 0.7f;
    }

    public void SetRightHandLocal()
    {
        transform.localPosition = new Vector3(-0.01f, 0.02f, 0.01f);
        transform.localRotation = Quaternion.Euler(new Vector3(85.97f, -3.8f,5.66f));
        transform.localScale = Vector3.one * 0.7f;
    }
    public void ResetLocalScale()
    {
        transform.localScale = Vector3.one;
    }
}
