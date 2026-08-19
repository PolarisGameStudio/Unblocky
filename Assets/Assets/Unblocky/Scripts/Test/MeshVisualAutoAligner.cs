using UnityEngine;

public class MeshVisualAutoAligner : MonoBehaviour
{
    public enum AlignMode
    {
        CenterToCenter,

        MinXToMinX,
        MaxXToMaxX,
        MinZToMinZ,
        MaxZToMaxZ,

        MinXMinZToMinXMinZ,
        MinXMaxZToMinXMaxZ,
        MaxXMinZToMaxXMinZ,
        MaxXMaxZToMaxXMaxZ
    }

    [Header("References")]
    [SerializeField] private Transform visualRoot;
    [SerializeField] private BoxCollider targetCollider;

    [Header("Align")]
    [SerializeField] private AlignMode alignMode = AlignMode.CenterToCenter;

    [Header("Axis")]
    [SerializeField] private bool alignX = true;
    [SerializeField] private bool alignY = false;
    [SerializeField] private bool alignZ = true;

    [ContextMenu("Auto Align Visual To Collider")]
    private void AutoAlignVisualToCollider()
    {
        if (visualRoot == null)
        {
            Debug.LogError("Visual Root is missing.");
            return;
        }

        if (targetCollider == null)
        {
            targetCollider = GetComponent<BoxCollider>();
        }

        if (targetCollider == null)
        {
            Debug.LogError("Target Collider is missing.");
            return;
        }

        Renderer[] renderers = visualRoot.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogError("No renderer found inside Visual Root.");
            return;
        }

        Bounds visualLocalBounds = CalculateVisualBoundsInRootLocalSpace(renderers);
        Bounds colliderLocalBounds = new Bounds(targetCollider.center, targetCollider.size);

        Vector3 visualAnchor = GetAnchorPoint(visualLocalBounds, alignMode);
        Vector3 colliderAnchor = GetAnchorPoint(colliderLocalBounds, alignMode);

        Vector3 localDelta = colliderAnchor - visualAnchor;

        if (!alignX) localDelta.x = 0f;
        if (!alignY) localDelta.y = 0f;
        if (!alignZ) localDelta.z = 0f;

        Vector3 worldDelta = transform.TransformVector(localDelta);

        visualRoot.position += worldDelta;

        Debug.Log($"Auto aligned visual. Delta Local = {localDelta}");
    }

    private Bounds CalculateVisualBoundsInRootLocalSpace(Renderer[] renderers)
    {
        bool hasBounds = false;
        Bounds result = new Bounds();

        foreach (Renderer renderer in renderers)
        {
            Bounds worldBounds = renderer.bounds;

            Vector3 center = worldBounds.center;
            Vector3 extents = worldBounds.extents;

            Vector3[] corners =
            {
                center + new Vector3(-extents.x, -extents.y, -extents.z),
                center + new Vector3(-extents.x, -extents.y,  extents.z),
                center + new Vector3(-extents.x,  extents.y, -extents.z),
                center + new Vector3(-extents.x,  extents.y,  extents.z),

                center + new Vector3( extents.x, -extents.y, -extents.z),
                center + new Vector3( extents.x, -extents.y,  extents.z),
                center + new Vector3( extents.x,  extents.y, -extents.z),
                center + new Vector3( extents.x,  extents.y,  extents.z),
            };

            foreach (Vector3 corner in corners)
            {
                Vector3 localCorner = transform.InverseTransformPoint(corner);

                if (!hasBounds)
                {
                    result = new Bounds(localCorner, Vector3.zero);
                    hasBounds = true;
                }
                else
                {
                    result.Encapsulate(localCorner);
                }
            }
        }

        return result;
    }

    private Vector3 GetAnchorPoint(Bounds bounds, AlignMode mode)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        Vector3 center = bounds.center;

        switch (mode)
        {
            case AlignMode.CenterToCenter:
                return center;

            case AlignMode.MinXToMinX:
                return new Vector3(min.x, center.y, center.z);

            case AlignMode.MaxXToMaxX:
                return new Vector3(max.x, center.y, center.z);

            case AlignMode.MinZToMinZ:
                return new Vector3(center.x, center.y, min.z);

            case AlignMode.MaxZToMaxZ:
                return new Vector3(center.x, center.y, max.z);

            case AlignMode.MinXMinZToMinXMinZ:
                return new Vector3(min.x, center.y, min.z);

            case AlignMode.MinXMaxZToMinXMaxZ:
                return new Vector3(min.x, center.y, max.z);

            case AlignMode.MaxXMinZToMaxXMinZ:
                return new Vector3(max.x, center.y, min.z);

            case AlignMode.MaxXMaxZToMaxXMaxZ:
                return new Vector3(max.x, center.y, max.z);

            default:
                return center;
        }
    }
}