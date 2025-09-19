using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    public PlayerController player;

    [Header("Camera Settings")]
    public float damping = 5;
    public Vector3 multiplier = new Vector3(0.5f, 1, 1);
    public float midairYMultiplier = 0.5f;

    Vector3 initialPosition;
    float elevation = 1;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        Vector3 playerPosition = player.transform.localPosition;

        float playerYDiffToElevation = playerPosition.y - elevation;

        Vector3 targetPosition = new Vector3(
            playerPosition.x,
            elevation + playerYDiffToElevation * midairYMultiplier,
            playerPosition.z
        );

        if (playerPosition.y < elevation || player.isOnFloor)
            elevation = playerPosition.y;

        Vector3 targetPositionAdj = Vector3.Scale(targetPosition, multiplier) + initialPosition + Vector3.down;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPositionAdj, damping * Time.fixedDeltaTime);
    }
}
