using UnityEngine;

public class WorldSpawner : MonoBehaviour
{

    public GameObject world;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
    }

    public void SpawnWorldChunk (GameObject worldChunk)
    {
        // reparent selected chunk to world
        // instead of instantiating a new one. This is a performance optimization called pooling
        worldChunk.transform.parent = world.transform;
        worldChunk.transform.position = transform.position;
        worldChunk.transform.rotation = transform.rotation;

        GameObject worldChunkEnd = worldChunk.transform.Find("End").gameObject;
        if (worldChunkEnd != null)
        {
            transform.position = transform.TransformPoint(worldChunkEnd.transform.localPosition);
            transform.Rotate(worldChunkEnd.transform.localRotation.eulerAngles);
        }
    }
}
