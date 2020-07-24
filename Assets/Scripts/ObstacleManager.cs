using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstacleSets;

    public GameObject particle;
    public GameObject money;

    private int obstacleSetNumber;
    private int obstacleSet = -1;

    public static float mapTimeLimit;   // Time until player finishes a level
    public static float mapTimer;
    private float timer;
    private bool removeObstacles = false;

    void Start()
    {
        obstacleSetNumber = obstacleSets.Length;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (UserData.isPlaying)
        {
            if (UserData.isAlive)
            {
                // If player has finished the level
                if (mapTimer < 0f)
                {
                    if (!removeObstacles)
                    {
                        StartCoroutine(HUDController.instance.ResetProgressBar());
                        removeObstacles = true;
                        timer = 0.0f;
                        UserData.isObstacleSetFinished = true;

                        ++UserData.mapLevel;

                        DeleteObstacles();  // Destroy obstacles when level is finished
                    }
                    if (timer > 1.5f) // Wait some time after the level is finished, then destroy the player
                    {
                        SoundManager.PlaySound("playerExplosion");
                        Destroy(GameObject.Find("Player(Clone)"));

                        UserData.isPlaying = false; // Player is not playing anymore

                        // Update customizables 
                        //CustomizeMenu.CheckSkinUnlockFromMapLevel();

                        UI.LevelEnded();
                    }
                }
                else // If player hasn't finished the level, but still alive
                {
                    removeObstacles = false;

                    HUDController.instance.UpdateProgressBar();  // Update progress bar
                    mapTimer -= Time.deltaTime;

                    // If current obstacle set is finished, generate new one
                    if (UserData.isObstacleSetFinished)
                        GenerateObstacleSet();
                }
            }
            else // If player is dead
            {
                if (!removeObstacles)
                {
                    StartCoroutine(HUDController.instance.ResetProgressBar());
                    removeObstacles = true;
                    timer = 0f;
                    UserData.isObstacleSetFinished = true;

                    SoundManager.PlaySound("explosion");
                    StartCoroutine(Utility.CameraShake(0.5f, 0.1f));    // Shake the camera
                }
                if (timer > 0.5f)  // Wait some time after the player is dead, then destroy obstacles
                {
                    UserData.isPlaying = false; // Player is not playing anymore

                    DeleteObstacles();

                    removeObstacles = false;    // Obstacles are deleted, so set it to "false"

                    UI.LevelEnded();
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
        SoundManager.PlaySound("obstacleExplosion");
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);

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
        // when map level is   0, time is 15 seconds,
        // when map level is 50, time is 20 seconds
        float timeLimit = 15f + UserData.mapLevel * 0.1f;
        mapTimeLimit = (timeLimit > 20f) ? 20f : timeLimit;
        mapTimer = mapTimeLimit;
    }

    void GenerateObstacleSet()
    {
        // Generate a random obstacle set number
        obstacleSet = Rand.Range(0, obstacleSetNumber);

        // Since a new obstacle set is going to start, set it to "false"
        UserData.isObstacleSetFinished = false;

        // Generate obstacle set
        Instantiate(obstacleSets[obstacleSet], transform);
    }
}
