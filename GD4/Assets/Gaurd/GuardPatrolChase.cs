
using UnityEngine;

public class GuardPatrolChase : MonoBehaviour
{
    public enum GuardState
    {
        Patrol,
        Chase,
        Return
    }

    [Header("Player")]
    public Transform player;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 1.5f;
    public float pointReachDistance = 0.3f;

    [Header("Chase")]
    public float chaseSpeed = 3.5f;
    public float rotationSpeed = 360f;

    [Header("State")]
    public GuardState currentState = GuardState.Patrol;

    private int patrolIndex = 0;
    private Vector3 lastSeenPosition;
    private bool playerDetected;
    private bool playerInLoseRange;
    private bool hasDetectedPlayer;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Assign XR Origin to Player!");
            enabled = false;
            return;
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("Assign Patrol Points!");
            enabled = false;
            return;
        }

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null)
            {
                Debug.LogError("Patrol Point is missing!");
                enabled = false;
                return;
            }
        }

        lastSeenPosition = player.position;
    }

    void Update()
    {
        switch (currentState)
        {
            case GuardState.Patrol:
                Patrol();
                break;

            case GuardState.Chase:
                Chase();
                break;

            case GuardState.Return:
                ReturnToPatrol();
                break;
        }
    }

    void Patrol()
    {
        MoveTowards(patrolPoints[patrolIndex].position,
                    patrolSpeed);

        if (HorizontalDistance(
            transform.position,
            patrolPoints[patrolIndex].position)
            <= pointReachDistance)
        {
            patrolIndex = (patrolIndex + 1)
                          % patrolPoints.Length;
        }
    }

    void Chase()
    {
        // If the player is currently visible in the
        // detection zone, update their latest position.
        if (playerDetected)
        {
            lastSeenPosition = player.position;
        }

        MoveTowards(lastSeenPosition, chaseSpeed);

        // If the player escaped the outer zone,
        // return to the patrol route.
        if (!playerInLoseRange)
        {
            currentState = GuardState.Return;
            Debug.Log("PLAYER ESCAPED - RETURNING");
            return;
        }

        // If the player left the detection zone but
        // remains inside the lose zone, the guard
        // moves toward the last known position.
        if (!playerDetected &&
            HorizontalDistance(
                transform.position,
                lastSeenPosition) <= pointReachDistance)
        {
            currentState = GuardState.Return;
            Debug.Log("PLAYER LOST - RETURNING");
        }
    }

    void ReturnToPatrol()
    {
        Vector3 target = patrolPoints[patrolIndex].position;

        MoveTowards(target, patrolSpeed);

        if (HorizontalDistance(transform.position, target)
            <= pointReachDistance)
        {
            currentState = GuardState.Patrol;

            if (playerDetected && playerInLoseRange)
            {
                currentState = GuardState.Chase;
            }

            Debug.Log("PATROL RESUMED");
        }
    }

    void MoveTowards(Vector3 target, float speed)
    {
        // Ignore vertical differences: VR head height
        // should not make the guard move into the air.
        target.y = transform.position.y;

        Vector3 direction = target - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    float HorizontalDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }

    public void EnterDetectionRange(Transform detectedPlayer)
    {
        if (detectedPlayer != player)
            return;

        playerDetected = true;
        playerInLoseRange = true;
        hasDetectedPlayer = true;
        lastSeenPosition = player.position;

        currentState = GuardState.Chase;

        Debug.Log("PLAYER DETECTED - CHASING");
    }

    public void ExitDetectionRange(Transform detectedPlayer)
    {
        if (detectedPlayer != player)
            return;

        playerDetected = false;
        Debug.Log("PLAYER LEFT DETECTION RANGE");
    }

    public void EnterLoseRange(Transform detectedPlayer)
    {
        if (detectedPlayer != player)
            return;

        playerInLoseRange = true;
    }

    public void ExitLoseRange(Transform detectedPlayer)
    {
        if (detectedPlayer != player)
            return;

        playerInLoseRange = false;

        if (hasDetectedPlayer)
        {
            currentState = GuardState.Return;
            Debug.Log("PLAYER ESCAPED - RETURNING");
        }
    }
}
