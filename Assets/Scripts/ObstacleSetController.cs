using UnityEngine;

public class ObstacleSetController : MonoBehaviour
{
    // The child that's the further away from the start pos
    public Transform child;

    private const float speed = 5f;

    private bool isReset = false;

    void Start()
    {
        int[] randState = Rand.GetState();  // Get the current state
        int locationCount = transform.GetChild(1).childCount;
        int occupiedLocationCount = 0;
        bool[] location = new bool[locationCount];
        int randLocation;

        for (int i = 0; i < UserData.potionNum && i < locationCount; ++i)
        {
            // If there is enogh chance to spawn the potion
            if (Random.Range(0, 100) < UserData.upgrades[i] * 5)
            {
                randLocation = Random.Range(0, 100);
                randLocation %= locationCount;

                // If location is occupied, try to find a empty location
                if (location[randLocation])
                {
                    bool foundLocation = false;
                    for (int loc = randLocation; loc < locationCount; ++loc)    // Search forward
                        if (!location[loc]) // If found empty location, set it
                        {
                            randLocation = loc;
                            foundLocation = true;
                        }

                    if (!foundLocation) // If couldn't find an empty location by searching forward,search backward
                        for (int loc = randLocation; loc > locationCount; --loc)    // Search backward
                            if (!location[loc]) // If found empty location, set it
                                randLocation = loc;
                }

                location[randLocation] = true;  // Occupy the location

                // Change the potions sprite & create the potion
                Instantiate(ObstacleManager.instance.potions[i], transform.GetChild(1).GetChild(randLocation).position, transform.rotation, transform);
                ++occupiedLocationCount;
            }
        }

        locationCount -= occupiedLocationCount;
        occupiedLocationCount = 0;

        int moneyCount = Random.Range(0, (UserData.moneyToSpawn % 5) + 3);

        // Always generate some money for player to pick-up (at least generate 2 money)
        if (moneyCount < 2 && UserData.moneyToSpawn > 0)
            moneyCount = UserData.moneyToSpawn < 2 ? UserData.moneyToSpawn : 2;

        for (int i = 0; i < moneyCount && i < locationCount && i < UserData.moneyToSpawn; ++i)
        {
            randLocation = Random.Range(0, 100);
            randLocation %= locationCount;

            // If location is occupied, try to find a empty location
            if (location[randLocation])
            {
                bool foundLocation = false;
                for (int loc = randLocation; loc < locationCount; ++loc)    // Search forward
                    if (!location[loc]) // If found empty location, set it
                    {
                        randLocation = loc;
                        foundLocation = true;
                    }

                if (!foundLocation) // If couldn't find an empty location by searching forward,search backward
                    for (int loc = randLocation; loc > locationCount; --loc)    // Search backward
                        if (!location[loc]) // If found empty location, set it
                            randLocation = loc;
            }

            location[randLocation] = true;  // Occupy the location

            // Place the coin
            Instantiate(ObstacleManager.instance.money, transform.GetChild(1).GetChild(randLocation).position, transform.rotation, transform);
        }

        Rand.SetState(randState);       // Set the current state back
    }

    void Update()
    {
        transform.Translate(0f, -speed * Time.deltaTime, 0f);

        if (!isReset && child != null)
        {
            if (child.position.y < 0)
            {
                UserData.isObstacleSetFinished = true;
                isReset = true;
            }
        }
    }
}
