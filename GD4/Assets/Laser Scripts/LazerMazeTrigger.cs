
using UnityEngine;

public class LaserMazeTrigger : MonoBehaviour
{
    public RandomLaserMaze laserMaze;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            laserMaze.GenerateMaze();
        }
    }
}
