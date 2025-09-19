using PathCreation;
using UnityEngine;

public class PropPathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public bool autoSnap = false;

    VertexPath path => pathCreator.path;
    private bool hasSnapped = false;


    void Start()
    {
        if (autoSnap)
            SnapObjectToPath();
    }

    public void SetPath(PathCreator newPathCreator)
    {
        pathCreator = newPathCreator;
    }

    public void SnapObjectToPath()
    {
        if (hasSnapped) return;
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        transform.position = TransformPoint(originalPosition);
        transform.rotation = TransformRotation(originalPosition, originalRotation);
        hasSnapped = true;
    }

    Vector3 TransformPoint(Vector3 worldPoint)
    {
        Vector3 splinePoint = GetPathPoint(worldPoint.z);
        Vector3 futureSplinePoint = GetPathPoint(worldPoint.z + 0.01f);

        Vector3 forwardVector = futureSplinePoint - splinePoint;
        Quaternion imaginaryPlaneRotation = Quaternion.LookRotation(forwardVector, Vector3.up);
        Vector3 pointWithinPlane = new Vector3(worldPoint.x, worldPoint.y, 0f);

        Vector3 newWorldVertex = splinePoint + imaginaryPlaneRotation * pointWithinPlane;
        return newWorldVertex;
    }

    Quaternion TransformRotation(Vector3 worldPoint, Quaternion originalRotation)
    {
        Vector3 splinePoint = GetPathPoint(worldPoint.z);
        Vector3 futureSplinePoint = GetPathPoint(worldPoint.z + 0.01f);

        Vector3 forwardVector = futureSplinePoint - splinePoint;
        Quaternion imaginaryPlaneRotation = Quaternion.LookRotation(forwardVector, Vector3.up);

        return originalRotation * imaginaryPlaneRotation;
    }

    Vector3 GetPathPoint(float distance)
    {
        float minDistance = 0;
        float maxDistance = path.length;
        if (distance > maxDistance)
        {
            float diff = distance - maxDistance;
            Vector3 a = path.GetPointAtDistance(maxDistance - 0.01f, EndOfPathInstruction.Stop);
            Vector3 b = path.GetPointAtDistance(maxDistance, EndOfPathInstruction.Stop);
            Vector3 forwardVector = b - a;

            return b + forwardVector * diff;
        }
        if (distance < minDistance)
        {
            float diff = distance - minDistance;
            Vector3 a = path.GetPointAtDistance(minDistance, EndOfPathInstruction.Stop);
            Vector3 b = path.GetPointAtDistance(minDistance + 0.01f, EndOfPathInstruction.Stop);
            Vector3 forwardVector = b - a;

            return a + forwardVector * diff;
        }

        return path.GetPointAtDistance(distance, EndOfPathInstruction.Stop);

    }
}
