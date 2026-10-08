using System.Collections.Generic;
using UnityEngine;

public class Block_MovingController : MonoBehaviour, IOperatable
{
    public bool isOperating = true;

    public float moveSpeed = 3f;

    public float waitTime = 1f;

    public List<Transform> waypoints;

    private int currentWaypointIndex = 0;
    private Rigidbody rb;

    private bool isWaiting = false;
    private float waitTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (!isOperating || waypoints.Count == 0) return;

        if (isWaiting)
        {
            waitTimer += Time.fixedDeltaTime;

            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    currentWaypointIndex = 0;
                }
            }
        }
        else
        {
            MoveTowardsWaypoint();
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];

        float distance = Vector3.Distance(rb.position, target.position);

        if (distance < 0.1f)
        {
            isWaiting = true;
            waitTimer = 0f;
        }
        else
        {
            Vector3 nextPosition = Vector3.MoveTowards(rb.position, target.position, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(nextPosition);
        }
    }

    public void SetOperating(bool state)
    {
        isOperating = state;
    }

    public void ToggleOperating()
    {
        isOperating = !isOperating;
    }
}
