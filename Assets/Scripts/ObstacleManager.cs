using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstacle;

    Vector2 screenSize;

    private const int maxObstacleSet = 2;
    private int obstacleSet = -1;

    private float obstacleSetTimeLimit; // Time until the obstacle set finishes
    private float obstacleTimeLimit;    // Time until a new obstacle appears
    private float timer;

    void Start()
    {
        Setup();
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        timer += Time.deltaTime;
        obstacleSetTimeLimit -= Time.deltaTime;

        if (obstacleSetTimeLimit < 0f)
        {
            GenerateObstacleSet();
        }

        if (timer > obstacleTimeLimit)
        {
            GenerateObstacles();

            timer = 0.0f;
        }
    }

    void Setup()
    {
        Random.InitState(Player.mapLevel);
    }

    void GenerateObstacleSet()
    {
        // TODO: Use const max number of obstacle sets instead of "1"
        obstacleSet = Random.Range(0, maxObstacleSet);  //
        switch (obstacleSet)
        {
            case 0:
                obstacleSetTimeLimit = 10.0f;
                obstacleTimeLimit = 0.5f;
                break;
            case 1:
                obstacleSetTimeLimit = 5.0f;
                obstacleTimeLimit = 0.6f;
                break;
        }
    }

    void GenerateObstacles()
    {
        switch (obstacleSet)
        {
            case 0:
                Randomized();
                break;
            case 1:
                DoubleTubes();
                break;
        }
    }

    // --- OBSTACLE SETS --- //

    /* Random obstacles appear */
    void Randomized()
    {
        Vector3 obstaclePos;
        Vector2 obstacleSize = new Vector2(0.65f, 0.65f);

        // Generate random x for the obstacle
        float obstacleX = Random.Range(-screenSize.x + obstacleSize.x / 2.0f, screenSize.x - obstacleSize.y / 2.0f);

        // Set obstacle position
        obstaclePos.x = obstacleX;
        obstaclePos.y = screenSize.y + 2.0f;
        obstaclePos.z = 0f;

        // Instantiate obstacle
        GameObject obstacleInstance = Instantiate(obstacle, obstaclePos, Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 5f;
        obstacleInstance.GetComponent<ObstacleController>().size = obstacleSize;
    }

    /* 2 random length tube obstacle appear */
    void DoubleTubes()
    {
        Vector3[] obstaclePos = new Vector3[2];
        Vector2[] obstacleSize = new Vector2[2];

        float gap = 0.35f;
        float minLength = 0.1f;
        float maxLength = screenSize.x - gap + minLength;

        // Generate random size to obstacles
        // Gap between 2 obstcles is 1.5f (minLength + maxLength - screenSize.x)
        obstacleSize[0].x = Random.Range(minLength, maxLength);
        obstacleSize[1].x = minLength + maxLength - obstacleSize[0].x;

        obstacleSize[0].y = 0.625f;
        obstacleSize[1].y = 0.625f;

        // Generate obstacle positions
        obstaclePos[0].x = -screenSize.x + obstacleSize[0].x / 2.0f;
        obstaclePos[1].x = +screenSize.x - obstacleSize[1].x / 2.0f;

        obstaclePos[0].y = screenSize.y + 2.0f;
        obstaclePos[1].y = screenSize.y + 2.0f;
        obstaclePos[0].z = 0f;
        obstaclePos[1].z = 0f;

        // Instantiate obstacles
        GameObject obstacleInstance = Instantiate(obstacle, obstaclePos[0], Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 6f;
        obstacleInstance.GetComponent<ObstacleController>().size = obstacleSize[0];

        obstacleInstance = Instantiate(obstacle, obstaclePos[1], Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 6f;
        obstacleInstance.GetComponent<ObstacleController>().size = obstacleSize[1];
    }
}
