using UnityEngine.XR.Interaction.Toolkit;

public class HandHold : XRBaseInteractable
{
    protected Weapon weapon = null;
    
    public void Setup(Weapon weapon)
    {
        this.weapon = weapon;
    }

    protected override void Awake()
    {
        base.Awake();
        selectEntered.AddListener(Grab);
        selectExited.AddListener(Drop);
    }

    private void OnDestroy()
    {
        selectEntered.RemoveListener(Grab);
        selectExited.RemoveListener(Drop);
    }

    protected virtual void BeginAction(IXRSelectInteractor interactor)
    {
        // Empty
    }

    protected virtual void EndAction(IXRSelectInteractor interactor)
    {
        // Empty
    }

    protected virtual void Grab(SelectEnterEventArgs args)
    {
        TryToHideHand(args.interactorObject,false);
    }

    protected virtual void Drop(SelectExitEventArgs args)
    {
        TryToHideHand(args.interactorObject,true);
    }

    private void TryToHideHand(IXRSelectInteractor interactor, bool hide)
    {
        if (interactor is not Hand hand) return;
        hand.SetVisibility(hide);
    }

    public void BreakHold(IXRSelectInteractor interactor)
    {

    }
}
