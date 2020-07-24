using System.Collections;
using UnityEngine;

public class MapLevelController : MonoBehaviour
{
    public static MapLevelController instance;

    private float destinationX;
    private float oldScaleX;

    void Start()
    {
        instance = this;
    }

    public IEnumerator ResetProgressBar()
    {
        while (transform.GetChild(1).transform.localScale.x != 0f)
        {
            float scaleX = transform.GetChild(1).transform.localScale.x;
            if (oldScaleX == 0f)
                oldScaleX = scaleX;

            float newScaleX = Mathf.Clamp(scaleX - oldScaleX / 37.5f, 0f, 7.5f);
            transform.GetChild(1).transform.localScale = new Vector3(newScaleX, 1.5f, 1f);

            yield return null;
        }

        transform.GetChild(1).transform.localScale = new Vector3(0f, 1.5f, 1f);
        oldScaleX = 0f;
    }

    public void ProgressBar()
    {
        destinationX = 7.5f - ObstacleManager.mapTimer / ObstacleManager.mapTimeLimit * 7.5f;
        transform.GetChild(1).transform.localScale = new Vector3(destinationX, 1.5f, 1f);
    }

    /*void Process()
    {
        if (UserData.leveledUp)
        {
            if (transform.GetChild(0).GetChild(0).transform.localScale.x == 1f)
            {
                // TODO: Particles after leveling up 
                transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(0f, 1f, 1f);

                // We are already leveled up, so set it to false
                UserData.leveledUp = false;

                // Decrease xp, increase money & level
                //UserData.exp -= UserData.level * UserData.level;
                //UserData.money += 10;
                //++UserData.level;

                // Re-update the UI
                UI.Update();
            }
            else
            {
                currentX = transform.GetChild(0).GetChild(0).transform.localScale.x;
                transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(Mathf.Clamp(currentX + speed, 0f, 1f), 1f, 1f);
                speed = Mathf.Clamp(speed - 0.001f, 0.005f, 0.01f);
            }
        }
        else if (UserData.gainedExp)
        {
            // Update the level bar according to "exp"
            currentX = transform.GetChild(0).GetChild(0).transform.localScale.x;
            //destinationX = UserData.exp / (float)(UserData.level * UserData.level);

            transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(Mathf.Clamp(currentX + speed, 0f, destinationX), 1f, 1f);
            speed = Mathf.Clamp(speed - 0.001f, 0.005f, 0.01f);

            if (transform.GetChild(0).GetChild(0).transform.localScale.x == destinationX)
            {
                UserData.gainedExp = false; // We already gained exp, so set it to false
                speed = 0.01f;              // Reset the speed of the exp bar
            }
        }
    }
    */
}