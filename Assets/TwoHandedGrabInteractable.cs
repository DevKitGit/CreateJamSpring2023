using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandedGrabInteractable : XRGrabInteractable
{
    [SerializeField] private InteractionLayerMask rightHandedLayer;
    [SerializeField] private InteractionLayerMask leftHandedLayer;
    [SerializeField] private float maxGuardDistance;
    private int _previousLayer;
    private Launcher _launcher;

    private void Start()
    {
        _launcher = GetComponentInChildren<Launcher>(true);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (!isSelected)
        {
            return interactor.interactionLayers == rightHandedLayer && base.IsSelectableBy(interactor);
        }
        
        if (isSelected)
        {
            if (interactor.interactionLayers == leftHandedLayer)
            {
                if (!interactor.hasSelection)
                {
                    Vector3 attachtrans = interactor.GetAttachTransform(this).position;
                
                    if (Vector3.Distance(attachtrans, attachTransform.position) > Vector3.Distance(attachtrans, secondaryAttachTransform.position))
                    {
                        return base.IsSelectableBy(interactor);
                    }
                    return false;
                }
                if (interactorsSelecting.Count == 1)
                {
                    return false;
                }
                Vector3 interactorPos = interactor.GetAttachTransform(this).position;
                if (Vector3.Distance(interactorPos, secondaryAttachTransform.position) > maxGuardDistance)
                {
                    return false;
                }
            }
        }
        return base.IsSelectableBy(interactor);
    }
    
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        
        if (gameObject.layer != LayerMask.NameToLayer("Gun"))
        {
            _previousLayer = gameObject.layer;
        }
        gameObject.SetLayerRecursively(LayerMask.NameToLayer("Gun"));
        base.OnSelectEntering(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (!args.interactableObject.isSelected)
        {
            gameObject.SetLayerRecursively(_previousLayer);
        }
        base.OnSelectExited(args);
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        if (args.interactorObject.interactionLayers == leftHandedLayer)
        {
            return;
        }
        _launcher.Fire();
        base.OnActivated(args);
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        if (args.interactorObject.interactionLayers == leftHandedLayer)
        {
            return;
        }
        _launcher.Detach();
        base.OnDeactivated(args);
    }

    private void OnDrawGizmosSelected()
    {
        if (secondaryAttachTransform != null)
        {
            Gizmos.DrawWireSphere(secondaryAttachTransform.position,maxGuardDistance);
        }
    }


}
