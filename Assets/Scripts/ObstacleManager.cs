using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject particle;

    Vector2 screenSize;

    private const int maxObstacleSet = 4;
    private int obstacleSet = -1;

    public static float mapTimeLimit;   // Time until player finishes a level
    private float obstacleSetTimeLimit; // Time until the obstacle set finishes
    private float obstacleTimeLimit;    // Time until a new obstacle appears
    private float timer;
    private bool removeObstacles = false;

    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (UserData.isPlaying)
        {

            if (UserData.isAlive)
            {
                // If player has finished the level
                if (mapTimeLimit <= 0f)
                {
                    if (!removeObstacles)
                    {
                        removeObstacles = true;
                        timer = 0.0f;
                        obstacleSetTimeLimit = 0f;

                        ++UserData.mapLevel;
                        DeleteObstacles();  // Destroy obstacles when level is finished
                    }
                    if (timer > 1.5f) // Wait some time after the level is finished, then destroy the player
                    {
                        Destroy(GameObject.Find("Player(Clone)"));

                        UserData.isPlaying = false; // Player is not playing anymore

                        // Update customizables 
                        CustomizeMenu.CheckSkinUnlockFromMapLevel();

                        // Open main menu & update score
                        UI.upgradesButton.SetActive(true);
                        UI.customizeButton.SetActive(true);
                        UI.mainMenu.SetActive(true);
                        UI.Update();
                    }
                }
                else // If player hasn't finished the level, but still alive
                {
                    removeObstacles = false;

                    // If ready to instantiate a new obstacle, instantiate obstacle
                    if (timer > obstacleTimeLimit)
                    {
                        GenerateObstacles();

                        obstacleSetTimeLimit -= obstacleTimeLimit;
                        timer -= obstacleTimeLimit;
                    }

                    // If current obstacle set is finished, generate new one
                    if (obstacleSetTimeLimit <= 0f)
                    {
                        GenerateObstacleSet();
                    }
                }
            }
            else // If player is dead
            {
                if (!removeObstacles)
                {
                    removeObstacles = true;
                    timer = 0f;
                    obstacleSetTimeLimit = 0f;
                }
                if (timer > 0.5f)  // Wait some time after the player is dead, then destroy obstacles
                {
                    UserData.isPlaying = false; // Player is not playing anymore

                    DeleteObstacles();

                    removeObstacles = false;    // Obstacles are deleted, so set it to "false"

                    // Update customizables 
                    CustomizeMenu.CheckSkinUnlockFromMapLevel();

                    // Open main menu & update score
                    UI.upgradesButton.SetActive(true);
                    UI.customizeButton.SetActive(true);
                    UI.mainMenu.SetActive(true);
                    UI.Update();
                }
            }
        }
        else if (!UserData.isAlive)
        {
            timer = 0f;
        }
    }

    void DeleteObstacles()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            GameObject particleInstance = Instantiate(particle, child.transform.position, child.transform.rotation);

            // Set particle color to red, because obstacles are red
            ParticleSystem.MainModule psmain = particleInstance.GetComponent<ParticleSystem>().main;
            psmain.maxParticles = UserData.particles;
            psmain.startColor = Color.red;

            Destroy(child.gameObject);
        }
    }

    public static void SetupMapLevel()
    {
        // Set the random seed to player's map level, so every level is
        // different but a level in different phones are same
        Rand.Init();
        Rand.InitState(UserData.mapLevel);

        // Set map time limit,
        // when map level is   0, time is 10 seconds,
        // when map level is 100, time is 20 seconds
        float timeLimit = 10f + UserData.mapLevel * 0.1f;
        mapTimeLimit = (timeLimit > 20f) ? 20f : timeLimit;
    }

    private void SetObstacleTimers(float _setTimeLimit, float _timeLimit)
    {
        if (mapTimeLimit >= _setTimeLimit)
        {
            mapTimeLimit -= _setTimeLimit;
            obstacleSetTimeLimit = _setTimeLimit;
        }
        else
        {
            mapTimeLimit = 0f;
            obstacleSetTimeLimit = (mapTimeLimit - _setTimeLimit > 0f) ? mapTimeLimit - _setTimeLimit : 0f;
        }

        obstacleTimeLimit = _timeLimit;
    }

    void GenerateObstacleSet()
    {
        obstacleSet = Rand.Range(0, maxObstacleSet);

        switch (obstacleSet)
        {
            case 0: // Randomized
                SetObstacleTimers(2f, 0.5f);
                break;
            case 1: // Double tubes
                SetObstacleTimers(2f, 0.5f);
                break;
            case 2: // Long obstacles
                SetObstacleTimers(3f, 0.75f);
                break;
            case 3: // Wide obstacles
                SetObstacleTimers(3f, 0.75f);
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
            case 2:
                LongObstacles();
                break;
            case 3:
                WideObstacles();
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
        float obstacleX = Rand.Range(-screenSize.x + obstacleSize.x / 2f, screenSize.x - obstacleSize.y / 2f);

        // Set obstacle position
        obstaclePos.x = obstacleX;
        obstaclePos.y = screenSize.y + 2f + (obstacleSize.y / 2f);
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

        float gap = 2.6f;
        float minLength = 0.1f;
        float maxLength = screenSize.x * 2f - gap - minLength;

        // Generate random size to obstacles
        // Gap between 2 obstcles is 1.5f (minLength + maxLength - screenSize.x)
        obstacleSize[0].x = Rand.Range(minLength, maxLength);
        obstacleSize[1].x = minLength + maxLength - obstacleSize[0].x;

        obstacleSize[0].y = 0.625f;
        obstacleSize[1].y = 0.625f;

        // Generate obstacle positions
        obstaclePos[0].x = -screenSize.x + obstacleSize[0].x / 2.0f;
        obstaclePos[1].x = +screenSize.x - obstacleSize[1].x / 2.0f;

        obstaclePos[0].y = screenSize.y + 2.0f + (obstacleSize[0].y / 2f);
        obstaclePos[1].y = screenSize.y + 2.0f + (obstacleSize[1].y / 2f);
        obstaclePos[0].z = 0f;
        obstaclePos[1].z = 0f;

        // Instantiate obstacles
        GameObject obstacleInstance = Instantiate(obstacle, obstaclePos[0], Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 5f;
        obstacleInstance.GetComponent<ObstacleController>().size = obstacleSize[0];

        obstacleInstance = Instantiate(obstacle, obstaclePos[1], Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 5f;
        obstacleInstance.GetComponent<ObstacleController>().size = obstacleSize[1];
    }

    /* Normal obstacles but they are longer */
    void LongObstacles()
    {
        Vector3 obstaclePos;
        Vector2 obstacleSize = new Vector2(0.65f, 2.6f);

        // Generate random x for the obstacle
        float obstacleX = Rand.Range(-screenSize.x + obstacleSize.x / 2f, screenSize.x - obstacleSize.y / 2f);

        // Set obstacle position
        obstaclePos.x = obstacleX;
        obstaclePos.y = screenSize.y + 2f + (obstacleSize.y / 2f);
        obstaclePos.z = 0f;

        // Instantiate obstacle
        GameObject obstacleInstance = Instantiate(obstacle, obstaclePos, Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 5f;
        obstacleInstance.GetComponent<ObstacleController>().size = obstacleSize;
    }

    /* Normal obstacles but they are wider */
    void WideObstacles()
    {
        Vector3 obstaclePos;
        Vector2 obstacleSize = new Vector2(2.6f, 0.65f);

        // Generate random x for the obstacle
        float obstacleX = Rand.Range(-screenSize.x + obstacleSize.x / 2f, screenSize.x - obstacleSize.y / 2f);

        // Set obstacle position
        obstaclePos.x = obstacleX;
        obstaclePos.y = screenSize.y + 2f + (obstacleSize.y / 2f);
        obstaclePos.z = 0f;

        // Instantiate obstacle
        GameObject obstacleInstance = Instantiate(obstacle, obstaclePos, Quaternion.Euler(0f, 0f, 0f), transform);
        obstacleInstance.GetComponent<ObstacleController>().speed = 5f;
        obstacleInstance.GetComponent<ObstacleController>().size = obstacleSize;
    }
}
