using PathCreation;
using UnityEngine;

public class VisualPlayerController : MonoBehaviour
{
    public GameObject playerWorldChild;
    public GameObject playerWorldModel;

    public PlayerController player;
    public GameManager gameManager;
    public ChunkManager chunkManager;

    [Header("Visual Player Settings")]
    public float turnMultiplierX = 2;

    void Update()
    {
        // Player child position
        playerWorldChild.transform.localPosition = new Vector3(
            player.transform.localPosition.x,
            player.transform.localPosition.y,
            0
        );

        // Player model and animation
        playerWorldModel.transform.localRotation = Quaternion.Euler(
            0, 
            player.velocity.x * turnMultiplierX / gameManager.speedMultiplier, 
            0
        );

        // Get current chunk path and other data
        ChunkManager.ChunkData currentChunk = chunkManager.currentChunk;
        if (currentChunk == null) return;

        VertexPath path = currentChunk.obj.path;

        float localDistance = gameManager.playerDistance - currentChunk.distance;
        float localTime = localDistance / currentChunk.length;
        float pathDistance = localTime * path.length;

        Vector3 pathPoint = path.GetPointAtDistance(pathDistance, EndOfPathInstruction.Stop);
        Quaternion pathRotation = path.GetRotationAtDistance(pathDistance, EndOfPathInstruction.Stop);

        // Player basis position and rotation
        transform.position = pathPoint;
        transform.rotation = pathRotation;


        //transform.localPosition = currentChunk.obj.transform.InverseTransformPoint(pathPoint);
        //transform.localPosition = currentChunk.worldObj.transform.TransformPoint(transform.localPosition);
        //transform.localRotation = currentChunk.worldObj.transform.rotation * pathRotation;
    }
}
