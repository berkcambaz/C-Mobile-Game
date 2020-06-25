using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstacle;

    Vector2 screenSize;

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
        obstacleSet = Random.Range(0, 1);
        switch (obstacleSet)
        {
            case 0:
                Debug.Log("new obstacle set");
                obstacleSetTimeLimit = 5.0f;
                obstacleTimeLimit = 0.5f;
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
        }
    }

    // --- OBSTACLE SETS --- //

    void Randomized()
    {
        // Generate random x for obstacle
        float obstacleX = Random.Range(-screenSize.x + 0.65f / 2.0f, screenSize.x - 0.65f / 2.0f);

        GameObject obstacleInstance = Instantiate(obstacle, new Vector3(obstacleX, screenSize.y + 2.0f, 0f), Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 5f;
        obstacleInstance.GetComponent<ObstacleController>().size = new Vector2(0.65f, 0.65f);
    }
}
