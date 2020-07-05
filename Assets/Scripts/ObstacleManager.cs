using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstacleSets;

    public GameObject particle;
    public GameObject money;

    private Vector2 screenSize;

    private int obstacleSetNumber;
    private int obstacleSet = -1;

    public static float mapTimeLimit;   // Time until player finishes a level
    private float timer;
    private bool removeObstacles = false;

    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
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
                if (mapTimeLimit < 0f)
                {
                    if (!removeObstacles)
                    {
                        removeObstacles = true;
                        timer = 0.0f;
                        UserData.isObstacleSetFinished = true;

                        UserData.exp += UserData.mapLevel;
                        ++UserData.mapLevel;

                        UserData.gainedExp = true;

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

                        UI.levelContent.SetActive(true);
                        UI.Update();
                    }
                }
                else // If player hasn't finished the level, but still alive
                {
                    removeObstacles = false;

                    mapTimeLimit -= Time.deltaTime;

                    // If current obstacle set is finished, generate new one
                    if (UserData.isObstacleSetFinished)
                        GenerateObstacleSet();
                }
            }
            else // If player is dead
            {
                if (!removeObstacles)
                {
                    removeObstacles = true;
                    timer = 0f;
                    UserData.isObstacleSetFinished = true;
                }
                if (timer > 0.5f)  // Wait some time after the player is dead, then destroy obstacles
                {
                    UserData.isPlaying = false; // Player is not playing anymore

                    DeleteObstacles();

                    removeObstacles = false;    // Obstacles are deleted, so set it to "false"

                    // Open main menu & update score
                    UI.upgradesButton.SetActive(true);
                    UI.customizeButton.SetActive(true);
                    UI.mainMenu.SetActive(true);

                    UI.levelContent.SetActive(true);
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
