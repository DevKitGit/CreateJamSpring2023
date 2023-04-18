using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Harpoon : MonoBehaviour
{
    [SerializeField, Range(0.1f,100f)] private float forceMultiplier = 1f;
    [SerializeField, Range(0.01f,100f)] private float torqueMultiplier = 1f;

    [SerializeField] private Rigidbody ropeBase;
    [SerializeField] private Rigidbody head;
    [SerializeField] private Transform headTransform;

    public Transform GetHead => headTransform;
    private LineRenderer _lineRenderer;
    public enum HitType
    {
        Untagged = 0,
        ItemThatPulls,
        ItemThatIsPulled,
        SecurityPlane,
        ShopItem
    }

    private Launcher _launcher;
    [ReadOnly] public bool detached;
    [ReadOnly] public bool fired;
    [ReadOnly] public bool hasHitOnce;
    private bool _passive;
    private Transform _launcherSpawn;
    private Vector3 hitOffset;
    private GameObject currentTarget;

    // Start is called before the first frame update
    private void Start()
    {
        _launcher = FindObjectOfType<Launcher>();
        _launcherSpawn = _launcher.SpawnAnchor;
        _lineRenderer = GetComponent<LineRenderer>();
        var position = _launcherSpawn.position;
        
        _lineRenderer.SetPositions(new []{position,position});
        _lineRenderer.enabled = false;
        SetRigidbodyInteractive(head, false);
        SetRigidbodyInteractive(ropeBase, false);
        
    }

    private void SetRigidbodyInteractive(Rigidbody rb, bool active)
    {
        rb.detectCollisions = active;
        rb.isKinematic = !active;
    }
    private void Update()
    {
        if (_passive) { return; }
        if (!fired)
        {
            head.transform.SetPositionAndRotation(_launcherSpawn.position, _launcherSpawn.rotation);
        }
        if (currentTarget != null)
        {
            head.transform.localPosition = hitOffset;
        }
        if (!detached)
        {
            ropeBase.transform.SetPositionAndRotation(_launcherSpawn.position, _launcherSpawn.rotation);
        }
        _lineRenderer.SetPosition(1, headTransform.position);
        _lineRenderer.SetPosition(0, ropeBase.position);
    }

    public void Fire(Vector3 direction)
    {
        fired = true;
        SetRigidbodyInteractive(head, true);
        head.AddForce(direction * forceMultiplier, ForceMode.Impulse);
        _lineRenderer.enabled = true;
    }
    public void Detach(Vector3 direction)
    {
        detached = true;
        SetRigidbodyInteractive(ropeBase,true);
        ropeBase.AddForce(direction * (forceMultiplier * 0.05f), ForceMode.Impulse);
        ropeBase.AddTorque(new Vector3(1,0,0) * (torqueMultiplier * 0.01f), ForceMode.Impulse);
        currentTarget = null;
        Invoke(nameof(MakePassive),4f);
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (hasHitOnce) { return; }
        if (!fired) {return;}
        GameObject target = other.gameObject;
        var wasGunHit = target.CompareTag("Gun");
        if (wasGunHit) { return; }
        hasHitOnce = true;
        head.isKinematic = true;
        head.constraints = RigidbodyConstraints.FreezeAll;
        Transform topmostParent = target.transform;
        while (topmostParent.parent != null)
        {
            topmostParent = topmostParent.parent;
        }
        head.transform.parent = topmostParent;
        var list = topmostParent.GetComponentsInChildren<Item>();
        foreach (var item in list)
        {
            item.hasBeenHit = true;
        }
        hitOffset = target.transform.InverseTransformPoint(head.transform.position);

        if(Enum.TryParse(target.tag, true, out HitType hitType));
        _launcher.HarpoonHitTarget(hitType, target);
        currentTarget = target;
    }
    private void MakePassive()
    {
        _passive = true;
        SetRigidbodyInteractive(head,false);
        SetRigidbodyInteractive(ropeBase,false);
        enabled = false;
    }

    public void DestroyHarpoon()
    {
        
        _launcher.Detach();
        Destroy(ropeBase.gameObject);
        Destroy(head.gameObject);
    }
}
