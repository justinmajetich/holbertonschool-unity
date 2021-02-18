using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    NavMeshAgent agent;

    bool isStopped = false;    
    int avoidanceDirection = 0;

    [SerializeField] float baseDestinationRange = 3f;
    Vector3 currentDestination;


    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        
        // GetNewDestination();
    }

    void Update()
    {
        // If agent is moving towards new grazing position.
        if (!isStopped)
        {
            AvoidStoppedAgents();
        }

        // If agent is not flagged as stopped but has arrived at destination.
        if (!isStopped && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || Mathf.Approximately(agent.velocity.sqrMagnitude, 0f))
                {
                    StartCoroutine(Stopped());
                }
            }
        }
    }

    void AvoidStoppedAgents()
    {
        int layerMask = 1 << 9; // "Target" layer.

        // Cast rays slightly angled left/right to avoid running into other targets.
        bool leftRay = Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(-0.5f, 0f, 0.6f)), Quaternion.Euler(0, -5f, 0f) * transform.forward, 20f, layerMask);
        bool rightRay = Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.5f, 0f, 0.6f)), Quaternion.Euler(0, 5f, 0f) * transform.forward, 20f, layerMask);

        // Set avoidance direction according to raycasts.
        if (leftRay && rightRay) {
            if (avoidanceDirection == 0)
                avoidanceDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        }
        else if (leftRay) {
            avoidanceDirection = 1;
        }
        else if (rightRay) {
            avoidanceDirection = -1;
        }
        else {
            avoidanceDirection = 0;
            return;
        }

        // Take vector pointing from agent to destination.
        Vector3 directionToDestination = currentDestination - transform.position;

        // Rotate the direction vector by 2.5f in appropriate direction.
        directionToDestination = Quaternion.Euler(0f, 2.5f * avoidanceDirection, 0f) * directionToDestination;

        // New destination is equal to rotated direction plus transform position.
        Vector3 newDestination = directionToDestination + transform.position;

        SetDestination(newDestination);
    }

    IEnumerator Stopped()
    {     
        isStopped = true;
        agent.enabled = false;

        yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));

        agent.enabled = true;
        GetNewDestination();

        isStopped = false;
    }

    public void GetNewDestination()
    {
        // int layerMask = 1 << 8; // "ARPlane" layer.
        RaycastHit hit;

        while (true) {
            // Calculate new randomized destination.
            Vector3 randomizedOffset = Utility.randomInsideXZCircle(baseDestinationRange + Random.Range(-1.0f, 2.0f));

            Vector3 newDestination = new Vector3(transform.position.x + Utility.InverseClamp(randomizedOffset.x, -1.5f, 1.5f),
                                                transform.position.y,
                                                transform.position.z + Utility.InverseClamp(randomizedOffset.z, -1.5f, 1.5f));

            // Check if new position is within bounds of game plane with downward raycast, and if so, set as a new destination.
            if (Physics.Raycast(newDestination, -Vector3.up, out hit, Mathf.Infinity)) {
                
                if (hit.collider.tag == "ARPlane") {
                    SetDestination(newDestination);
                    return;
                }
            }
        }
    
    }

    void SetDestination(Vector3 destination)
    {
        agent.ResetPath();
        agent.SetDestination(destination);
        currentDestination = destination;
    }
}
