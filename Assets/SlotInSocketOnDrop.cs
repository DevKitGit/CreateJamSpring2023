using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlotInSocketOnDrop : MonoBehaviour
{
    private XRSocketInteractor _socketInteractor;
    private void Start()
    {
        _socketInteractor = FindObjectOfType<XRSocketInteractor>();
    }

    public void Slot(SelectExitEventArgs args)
    {
        if (args.interactorObject is not XRSocketInteractor)
        {
            args.manager.SelectEnter(_socketInteractor, args.interactableObject);

        }
    }
}
