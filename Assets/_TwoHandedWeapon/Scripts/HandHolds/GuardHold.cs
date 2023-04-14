using UnityEngine.XR.Interaction.Toolkit;

public class GuardHold : HandHold
{
    protected override void Grab(SelectEnterEventArgs args)
    {
        base.Grab(args);
    }

    protected override void Drop(SelectExitEventArgs args)
    {
        base.Drop(args);
    }
}
