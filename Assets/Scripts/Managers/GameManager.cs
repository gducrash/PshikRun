using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Game Stats")]
    public float playerDistance = 0;
    public float initialSpeed = 5;
    public float speedMultiplier = 1;

    public float score = 0;
    public float scoreMultiplier = 1;

    [Header("Game State")]
    public bool isGameOver = false;

    float distanceDelta = 0;


    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        StartCoroutine(IncreaseDifficultyLoop());
    }

    private IEnumerator IncreaseDifficultyLoop()
    {
        for (int i = 0; i < 16; i++)
        {
            yield return new WaitForSeconds(16);
            speedMultiplier += 0.1f;
        }
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        distanceDelta = initialSpeed * speedMultiplier * Time.fixedDeltaTime;
        playerDistance += distanceDelta;
        score += distanceDelta * scoreMultiplier;
    }

    public void GameOver ()
    {
        isGameOver = true;
        Invoke("Restart", 1f);
    }

    void Restart ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
