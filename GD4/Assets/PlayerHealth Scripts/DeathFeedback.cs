
using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class DeathFeedback : MonoBehaviour
{
    public float hitAmplitude = 0.3f;
    public float hitDuration = 0.1f;

    public float deathAmplitude = 1f;

    private PlayerHealth health;

    void Start()
    {
        health = GetComponent<PlayerHealth>();

        if (health == null)
        {
            Debug.LogError("[Haptic] ما لقيت PlayerHealth على نفس الكائن!");
            return;
        }

        health.OnDamaged += OnHit;
        health.OnDied += OnDeath;

        Debug.Log("[Haptic] جاهز، مربوط مع PlayerHealth.");
    }

    void OnDestroy()
    {
        if (health == null) return;

        health.OnDamaged -= OnHit;
        health.OnDied -= OnDeath;
    }

    void OnHit()
    {
        Vibrate(hitAmplitude, hitDuration);
    }

    void OnDeath()
    {
        StartCoroutine(DeathVibration());
    }

    IEnumerator DeathVibration()
    {
        while (true)
        {
            Vibrate(deathAmplitude, 0.2f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Vibrate(float amplitude, float duration)
    {
        VibrateNode(XRNode.LeftHand, amplitude, duration);
        VibrateNode(XRNode.RightHand, amplitude, duration);
    }

    void VibrateNode(XRNode node, float amplitude, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (!device.isValid) return;

        HapticCapabilities caps;

        if (!device.TryGetHapticCapabilities(out caps) ||
            !caps.supportsImpulse)
            return;

        device.SendHapticImpulse(0, amplitude, duration);
    }
}
