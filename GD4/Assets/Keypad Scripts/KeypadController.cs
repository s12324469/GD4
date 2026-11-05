
using UnityEngine;
using TMPro;
using System.Collections;

public class KeypadController : MonoBehaviour
{
    [Header("Password")]
    public string correctPassword = "2580";
    private string enteredPassword = "";

    [Header("Display")]
    public TMP_Text passwordDisplay;

    [Header("Door")]
    public Transform doorPivot;
    public float openAngle = 90f;
    public float openDuration = 1.5f;

    private bool isOpening = false;
    private Quaternion closedRotation;

    private void Start()
    {
        if (doorPivot != null)
            closedRotation = doorPivot.localRotation;

        UpdateDisplay();
    }

    public void AddDigit(string digit)
    {
        if (isOpening || enteredPassword.Length >= 8)
            return;

        enteredPassword += digit;
        UpdateDisplay();
    }

    public void ClearPassword()
    {
        if (isOpening) return;

        enteredPassword = "";
        UpdateDisplay();
    }

    public void CheckPassword()
    {
        if (isOpening) return;

        if (enteredPassword == correctPassword)
        {
            if (doorPivot == null)
            {
                Debug.LogError("Door Pivot is not assigned!");
                return;
            }

            isOpening = true;
            StartCoroutine(OpenDoor());
        }
        else
        {
            Debug.Log("Wrong password!");
            enteredPassword = "";
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (passwordDisplay != null)
            passwordDisplay.text = enteredPassword;
    }

    private IEnumerator OpenDoor()
    {
        Quaternion startRotation = doorPivot.localRotation;
        Quaternion targetRotation =
            closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / openDuration);

            doorPivot.localRotation =
                Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        doorPivot.localRotation = targetRotation;
        Debug.Log("Correct password! Door opened.");
    }
}
