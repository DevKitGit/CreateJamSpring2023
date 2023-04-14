using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : XRGrabInteractable
{
    private GripHold _gripHold = null;
    private IXRSelectInteractor _gripHand = null;
    private readonly Vector3 gripRotation = new Vector3(45, 0, 0);
    protected override void Awake()
    {
        base.Awake();
        SetupHolds();
        selectEntered.AddListener(SetInitialRotation);
    }

    private void SetupHolds()
    {
        _gripHold = GetComponentInChildren<GripHold>();
        _gripHold.Setup(this);
    }

    private void SetupExtras()
    {
        
    }

    private void OnDestroy()
    {
        selectEntered.RemoveListener(SetInitialRotation);
    }

    private void SetInitialRotation(SelectEnterEventArgs args)
    {
        Quaternion newRotation = Quaternion.Euler(gripRotation);
        args.interactorObject.GetAttachTransform(this).localRotation = newRotation;
    }

    public void SetGripHand(IXRSelectInteractor interactor)
    {
        _gripHand = interactor;
        OnSelectEntered(new SelectEnterEventArgs
        {
            interactorObject = interactor,
            interactableObject = this,
            manager = interactionManager
        });
    }

    public void ClearGripHand(IXRSelectInteractor interactor)
    {
        _gripHand = null;
        OnSelectExited(new SelectExitEventArgs
        {
            interactorObject = interactor,
            interactableObject = this,
            manager = interactionManager
        });
    }

    public void SetGuardHand(IXRSelectInteractor interactor)
    {

    }

    public void ClearGuardHand(IXRSelectInteractor interactor)
    {

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
    }

    private void SetGripRotation()
    {

    }

    private void CheckDistance(IXRSelectInteractor interactor, HandHold handHold)
    {

    }

    public void PullTrigger()
    {

    }

    public void ReleaseTrigger()
    {

    }

    public void ApplyRecoil()
    {

    }
}
