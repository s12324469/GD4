using UnityEngine;

public class PanelShow : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Object Collider")]
    [SerializeField] private Collider touchCollider;

    [Header("Hand Colliders")]
    [SerializeField] private Collider leftHandCollider;
    [SerializeField] private Collider rightHandCollider;

    private bool touching = false;

    private Collider[] results = new Collider[10];

    private void Start()
    {
        panel.SetActive(false);

        if (touchCollider == null)
            touchCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        bool leftTouch = CheckHand(leftHandCollider);
        bool rightTouch = CheckHand(rightHandCollider);

        bool newTouchState = leftTouch || rightTouch;

        if (newTouchState != touching)
        {
            touching = newTouchState;

            panel.SetActive(touching);

            Debug.Log("Touch: " + touching);
        }
    }

    private bool CheckHand(Collider hand)
    {
        if (hand == null || touchCollider == null)
            return false;

        Bounds bounds = hand.bounds; // الحجم لل Collider

        int count = Physics.OverlapBoxNonAlloc(
            bounds.center,
            bounds.extents,
            results,
            hand.transform.rotation
        );

        for (int i = 0; i < count; i++)
        {
            if (results[i] == touchCollider)
            {
                Debug.Log("HAND TOUCHED OBJECT: " + hand.name);
                return true;
            }
        }

        return false;
    }
}