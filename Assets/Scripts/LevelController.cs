using System.Security.Cryptography;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private float destinationX;
    private float currentX;
    private float speed = 0.01f;

    void Update()
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
                UserData.exp -= UserData.level * UserData.level;
                UserData.money += 10;
                ++UserData.level;

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
            destinationX = UserData.exp / (float)(UserData.level * UserData.level);

            transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(Mathf.Clamp(currentX + speed, 0f, destinationX), 1f, 1f);
            speed = Mathf.Clamp(speed - 0.001f, 0.005f, 0.01f);

            if (transform.GetChild(0).GetChild(0).transform.localScale.x == destinationX)
            {
                UserData.gainedExp = false; // We already gained exp, so set it to false
                speed = 0.01f;              // Reset the speed of the exp bar
            }
        }
    }
}