using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstacle;

    private int obstacleSet = -1;

    private float timerLimit = 0.5f;    // TODO: Make it so it depends on obstacle set
    private float timer;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timerLimit)
        {
            GenerateObstacles();

            timer = 0.0f;
        }
    }

    void Setup()
    {
        Random.InitState(Player.mapLevel);
    }

    void GenerateObstacles()
    {
        if (obstacleSet == -1)
            obstacleSet = Random.Range(0, 1);

        Debug.Log(obstacleSet);

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
        Vector2 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // Generate random x for obstacle
        float obstacleX = Random.Range(-screenSize.x + 0.65f / 2.0f, screenSize.x - 0.65f / 2.0f);

        GameObject obstacleInstance = Instantiate(obstacle, new Vector3(obstacleX, screenSize.y + 2.0f, 0f), Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 5f;
        obstacleInstance.GetComponent<ObstacleController>().size = new Vector2(0.65f, 0.65f);
    }
}
