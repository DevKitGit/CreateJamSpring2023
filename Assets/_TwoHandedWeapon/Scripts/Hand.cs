using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : XRDirectInteractor
{
    private SkinnedMeshRenderer _meshRenderer = null;
    protected override void Awake()
    {
        base.Awake();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void SetVisibility(bool value)
    {
        _meshRenderer.enabled = value;
    }
}
