using PathCreation;
using UnityEngine;

public class ChunkObject : MonoBehaviour
{
    public PathCreator pathCreator;
    public VertexPath path => pathCreator.path;
    public GameObject worldChunk;
    public bool isInUse = false;

    public float length => pathCreator.path.length;

    Collectable[] collectables = {};

    public void Setup()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;

        GameObject worldChunk = this.worldChunk;

        worldChunk.transform.position = Vector3.zero;
        worldChunk.transform.rotation = Quaternion.identity;

        SeparateSkin[] objectsWithSkin = gameObject.GetComponentsInChildren<SeparateSkin>();
        foreach (SeparateSkin obj in objectsWithSkin)
        {
            obj.skin.transform.parent = worldChunk.transform;
        }

        MeshPathDeformer[] deformers = worldChunk.GetComponentsInChildren<MeshPathDeformer>();
        foreach (MeshPathDeformer deformer in deformers)
        {
            deformer.SetPath(pathCreator);
            deformer.DeformMesh();
        }

        PropPathFollower[] followers = worldChunk.GetComponentsInChildren<PropPathFollower>();
        foreach (PropPathFollower follower in followers)
        {
            follower.SetPath(pathCreator);
            follower.SnapObjectToPath();
        }

        collectables = gameObject.GetComponentsInChildren<Collectable>();
        foreach (Collectable collectable in collectables)
        {
            collectable.Setup();
        }
    }

    public void PerSpawnSetup()
    {
        foreach (Collectable collectable in collectables)
        {
            collectable.Reset();
        }
    }

    public void Spawn()
    {
        PerSpawnSetup();
        isInUse = true;
        gameObject.SetActive(true);
        worldChunk.SetActive(true);
    }

    public void Despawn()
    {
        isInUse = false;
        gameObject.SetActive(false);
        worldChunk.SetActive(false);
    }
}
