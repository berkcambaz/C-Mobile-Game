using UnityEngine;

public class ObstacleSetController : MonoBehaviour
{
    // The child that's the further away from the start pos
    public Transform child;

    private bool isReset = false;

    void Update()
    {
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
