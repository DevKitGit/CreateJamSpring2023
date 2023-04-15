using UnityEngine.XR.Interaction.Toolkit;

public class GripHold : HandHold
{
    protected override void BeginAction(IXRSelectInteractor interactor)
    {
        base.BeginAction(interactor);
    }

    protected override void EndAction(IXRSelectInteractor interactor)
    {
        base.EndAction(interactor);
    }

    protected override void Grab(SelectEnterEventArgs args)
    {
        print("grip grab");
        
        base.Grab(new SelectEnterEventArgs
        {
            interactorObject = args.interactorObject,
            interactableObject = args.interactableObject,
            manager = interactionManager
        });
        weapon.SetGripHand(args.interactorObject);
    }

    protected override void Drop(SelectExitEventArgs args)
    {
        print("grip drop");

        base.Drop(new SelectExitEventArgs
        {
            interactorObject = args.interactorObject,
            interactableObject = args.interactableObject,
            manager = interactionManager,
            isCanceled = args.isCanceled
        });
        weapon.ClearGripHand(args.interactorObject);
    }
}
