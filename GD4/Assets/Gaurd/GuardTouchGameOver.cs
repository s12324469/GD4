using UnityEngine;

public class GuardTouchGameOver : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER DETECTED: " + other.name);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("PANEL ACTIVATED!");
        }
        else
        {
            Debug.LogError("GAME OVER PANEL IS NOT ASSIGNED!");
        }
    }
}