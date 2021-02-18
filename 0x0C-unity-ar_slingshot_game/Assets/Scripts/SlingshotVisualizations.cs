using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotVisualizations : MonoBehaviour
{
    LineRenderer m_LineRenderer;
    public int resolution = 15;
    public float startingWidth = 0.05f;
    public float endingWidth = 0.01f;
    float gravity;


    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = resolution;
        m_LineRenderer.startWidth = startingWidth;
        m_LineRenderer.endWidth = endingWidth;

        gravity = -Physics.gravity.y;
    }

    public void Visualize(Transform ammo, float velocity, float angle = 0f)
    {
        m_LineRenderer.positionCount = resolution + 1;

        Vector3[] positions = new Vector3[resolution + 1]; 

        // Convert angle to radian.
        float radianAngle = Mathf.Deg2Rad * angle;

        // Get offset of ammo Y position from game plane Y position.
        float gamePlaneY = ARSurfaceManager.gamePlane.transform.position.y;
        float ammoY = ammo.position.y;
        float yOffsetFromGamePlane = Mathf.Sqrt(Mathf.Pow(ammoY - gamePlaneY, 2f));

        // Get max distance of trajectory along X axis.
        float trajectoryDistance = TrajectoryDistance(velocity, radianAngle, yOffsetFromGamePlane);
        
        for (int i = 0; i < resolution + 1; i++)
        {
            float t = i / (float)resolution;
            float z = t * trajectoryDistance;

            float y = z * Mathf.Tan(radianAngle) - ((gravity * (z * z)) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

            positions[i] = (y * Vector3.up) + (z * CalculateTrajectoryDirection(trajectoryDistance, ammo)) + ammo.position;
        }
        m_LineRenderer.SetPositions(positions);
    }

    float TrajectoryDistance(float velocity, float angle, float initialHeight = 0f)
    {
        float zVelocity = Mathf.Cos(angle) * velocity; 
        float yVelocity = Mathf.Sin(angle) * velocity;
        return (zVelocity / gravity) * (yVelocity + Mathf.Sqrt(yVelocity * yVelocity + 2f * gravity * initialHeight));
    }

    Vector3 CalculateTrajectoryDirection(float trajectoryDistance, Transform ammo)
    {
        Vector3 direction = 
            transform.forward * trajectoryDistance
            + transform.position
            - ammo.transform.position;

        // Only want to affect the X and Z axes.
        // direction.y = 0;

        // Isolate direction, not magnitude.
        return direction.normalized;
    }

    public void Hide() {
        m_LineRenderer.positionCount = 0;
    }
}
