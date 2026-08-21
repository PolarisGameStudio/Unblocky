using UnityEngine;

public class ClipToGate : MonoBehaviour
{
    [Header("Gate Settings")]
    public Transform gateTransform; // Kéo thả Transform của Gate vào đây

    [Tooltip("Hướng giữ lại (Vector chỉ về phía phần Mesh KHÔNG bị mất)")]
    public Vector3 keepDirection = Vector3.forward;

    private Renderer meshRenderer;
    private MaterialPropertyBlock mpb;

    private static readonly int ClipPositionID = Shader.PropertyToID("_ClipPosition");
    private static readonly int ClipNormalID = Shader.PropertyToID("_ClipNormal");

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void SetMatClipping(Vector3 vWorldpos, Vector3 vWorldNormal)
    {
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetVector(ClipPositionID, vWorldpos);
        mpb.SetVector(ClipNormalID, vWorldNormal);
        meshRenderer.SetPropertyBlock(mpb);
    }

}
