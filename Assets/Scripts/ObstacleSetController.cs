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
        bool[] location = new bool[locationCount];
        int randLocation;

        for (int i = 0; i < UserData.potionNum; ++i)
        {
            // If there is enogh chance to spawn the potion
            if (Random.Range(0, 100) < 10 * 3 + 1)
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
            }
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
