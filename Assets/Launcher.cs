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

    private Stopwatch _stopwatch;
    private Harpoon _spawnedHarpoon;
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
    }
    
    public void Fire()
    {
        if (_spawnedHarpoon == null || _spawnedHarpoon.fired)
        {
            return;
        }
        _spawnedHarpoon.Fire(transform.forward);
        _stopwatch = Stopwatch.StartNew();
    }
    
    public void Detach()
    {
        if (_spawnedHarpoon == null || !_spawnedHarpoon.fired || _spawnedHarpoon.detached)
        {
            return;
        }        
        _spawnedHarpoon.Detach(transform.forward);
        Invoke(nameof(SpawnHarpoon),reloadSpeed);
    }
    
    void Update()
    {
        if (_spawnedHarpoon is { fired: true, detached: false })
        {
            if (_stopwatch.ElapsedMilliseconds > fallbackTimeoutSec * 1000)
            {
                Detach();
                return;
            }
            PullBoatCloser();
        }
    }

    private void PullBoatCloser()
    {
        
    }
    
    public void HarpoonHitTarget(Harpoon.HitType hitType, GameObject hitTarget)
    {
        
    }
}
