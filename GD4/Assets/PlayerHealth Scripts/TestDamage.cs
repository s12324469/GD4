using UnityEngine;
using UnityEngine.InputSystem;

public class TestDamage : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }
}