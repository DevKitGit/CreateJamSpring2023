using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Launcher : MonoBehaviour
{
    [SerializeField,Min(0.5f)] private float reloadSpeed = 0.5f;
    [SerializeField] private float fallbackTimeoutSec = 1f;
    [SerializeField] private GameObject harpoonPrefab;
    [SerializeField] private Transform spawnAnchor;
    [SerializeField] private Transform boat;
    [SerializeField] private float boatMoveSpeed;
    [SerializeField] private float itemPullMoveSpeed;
    [SerializeField] private float minimumAttachDistance;
    [SerializeField] private float minimumPullAttachDistance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip onFireOne, onFireTwo, onReload, detatch, pulling;
    
    
    private Stopwatch _stopwatch;
    private Harpoon _spawnedHarpoon;
    private Harpoon.HitType currentHitType;
    private GameObject currentHitTarget;
    public Transform SpawnAnchor => spawnAnchor;
    void Start()
    {
        SpawnHarpoon();
    }

    private void SpawnHarpoon()
    {
        var spawned = Instantiate(harpoonPrefab, spawnAnchor.position, spawnAnchor.rotation);
        spawned.transform.parent = null;
        _spawnedHarpoon = spawned.GetComponentInChildren<Harpoon>();
        _audioSource.PlayOneShot(onReload);
    }
    
    public void Fire()
    {
        if (_spawnedHarpoon == null || _spawnedHarpoon.fired)
        {
            return;
        }
        _spawnedHarpoon.Fire(transform.forward);
        _stopwatch = Stopwatch.StartNew();
        //play sound here
        _audioSource.PlayOneShot(onFireOne);
        Invoke(nameof(PlayMe), .1f);
    }

    private void PlayMe()
    {
        _audioSource.PlayOneShot(onFireTwo);
    }
    public void Detach()
    {
        if (_spawnedHarpoon == null || !_spawnedHarpoon.fired || _spawnedHarpoon.detached)
        {
            return;
        }        
        _spawnedHarpoon.Detach(transform.forward);
        _spawnedHarpoon = null;
        _audioSource.loop = false;
        _audioSource.Stop();
        Invoke(nameof(SpawnHarpoon),reloadSpeed);
        _audioSource.PlayOneShot(detatch);
    }
    
    void Update()
    {
        if (_spawnedHarpoon is { fired: true, detached:false,hasHitOnce:true})
        {
            Vector3 target;
            Vector3 origin;
            switch (currentHitType)
            {
                case Harpoon.HitType.None:
                case Harpoon.HitType.SecurityBorder:
                    break;
                case Harpoon.HitType.ItemThatPulls:
                    target = currentHitTarget.transform.position;
                    origin = boat.position;
                    target.y = origin.y;
                    if (CheckIfTooCloseToAttached(origin, target,minimumAttachDistance))
                    {
                        Detach();
                    }
                    boat.position = Vector3.MoveTowards(origin, target, boatMoveSpeed * Time.deltaTime);
                    break;
                case Harpoon.HitType.ItemThatIsPulled:
                case Harpoon.HitType.ShopItem:
                    target = boat.position;
                    origin = currentHitTarget.transform.position;
                    target.y = origin.y;
                    if (CheckIfTooCloseToAttached(origin, target,minimumPullAttachDistance))
                    {
                        Detach();
                    }
                    currentHitTarget.transform.position = Vector3.MoveTowards(origin, target, itemPullMoveSpeed * Time.deltaTime);
                    break;
            }
            return;
        }
        if (_spawnedHarpoon is not { fired: true, detached: false }) { return; }
        if (_stopwatch.ElapsedMilliseconds < fallbackTimeoutSec * 1000) { return; }
        Detach();
    }
    
    private bool CheckIfTooCloseToAttached(Vector3 origin, Vector3 target, float distance)
    {
        return Vector3.Distance(origin, target) < distance;
    }
    public void HarpoonHitTarget(Harpoon.HitType hitType, GameObject hitTarget)
    {
        if (hitType is Harpoon.HitType.None or Harpoon.HitType.SecurityBorder)
        {
            Detach();
            currentHitTarget = null;
            currentHitType = Harpoon.HitType.None;
            return;
        }
        
        currentHitType = hitType;
        currentHitTarget = hitTarget;
        _audioSource.loop = true;
        _audioSource.PlayOneShot(pulling);
    }
}