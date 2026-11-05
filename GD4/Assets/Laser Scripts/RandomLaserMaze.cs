
using UnityEngine;
using System.Collections.Generic;

public class RandomLaserMaze : MonoBehaviour
{
    [Header("Laser")]
    public GameObject laserPrefab;

    [Header("Walls")]
    public Transform startFace;
    public Transform endFace;

    [Header("Laser Count")]
    public int minLaserCount = 5;
    public int maxLaserCount = 10;

    [Header("Height")]
    public float minHeight = 0.5f;
    public float maxHeight = 2.2f;

    [Header("Random Position")]
    public float minZSpacing = 0.5f;
    public float edgePadding = 0.3f;

    [Header("Random Tilt")]
    public float maxHeightDifference = 1.2f;

    private readonly List<GameObject> spawnedLasers = new List<GameObject>();

    void Start()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        ClearLasers();

        if (laserPrefab == null || startFace == null || endFace == null)
        {
            Debug.LogError("Assign Laser Prefab and both walls!");
            return;
        }

        BoxCollider startBox = startFace.GetComponent<BoxCollider>();
        BoxCollider endBox = endFace.GetComponent<BoxCollider>();

        if (startBox == null || endBox == null)
        {
            Debug.LogError("Both walls need BoxColliders!");
            return;
        }

        int count = Random.Range(minLaserCount, maxLaserCount + 1);

        float startHalfZ = startBox.size.z * 0.5f;
        float endHalfZ = endBox.size.z * 0.5f;

        float usableStartZ = startHalfZ - edgePadding;
        float usableEndZ = endHalfZ - edgePadding;

        List<float> zPositions = new List<float>();

        for (int i = 0; i < count; i++)
        {
            float z = Random.Range(-usableStartZ, usableStartZ);

            int attempts = 0;

            while (IsTooClose(z, zPositions) && attempts < 30)
            {
                z = Random.Range(-usableStartZ, usableStartZ);
                attempts++;
            }

            zPositions.Add(z);
        }

        for (int i = 0; i < count; i++)
        {
            float startZ = zPositions[i];

            float endZ = Random.Range(-usableEndZ, usableEndZ);

            float startY = Random.Range(minHeight, maxHeight);
            float endY = startY + Random.Range(
                -maxHeightDifference,
                maxHeightDifference
            );

            endY = Mathf.Clamp(endY, minHeight, maxHeight);

            Vector3 startLocal = new Vector3(
                startBox.center.x + startBox.size.x * 0.5f,
                startBox.center.y - startBox.size.y * 0.5f + startY,
                startBox.center.z + startZ
            );

            Vector3 endLocal = new Vector3(
                endBox.center.x - endBox.size.x * 0.5f,
                endBox.center.y - endBox.size.y * 0.5f + endY,
                endBox.center.z + endZ
            );

            Vector3 startPoint = startFace.TransformPoint(startLocal);
            Vector3 endPoint = endFace.TransformPoint(endLocal);

            CreateLaser(startPoint, endPoint);
        }
    }

    bool IsTooClose(float z, List<float> positions)
    {
        foreach (float p in positions)
        {
            if (Mathf.Abs(z - p) < minZSpacing)
                return true;
        }

        return false;
    }

    void CreateLaser(Vector3 start, Vector3 end)
    {
        GameObject laser = Instantiate(laserPrefab);

        Vector3 direction = end - start;
        float distance = direction.magnitude;

        laser.transform.position = (start + end) * 0.5f;
        laser.transform.rotation = Quaternion.FromToRotation(
            Vector3.up,
            direction.normalized
        );

        // Unity Cylinder height is along Y.
        // Default cylinder height is 2 units.
        laser.transform.localScale = new Vector3(
            laser.transform.localScale.x,
            distance * 0.5f,
            laser.transform.localScale.z
        );

        spawnedLasers.Add(laser);
    }

    public void ClearLasers()
    {
        foreach (GameObject laser in spawnedLasers)
        {
            if (laser != null)
                Destroy(laser);
        }

        spawnedLasers.Clear();
    }
}
