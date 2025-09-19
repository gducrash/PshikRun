using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameManager gameManager;
    public WorldSpawner worldSpawner;
    public GameObject chunkPoolObj;
    public GameObject gameChunkContainer;

    [Header("Chunk Settings")]
    public float chunkLookaheadDistance = 50;
    public float chunkDespawnDistance = 5;

    [HideInInspector]
    public ChunkData currentChunk = null;

    [System.Serializable]
    public class ChunkData
    {
        public ChunkObject obj;
        public GameObject worldObj;
        public float distance; // The world Z position where the chunk begins
        public float length;   // Z length of the chunk
        public int idx; // Which chunk in the sequence
    }

    private ChunkObject[] chunkPool = {};
    private List<ChunkData> spawnedChunks = new List<ChunkData>();

    private float lastChunkEndDistance = 0;
    private int chunkCount = 0;

    void Start()
    {
        chunkPoolObj.SetActive(true);
        chunkPool = chunkPoolObj.GetComponentsInChildren<ChunkObject>();
        foreach (ChunkObject chunk in chunkPool)
        {
            chunk.Setup();
        }
        chunkPoolObj.SetActive(false);
    }

    void FixedUpdate()
    {
        if (gameManager.isGameOver) return;

        gameChunkContainer.transform.localPosition = new Vector3(0, 0, -gameManager.playerDistance);
        if (gameManager.playerDistance >= lastChunkEndDistance - chunkLookaheadDistance)
        {
            SpawnNewChunk();
        }
        if (spawnedChunks.Count > 0)
        {
            ChunkData firstChunk = spawnedChunks[0];
            if (gameManager.playerDistance > firstChunk.distance + firstChunk.length + chunkDespawnDistance)
            {
                DespawnChunk(firstChunk);
            }
        }
        UpdateCurrentChunk();
    }

    private void UpdateCurrentChunk()
    {
        for (int i = 0; i < spawnedChunks.Count; i++)
        {
            ChunkData chunk = spawnedChunks[i];
            if (
                gameManager.playerDistance >= chunk.distance && 
                gameManager.playerDistance < chunk.distance + chunk.length
            )
            {
                currentChunk = chunk;
                break;
            }
        }
    }

    private void SpawnNewChunk()
    {
        ChunkData newChunk = new ChunkData();

        ChunkObject newChunkObj = chunkPool[Random.Range(0, chunkPool.Length)];
        while (newChunkObj.isInUse)
        {
            newChunkObj = chunkPool[Random.Range(0, chunkPool.Length)];
        }

        newChunk.obj = newChunkObj;
        newChunk.idx = chunkCount;
        newChunk.distance = lastChunkEndDistance;
        newChunk.length = newChunk.obj.length;
        spawnedChunks.Add(newChunk);
        newChunkObj.Spawn();

        lastChunkEndDistance += newChunk.length;
        chunkCount++;

        // reparent selected chunk to gameChunkContainer
        // instead of instantiating a new one. This is a performance optimization called pooling
        ChunkObject obj = newChunk.obj;
        obj.gameObject.transform.parent = gameChunkContainer.transform;

        obj.transform.localPosition = new Vector3(0, 0, newChunk.distance);
        obj.transform.localRotation = Quaternion.identity;

        worldSpawner.SpawnWorldChunk(obj.worldChunk);
        newChunk.worldObj = obj.worldChunk;
    }

    private void DespawnChunk(ChunkData chunk)
    {
        chunk.obj.Despawn();
        spawnedChunks.Remove(chunk);
    }
}
