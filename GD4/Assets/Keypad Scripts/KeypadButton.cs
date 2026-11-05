
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public KeypadController keypad;
    public string digit = "1";

    public void Press()
    {
        if (keypad != null)
            keypad.AddDigit(digit);
    }
}
