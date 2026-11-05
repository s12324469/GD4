
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationAngle = 60f;
    public float rotationSpeed = 1f;

    [Header("Detection")]
    public Transform playerHead;
    public Transform cameraView;
    public float viewDistance = 10f;

    [Range(1f, 360f)]
    public float viewAngle = 180f;

    [Header("Damage")]
    public PlayerHealth playerHealth;
    public int damageAmount = 10;
    public float damageInterval = 1f;

    private Quaternion startRotation;
    private bool playerDetected;
    private float nextDamageTime;

    void Start()
    {
        startRotation = transform.localRotation;

        if (playerHead == null)
            Debug.LogError("Player Head is NOT assigned!");

        if (cameraView == null)
            Debug.LogError("Camera View is NOT assigned!");

        if (playerHealth == null && playerHead != null)
        {
            playerHealth = playerHead.GetComponentInParent<PlayerHealth>();

            if (playerHealth == null)
                playerHealth = playerHead.GetComponent<PlayerHealth>();
        }

        if (playerHealth == null)
            Debug.LogWarning("PlayerHealth is NOT assigned!");
    }

    void Update()
    {
        RotateCamera();
        DetectPlayer();
        ApplyDamage();
    }

    void RotateCamera()
    {
        float angle = Mathf.Sin(Time.time * rotationSpeed)
                      * rotationAngle;

        transform.localRotation =
            startRotation * Quaternion.Euler(0f, angle, 0f);
    }

    void DetectPlayer()
    {
        if (playerHead == null || cameraView == null)
        {
            SetDetected(false);
            return;
        }

        Vector3 origin = cameraView.position;
        Vector3 direction = playerHead.position - origin;

        float distance = direction.magnitude;

        float angle = 0f;

        if (distance > 0.001f)
        {
            angle = Vector3.Angle(
                cameraView.forward,
                direction.normalized
            );
        }

        bool inRange = distance <= viewDistance;
        bool inAngle = angle <= viewAngle / 2f;
        bool detected = inRange && inAngle;

        Debug.DrawRay(
            origin,
            cameraView.forward * viewDistance,
            Color.blue
        );

        Debug.DrawRay(
            origin,
            direction,
            detected ? Color.green : Color.red
        );

        Debug.Log(
            "Distance: " + distance.ToString("F2") +
            " | Angle: " + angle.ToString("F1") +
            " | In Range: " + inRange +
            " | In Angle: " + inAngle +
            " | Detected: " + detected
        );

        SetDetected(detected);
    }

    void SetDetected(bool detected)
    {
        if (playerDetected == detected)
            return;

        playerDetected = detected;

        if (detected)
        {
            Debug.Log("PLAYER DETECTED!");
            nextDamageTime = 0f;
        }
        else
        {
            Debug.Log("Player is hidden.");
        }
    }

    void ApplyDamage()
    {
        if (!playerDetected || playerHealth == null)
            return;

        if (Time.time >= nextDamageTime)
        {
            playerHealth.TakeDamage(damageAmount);

            Debug.Log(
                "PLAYER DAMAGED! -" + damageAmount +
                " HP | Current Health: " +
                playerHealth.currentHealth
            );

            nextDamageTime = Time.time + damageInterval;
        }
    }
}
