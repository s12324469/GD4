using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserScript : MonoBehaviour
{
    private LineRenderer lr;

    [SerializeField] private Transform startPoint;
    [SerializeField] private float laserLength = 5000f;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lr.SetPosition(0, startPoint.position);

        RaycastHit hit;

        if (Physics.Raycast(
            startPoint.position,
            -startPoint.right,
            out hit,
            laserLength))
        {
            lr.SetPosition(1, hit.point);

            // إذا الليزر ضرب اللاعب
            if (hit.collider.CompareTag("Player") ||
                hit.collider.transform.root.CompareTag("Player"))
            {
                RestartScene();
            }
        }
        else
        {
            lr.SetPosition(
                1,
                startPoint.position - startPoint.right * laserLength
            );
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}