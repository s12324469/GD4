
using UnityEngine;
using System.Collections;

public class LaserSwitch : MonoBehaviour
{
    [Header("Laser Objects - 4 Objects")]
    public GameObject[] laserObjects = new GameObject[4];

    [Header("Settings")]
    public float offDuration = 30f;

    private bool isCoolingDown = false;

    public void PressSwitch()
    {
        if (isCoolingDown)
            return;

        StartCoroutine(TurnOffLaser());
    }

    IEnumerator TurnOffLaser()
    {
        isCoolingDown = true;

        // Turn off all 4 laser objects
        foreach (GameObject laser in laserObjects)
        {
            if (laser != null)
                laser.SetActive(false);
        }

        Debug.Log("All lasers OFF for 30 seconds");

        yield return new WaitForSeconds(offDuration);

        // Turn all 4 laser objects back on
        foreach (GameObject laser in laserObjects)
        {
            if (laser != null)
                laser.SetActive(true);
        }

        Debug.Log("All lasers ON again");

        isCoolingDown = false;
    }
}

