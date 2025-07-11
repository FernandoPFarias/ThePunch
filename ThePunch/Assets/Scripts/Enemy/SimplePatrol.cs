using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    private int currentPoint = 0;
    private bool isActive = true;

    public void SetActive(bool active)
    {
        isActive = active;
    }

    void Update()
    {
        if (!isActive || patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }
} 