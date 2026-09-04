using Flavor;
using UnityEngine;

public class ClipToGate : MonoBehaviour
{

    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private MaterialPropertyBlock mpb;

    private static readonly int ClipPositionID = Shader.PropertyToID("_ClipPosition");
    private static readonly int ClipNormalID = Shader.PropertyToID("_ClipNormal");

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void SetMatClipping(Vector3 vWorldpos, Vector3 vWorldNormal)
    {
        this.Log($"vWorldPos {vWorldpos} - vWorldNormal {vWorldNormal}");
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetVector(ClipPositionID, vWorldpos);
        mpb.SetVector(ClipNormalID, vWorldNormal);
        meshRenderer.SetPropertyBlock(mpb);
    }

    public void SetClippingGate(IGateInfo gateInfo)
    {
        var vWorldPos = gateInfo.vPos;
        var vWorldNormal = -DirectionExtensions.ToWorldVector(gateInfo.Direction);

        vWorldPos += vWorldNormal * 1f;

        if (vWorldPos == null || vWorldNormal == null) return;

        SetMatClipping(vWorldPos, vWorldNormal);
    }

}
