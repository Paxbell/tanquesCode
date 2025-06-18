using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public float detectRange = 15f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;

    private Transform targetTank;

    void Update()
    {
        DetectClosestTank();

        if (targetTank != null && Vector3.Distance(transform.position, targetTank.position) < detectRange)
        {
            FollowAndShootTank();
        }
        else
        {
            Patrol();
        }
    }

    void DetectClosestTank()
    {
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestTank = null;

        foreach (GameObject tank in tanks)
        {
            float dist = Vector3.Distance(transform.position, tank.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTank = tank.transform;
            }
        }

        targetTank = closestTank;
    }

    void FollowAndShootTank()
    {
        Vector3 direction = (targetTank.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        
        // Aquí puedes instanciar un proyectil si quieres
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
