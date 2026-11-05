
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class StepOverLaser : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference stepOverAction;

    [Header("Player")]
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform playerCamera;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 0.7f;
    [SerializeField] private float jumpDuration = 0.6f;
    [SerializeField] private float forwardDistance = 1.2f;

    private bool isJumping;

    private void OnEnable()
    {
        if (stepOverAction != null && stepOverAction.action != null)
        {
            stepOverAction.action.performed += OnJumpPressed;
            stepOverAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (stepOverAction != null && stepOverAction.action != null)
        {
            stepOverAction.action.performed -= OnJumpPressed;
            stepOverAction.action.Disable();
        }

        StopAllCoroutines();
        isJumping = false;
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        if (isJumping || xrOrigin == null || playerCamera == null)
            return;

        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        isJumping = true;

        Vector3 startPosition = xrOrigin.position;

        Vector3 forward = Vector3.ProjectOnPlane(
            playerCamera.forward,
            Vector3.up
        ).normalized;

        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.forward;

        Vector3 endPosition =
            startPosition + forward * forwardDistance;

        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / jumpDuration);

            // حركة للأمام مع قوس نطة للأعلى
            float height = 4f * jumpHeight * t * (1f - t);

            xrOrigin.position = Vector3.Lerp(
                startPosition,
                endPosition,
                t
            ) + Vector3.up * height;

            yield return null;
        }

        xrOrigin.position = endPosition;
        isJumping = false;
    }
}
