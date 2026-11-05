
using UnityEngine;

public class PlayerBodyCollider : MonoBehaviour
{
    [Header("References")]
    public Transform head;
    public CapsuleCollider bodyCollider;

    [Header("Body Settings")]
    public float standingHeight = 1.88f;
    public float minimumHeight = 0.8f;
    public float headClearance = 0.15f;

    private void LateUpdate()
    {
        if (head == null || bodyCollider == null)
            return;

        float headHeight =
            transform.InverseTransformPoint(head.position).y;

        float newHeight = Mathf.Clamp(
            headHeight + headClearance,
            minimumHeight,
            standingHeight
        );

        bodyCollider.height = newHeight;

        Vector3 center = bodyCollider.center;
        center.y = newHeight / 2f;
        bodyCollider.center = center;
    }
}
