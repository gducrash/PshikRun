using PathCreation;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
public class MeshPathDeformer : MonoBehaviour
{
    public PathCreator pathCreator;
    public bool autoDeform = false;

    VertexPath path => pathCreator.path;
    private Mesh originalMesh;
    private Mesh deformedMesh;

    void Awake()
    {
        originalMesh = GetComponent<MeshFilter>().sharedMesh;
    }


    void Start()
    {
        if (autoDeform)
            DeformMesh();
    }

    void GetMesh() => deformedMesh = Instantiate(originalMesh);

    public void SetPath(PathCreator newPathCreator)
    {
        pathCreator = newPathCreator;
    }

    public void DeformMesh()
    {
        GetMesh();

        Vector3[] vertices = deformedMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = TransformVertex(vertices[i]);

        deformedMesh.vertices = vertices;
        deformedMesh.RecalculateNormals();
        deformedMesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh = deformedMesh;
    }

    Vector3 TransformVertex(Vector3 meshVertex)
    {
        Vector3 worldVertex = transform.TransformPoint(meshVertex);
        worldVertex = pathCreator.transform.InverseTransformPoint(worldVertex);

        Vector3 splinePoint = GetPathPoint(worldVertex.z);
        Vector3 futureSplinePoint = GetPathPoint(worldVertex.z + 0.01f);

        Vector3 forwardVector = futureSplinePoint - splinePoint;
        Quaternion imaginaryPlaneRotation = Quaternion.LookRotation(forwardVector, Vector3.up);
        Vector3 pointWithinPlane = new Vector3(worldVertex.x, worldVertex.y, 0f);

        Vector3 newWorldVertex = splinePoint + imaginaryPlaneRotation * pointWithinPlane;
        return transform.InverseTransformPoint(newWorldVertex);
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
