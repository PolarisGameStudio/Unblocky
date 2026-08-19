using UnityEngine;

[ExecuteInEditMode]
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

    void LateUpdate()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<Renderer>();
        if (gateTransform == null || meshRenderer == null) return;

        // Khắc phục lỗi Null khi chạy trong Editor
        if (mpb == null)
        {
            mpb = new MaterialPropertyBlock();
        }

        // Lấy vị trí thế giới của Gate
        Vector3 gatePos = gateTransform.position;

        // Hướng Vector pháp tuyến mặt cắt (tự động xoay theo Gate)
        Vector3 worldNormal = gateTransform.TransformDirection(keepDirection).normalized;

        // Truyền tham số vào Shader qua MaterialPropertyBlock
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetVector(ClipPositionID, gatePos);
        mpb.SetVector(ClipNormalID, worldNormal);
        meshRenderer.SetPropertyBlock(mpb);
    }
}
