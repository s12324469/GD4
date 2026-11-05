using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage; // Drag the Fill image here
    private PlayerHealth playerHealth;
    private float targetFill = 1f;

    void Start()
    {
        if (fillImage == null)
        {
            Debug.LogError("[HealthUI] Fill Image is not assigned in the Inspector!");
            return;
        }

        playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("[HealthUI] No PlayerHealth found in the scene!");
            return;
        }

        playerHealth.OnHealthChanged += UpdateBar;
        UpdateBar(playerHealth.currentHealth, playerHealth.maxHealth);
        Debug.Log("[HealthUI] Health bar linked successfully");
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateBar;
    }

    void UpdateBar(int current, int max)
    {
        targetFill = (float)current / max;
        Debug.Log($"[HealthUI] Health changed: {current} / {max}");
    }

    void Update()
    {
        if (fillImage == null) return;

        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * 8f);

        float t = fillImage.fillAmount;
        fillImage.color = t > 0.5f
            ? Color.Lerp(Color.yellow, Color.green, (t - 0.5f) * 2f)
            : Color.Lerp(Color.red, Color.yellow, t * 2f);
    }
}