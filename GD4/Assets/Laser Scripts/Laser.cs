using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 10;
    public float damageCooldown = 1f;
    private float lastDamageTime = -10f;

    void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    void OnTriggerStay(Collider other)
    {
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time < lastDamageTime + damageCooldown) return;

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}