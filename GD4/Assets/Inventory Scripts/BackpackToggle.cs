
using UnityEngine;
using UnityEngine.InputSystem;

public class BackpackToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform backpackRoot;
    [SerializeField] private GameObject backpackVisual;
    [SerializeField] private InputActionReference toggleAction;

    [Header("Hand Position Offset")]
    [SerializeField] private Vector3 positionOffset =
        new Vector3(0f, 0.1f, 0.3f);

    [SerializeField] private Vector3 rotationOffset =
        Vector3.zero;

    private bool isOpen = false;
    private Vector3 hiddenPosition = new Vector3(0f, -1000f, 0f);

    private void Awake()
    {
        if (backpackVisual != null)
            backpackVisual.SetActive(false);

        if (backpackRoot != null)
            backpackRoot.position = hiddenPosition;
    }

    private void OnEnable()
    {
        if (toggleAction != null && toggleAction.action != null)
        {
            toggleAction.action.performed += OnToggle;
            toggleAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null && toggleAction.action != null)
        {
            toggleAction.action.performed -= OnToggle;
            toggleAction.action.Disable();
        }
    }

    private void LateUpdate()
    {
        if (!isOpen || handTransform == null || backpackRoot == null)
            return;

        backpackRoot.position =
            handTransform.TransformPoint(positionOffset);

        backpackRoot.rotation =
            handTransform.rotation *
            Quaternion.Euler(rotationOffset);
    }

    private void OnToggle(InputAction.CallbackContext context)
    {
        if (handTransform == null ||
            backpackRoot == null ||
            backpackVisual == null)
            return;

        if (isOpen)
            CloseBackpack();
        else
            OpenBackpack();
    }

    private void OpenBackpack()
    {
        isOpen = true;
        backpackVisual.SetActive(true);
    }

    private void CloseBackpack()
    {
        backpackVisual.SetActive(false);
        backpackRoot.position = hiddenPosition;
        isOpen = false;
    }
}
