using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartOnDeath : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    public float waitAfterFade = 0.5f;
    public Camera playerCamera;

    private PlayerHealth health;
    private Image fadeImage;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        if (health == null)
        {
            Debug.LogError("[Restart] PlayerHealth not found on this object!");
            return;
        }

        if (playerCamera == null) playerCamera = Camera.main;

        CreateFadeCanvas();
        health.OnDied += OnPlayerDied;
    }

    void OnDestroy()
    {
        if (health != null)
            health.OnDied -= OnPlayerDied;
    }

    void CreateFadeCanvas()
    {
        GameObject canvasGO = new GameObject("FadeCanvas");
        canvasGO.transform.SetParent(playerCamera.transform, false);

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 1000;

        RectTransform rt = canvasGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1000, 1000);
        rt.localScale = Vector3.one * 0.002f;
        rt.localPosition = new Vector3(0, 0, 0.3f);
        rt.localRotation = Quaternion.identity;

        GameObject imgGO = new GameObject("Black");
        imgGO.transform.SetParent(canvasGO.transform, false);

        fadeImage = imgGO.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = false;

        RectTransform r = fadeImage.rectTransform;
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;
    }

    void OnPlayerDied()
    {
        Debug.Log("[Restart] Player died, starting fade.");
        StartCoroutine(FadeAndRestart());
    }

    IEnumerator FadeAndRestart()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = Color.black;
        yield return new WaitForSeconds(waitAfterFade);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}