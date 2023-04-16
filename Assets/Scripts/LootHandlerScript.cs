using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LootHandlerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lootCounterText;
    [FormerlySerializedAs("_getLooties")] [SerializeField] private AudioSource _audiosource;
    [SerializeField] private SphereCollider loots;
    [SerializeField] private List<SphereCollider> purchasables;
    [SerializeField] private float movespeed = 1f;
    [SerializeField] private Transform boatTransform;
    [SerializeField] private List<GameObject> _boughtItems;
    [FormerlySerializedAs("_onItemGet")] [SerializeField] private List<AudioClip> _coinSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _onRumBuy = new List<AudioClip>();
    public List<Transform> orgTransform;

    private bool _ableToBuy = true;
    private int _currentPurchaseAttempt;
    private int _lootCounter = 3;

    public void Start()
    {
        lootCounterText.text = _lootCounter.ToString();
        lootCounterText.ForceMeshUpdate(true);
    }

    public void Update()
    {
        if (_ableToBuy)
        {
            purchasables[_currentPurchaseAttempt].transform.position = Vector3.MoveTowards(purchasables[_currentPurchaseAttempt].transform.position,
                boatTransform.position, movespeed*Time.deltaTime);
            if (Vector3.Distance(purchasables[_currentPurchaseAttempt].transform.position, boatTransform.position) <= 0)
            {
                _boughtItems.Add(purchasables[_currentPurchaseAttempt].gameObject);
                purchasables.RemoveAt(_currentPurchaseAttempt);
            }
        }
        if (!_ableToBuy && Vector3.Distance(purchasables[_currentPurchaseAttempt].transform.position, boatTransform.position) <= 0)
        {
            purchasables[_currentPurchaseAttempt].transform.position = Vector3.MoveTowards(purchasables[_currentPurchaseAttempt].transform.position,
                orgTransform[_currentPurchaseAttempt].transform.position, movespeed*Time.deltaTime);
        }
    }
    


    private void PlayRandomRumSound()
    {
        _audiosource.PlayOneShot(_onRumBuy[Random.Range(0,_onRumBuy.Count)]);
    }
    
    private void PlayRandomCoinsSound()
    {
        _audiosource.PlayOneShot(_coinSounds[Random.Range(0,_coinSounds.Count)]);
    }
    private void OnTriggerEnter(Collider other)
    {
        var cost = 0;
        Item item = other.GetComponentInChildren<Item>();
        if (item != null)
        {
            _lootCounter += item.value;
            lootCounterText.SetText(_lootCounter.ToString());
            lootCounterText.ForceMeshUpdate(true);
            item.OnItemReceived.Invoke();
            Transform topmostParent = item.transform;
            while (topmostParent.parent != null)
            {
                topmostParent = topmostParent.parent;
            }
            Destroy(topmostParent.gameObject);
        }
    }
}
