
using UnityEngine;

public class GuardRangeTrigger : MonoBehaviour
{
    public enum RangeType
    {
        Detection,
        Lose
    }

    public RangeType rangeType;

    public GuardPatrolChase guard;

    private bool IsPlayer(Collider other)
    {
        if (guard == null || guard.player == null)
            return false;

        Transform t = other.transform;

        return t == guard.player ||
               t.IsChildOf(guard.player);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other))
            return;

        if (rangeType == RangeType.Detection)
        {
            guard.EnterDetectionRange(guard.player);
        }
        else
        {
            guard.EnterLoseRange(guard.player);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other))
            return;

        if (rangeType == RangeType.Detection)
        {
            guard.ExitDetectionRange(guard.player);
        }
        else
        {
            guard.ExitLoseRange(guard.player);
        }
    }
}
