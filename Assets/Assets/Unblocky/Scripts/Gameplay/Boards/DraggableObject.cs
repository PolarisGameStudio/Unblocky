using Flavor;
using System;
using UnityEngine;

public sealed class DraggableObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float followSpeed = 12f;
    [SerializeField] private float maxVelocity = 20f;
    [SerializeField] private float snapBeforeDropDistance = 0.05f;

    private bool isDragging;
    private bool isMovingToDropPoint;
    private Vector3 targetPosition;
    private Vector3 dropTargetPosition;

    public event Action DroppedOnGround;
    public bool IsDragging => isDragging;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        SwitchToPlacedPhysics();
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            MoveToTargetByVelocity();
            return;
        }

        if (isMovingToDropPoint)
        {
            MoveToDropPointByVelocity();
            return;
        }
    }

    public void BeginDrag()
    {
        isDragging = true;
        isMovingToDropPoint = false;

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        ClearVelocity();
    }

    public void SetDragTarget(Vector3 target)
    {
        targetPosition = target;
    }

    public void MoveToBeforeDrop(Vector3 finalWorldPosition)
    {
        isDragging = false;
        isMovingToDropPoint = true;

        dropTargetPosition = finalWorldPosition;

        targetPosition = new Vector3(
            finalWorldPosition.x,
            rb.position.y,
            finalWorldPosition.z
        );

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        ClearVelocity();
    }

    private void BeginPhysicalDrop()
    {
        isDragging = false;
        isMovingToDropPoint = false;

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.constraints =
            RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotation;

        ClearVelocity();
    }

    private void SwitchToPlacedPhysics()
    {
        isDragging = false;
        isMovingToDropPoint = false;

        ClearVelocity();

        rb.isKinematic = true;
        rb.useGravity = false;

        rb.constraints =
            RigidbodyConstraints.FreezePosition |
            RigidbodyConstraints.FreezeRotation;
    }

    private void MoveToTargetByVelocity()
    {
        if (rb == null || rb.isKinematic)
            return;

        Vector3 current = rb.position;

        Vector3 currentXZ = new Vector3(current.x, 0f, current.z);
        Vector3 targetXZ = new Vector3(targetPosition.x, 0f, targetPosition.z);

        Vector3 directionXZ = targetXZ - currentXZ;
        Vector3 velocity = directionXZ * followSpeed;

        if (velocity.magnitude > maxVelocity)
            velocity = velocity.normalized * maxVelocity;

        rb.linearVelocity = new Vector3(velocity.x, 0f, velocity.z);

        Vector3 fixedPosition = rb.position;
        fixedPosition.y = targetPosition.y;

        rb.position = fixedPosition;
        transform.position = fixedPosition;
    }

    private void MoveToDropPointByVelocity()
    {
        if (rb == null || rb.isKinematic)
            return;

        Vector3 current = rb.position;

        Vector3 currentXZ = new Vector3(current.x, 0f, current.z);
        Vector3 targetXZ = new Vector3(targetPosition.x, 0f, targetPosition.z);

        Vector3 direction = targetPosition - current;
        Vector3 velocity = direction * followSpeed;

        if (velocity.magnitude > maxVelocity)
            velocity = velocity.normalized * maxVelocity;

        rb.linearVelocity = velocity;

        float distanceXZ = Vector3.Distance(currentXZ, targetXZ);

        if (distanceXZ <= snapBeforeDropDistance)
        {
            Vector3 snapPosition = new Vector3(
                dropTargetPosition.x,
                rb.position.y,
                dropTargetPosition.z
            );

            ClearVelocity();

            rb.position = snapPosition;
            transform.position = snapPosition;

            BeginPhysicalDrop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.IsInLayerMask(groundMask)) return;

        if (isDragging)
            return;

        SwitchToPlacedPhysics();
        DroppedOnGround?.Invoke();
    }

    private void ClearVelocity()
    {
        if (rb == null || rb.isKinematic)
            return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


}