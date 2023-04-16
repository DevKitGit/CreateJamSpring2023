using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.iOS;

public class LootHandlerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lootCounterText;

    [SerializeField] private AudioSource _getLooties;
    [SerializeField] private AudioClip lootEnter;
    
    
    [SerializeField] private SphereCollider loots;
    [SerializeField] private List<SphereCollider> purchasables;
    [SerializeField] private float movespeed = 1f;
    [SerializeField] private Transform boatTransform;
    private bool _ableToBuy = true;
    
    [SerializeField] private List<GameObject> _boughtItems;
    
    
    
    public List<Transform> orgTransform;

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
    

    private void OnTriggerEnter(Collider other)
    {
        var cost = 0;
        if (other.GetComponent<SphereCollider>() == loots.GetComponent<SphereCollider>())
        {
            _lootCounter += 1;
            lootCounterText.text = _lootCounter.ToString();
            lootCounterText.ForceMeshUpdate(true);
            _getLooties.PlayOneShot(lootEnter);
            Destroy(loots.gameObject);
        }

        foreach (SphereCollider purchasable in purchasables)
        {
            if (other.GetComponent<SphereCollider>() == purchasable.GetComponent<SphereCollider>())
            {
                switch (purchasable.name)
                {
                    case "Item1":
                        cost = 3;
                        _currentPurchaseAttempt = 0;
                        if (cost <= _lootCounter && _ableToBuy)
                        {
                            _lootCounter -= cost;
                            lootCounterText.text = _lootCounter.ToString();
                            lootCounterText.ForceMeshUpdate(true);
                        }
                        else
                        {
                            _ableToBuy = false;
                        }
                        break;
                    case "Item2":
                        cost = 1;
                        _currentPurchaseAttempt = 1;
                        if (cost <= _lootCounter && _ableToBuy)
                        {
                            _lootCounter -= cost;
                            lootCounterText.text = _lootCounter.ToString();
                            lootCounterText.ForceMeshUpdate(true);
                        }
                        else
                        {
                            _ableToBuy = false;
                            
                        }
                        break;
                    case "Item3":
                        cost = 2;
                        _currentPurchaseAttempt = 2;
                        if (cost <= _lootCounter && _ableToBuy)
                        {
                            _lootCounter -= cost;
                            lootCounterText.text = _lootCounter.ToString();
                            lootCounterText.ForceMeshUpdate(true);
                        }
                        else
                        {
                            _ableToBuy = false;
                        }
                        break;
                }
            }
        }
    }
}
