using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterMachine : MonoBehaviour
{
    public void ButtonClick(int button)
    {
        UserData.isUnlockingCharacter = true;
        switch (button)
        {
            case 0: // Buy with ad
                // TODO: Implement
                break;
            case 1: // Buy with money
                // If player has enough money & all skins are not unlocked
                if (UserData.money > 99 && UserData.unlockedSkinCount != UserData.skinSprites.Length)
                {
                    UserData.money -= 100;
                    UI.Update();
                    // TODO: Animation for money going to the button
                    StartCoroutine(BuyCharacter());
                }
                break;
        }
    }

    private IEnumerator BuyCharacter()
    {
        // Start creating particles & shaking the camera
        transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
        StartCoroutine(Utility.CameraShake(1f, 0.1f));

        yield return new WaitForSeconds(1f);

        // Shake the camera & create the newly unlocked character
        StartCoroutine(Utility.CameraShake(0.25f, 0.5f));
        UnlockRandomSkin();
    }

    private void UnlockRandomSkin()
    {
        int randSelectedSkin = Random.Range(0, UserData.skinSprites.Length);

        // If character is already unlocked
        if (UserData.skins[randSelectedSkin])
        {
            bool foundUnlockedSkin = false;
            for (int i = randSelectedSkin; i < UserData.skinSprites.Length; ++i)
                if (!UserData.skins[i])
                {
                    randSelectedSkin = i;
                    foundUnlockedSkin = true;
                }

            // If haven't found the unlocked skin search for it
            if (!foundUnlockedSkin)
                for (int i = randSelectedSkin; i > UserData.skinSprites.Length; --i)
                    if (!UserData.skins[i])
                        randSelectedSkin = i;
        }

        // Unlock the character
        UserData.skins[randSelectedSkin] = true;
        UserData.lastOpenedSkin = randSelectedSkin; // Set the last opened skin to current opened skin

        UI.notification.SetActive(true);

        UserData.customizables[randSelectedSkin].transform.GetChild(2).gameObject.SetActive(true);  // Activates skin's description
        UserData.customizables[randSelectedSkin].transform.GetChild(3).gameObject.SetActive(false); // Deactivates skin's unlock text
        // Replace question mark image with skin's sprite
        UserData.customizables[randSelectedSkin].transform.GetChild(0).GetComponent<Image>().sprite = UserData.skinSprites[randSelectedSkin];

        UserData.isUnlockingCharacter = false;
    }
}
