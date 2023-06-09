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
    [SerializeField] private AudioSource _audiosource;
    [SerializeField] private SphereCollider loots;
    [SerializeField] private List<GameObject> purchasables;
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
        if (item != null && item.hasBeenHit && !item.hasBeenBought)
        {
            _ableToBuy = item.value >= 0 || -item.value <= _lootCounter;
            if (!_ableToBuy)
            {
                item.ReturnItem();
                item.hasBeenHit = false;
                item.hasBeenBought = false;
                return;
            }
            if (item.gameObject.name == "Rum")
            {
                PlayRandomRumSound();
            }
            else
            {
                PlayRandomCoinsSound();
            }

            item.hasBeenBought = true;
            item.hasBeenHit = true;
            _lootCounter += item.value;
            lootCounterText.SetText(_lootCounter.ToString());
            lootCounterText.ForceMeshUpdate(true);
            item.OnItemReceived.Invoke();
            Transform topmostParent = item.transform;
            while (topmostParent.parent != null)
            {
                topmostParent = topmostParent.parent;
            }
            var harpoons = topmostParent.transform.GetComponentsInChildren<Harpoon>();
            for (var index = 0; index < harpoons.Length; index++)
            {
                var harpoon = harpoons[index];
                harpoon.DestroyHarpoon();
            }
            if (item.destroyOnReceive)
            {
                Destroy(topmostParent.gameObject);
            }
        }
    }
}
