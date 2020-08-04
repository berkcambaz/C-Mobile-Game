using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    public Sprite[] buttons;
    private enum BUTTON
    {
        Upgrade,
        Upgraded,
        Not_Upgraded
    }

    private Touch touch;
    private Vector3 touchPos;
    private Vector3 touchPosOld;
    private float scrollSensitivity = 2f;
    private float move;

    private Vector2 dragStartPos;
    private bool isHeld = false;
    private int pageIndex = 0;

    void Update()
    {
#if RELEASE
        // If received only 1 touch
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            touchPos = touch.position;
        }

#else

        if (Input.GetMouseButton(0))
        {
            touchPos = Input.mousePosition;
        }
#endif

        DragAndDropMovement();
        SlideControl();
    }

    void DragAndDropMovement()
    {
#if RELEASE
        // If touch has started
        if (touch.phase == TouchPhase.Began)
        {
            isHeld = true;

            dragStartPos.x = touchPos.x;
        }

        // If touch has stopped
        if (touch.phase == TouchPhase.Ended)
        {
            isHeld = false;
            isMoved = false;
        }

#else

        if (Input.GetMouseButtonDown(0))
        {
            isHeld = true;

            dragStartPos.x = touchPos.x;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHeld = false;
        }
#endif

        // If touching
        if (isHeld)
        {
            move = 0f;
            if (touchPos.x > dragStartPos.x)
                move = touchPos.x - dragStartPos.x;
            else if (touchPos.x < dragStartPos.x)
                move = touchPos.x - dragStartPos.x;

            if (!Mathf.Approximately(touchPos.x, touchPosOld.x))
            {
                touchPosOld.x = touchPos.x;
                transform.localPosition = new Vector3(transform.localPosition.x + move * scrollSensitivity, 0f, 0f);
            }
            dragStartPos = touchPos;
        }
    }

    void SlideControl()
    {
        pageIndex = Mathf.RoundToInt(transform.localPosition.x / -480f);

        // Clamp the index
        if (pageIndex < 0)
            pageIndex = 0;
        else if (pageIndex > 1)
            pageIndex = 1;

        if (!isHeld)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(pageIndex * -1920f, 0f, 0f), 0.1f);
        }
    }

    public void InitUpgrades()
    {

        UserData.potionNum = transform.GetChild(1).childCount;

        for (int potionNum = 0; potionNum < UserData.potionNum; ++potionNum)
        {
            Transform upgradeObj = transform.GetChild(1).GetChild(potionNum).GetChild(3);
            int upgradeCount = upgradeObj.childCount;

            int upgrade = 0;
            for (; upgrade < UserData.upgrades[potionNum]; ++upgrade)
            {
                // If an upgrade is unlocked set it's sprite to "upgraded" 
                upgradeObj.GetChild(upgrade).GetComponent<Image>().sprite = buttons[(int)BUTTON.Upgraded];
            }

            // Make the last button upgradeable
            if (upgrade < upgradeCount)
            {
                upgradeObj.GetChild(upgrade).GetComponent<Image>().sprite = buttons[(int)BUTTON.Upgrade];
                upgradeObj.GetChild(upgrade).GetComponent<Button>().enabled = true;
            }

            // Set the cost of potion
            int[] cost = null;
            switch (potionNum)
            {
                case 0:
                    cost = new int[3] { 250, 500, 1000 };
                    break;
                case 1:
                    cost = new int[3] { 250, 500, 1000 };
                    break;
            }
            string potionCost = cost.Length == UserData.upgrades[potionNum] ? "max\nlevel" : "-Cost-\n" + cost[UserData.upgrades[potionNum]];
            transform.GetChild(1).GetChild(potionNum).GetChild(2).GetComponent<Text>().text = potionCost;
        }
    }

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
                if (UserData.money > 249 && UserData.unlockedSkinCount != UserData.skinSprites.Length)
                {
                    UserData.money -= 250;
                    UI.Update();
                    // TODO: Animation for money going to the button
                    StartCoroutine(BuyCharacter());
                }
                else    // If player doesn't have enough money to buy skin
                {
                    // TODO: Implement
                }
                break;
        }
    }

    public void UpgradePotion(int potionNum)
    {
        int[] cost = null;
        switch (potionNum)
        {
            case 0: // Upgrade for strength potion
                cost = new int[3] { 250, 500, 1000 };
                break;
            case 1: // Upgrade for small cube potion
                cost = new int[3] { 250, 500, 1000 };
                break;
        }

        UpgradePotion(potionNum, cost);
        UI.Update();
    }

    private void UpgradePotion(int potionNum, int[] cost)
    {
        int upgradeCost = cost[UserData.upgrades[potionNum]];

        // If player doesn't have enough money, return from the function
        if (UserData.money < upgradeCost)
            return;

        // Decrease the score & update the UI
        UserData.money -= upgradeCost;
        UI.Update();

        string upgradeStr = UserData.upgrades[potionNum] == cost.Length - 1 ? "max\nlevel" : "-Cost-\n" + cost[UserData.upgrades[potionNum] + 1];

        // If player has enough money to upgrade
        if (UserData.money > upgradeCost - 1)
        {
            transform.GetChild(1).GetChild(potionNum).GetChild(3).GetChild(UserData.upgrades[potionNum]).GetComponent<Button>().enabled = false;
            transform.GetChild(1).GetChild(potionNum).GetChild(3).GetChild(UserData.upgrades[potionNum]).GetComponent<Image>().sprite = buttons[(int)BUTTON.Upgraded];

            if (++UserData.upgrades[potionNum] != transform.GetChild(1).GetChild(potionNum).GetChild(3).childCount)
            {
                transform.GetChild(1).GetChild(potionNum).GetChild(3).GetChild(UserData.upgrades[potionNum]).GetComponent<Button>().enabled = true;
                transform.GetChild(1).GetChild(potionNum).GetChild(3).GetChild(UserData.upgrades[potionNum]).GetComponent<Image>().sprite = buttons[(int)BUTTON.Upgrade];
            }

            transform.GetChild(1).GetChild(potionNum).GetChild(2).GetComponent<Text>().text = upgradeStr;
        }
    }

    private IEnumerator BuyCharacter()
    {
        // Start creating particles & shaking the camera
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
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

        // Activates skin's description
        UserData.customizables[randSelectedSkin].transform.GetChild(2).gameObject.SetActive(true);
        // Replace question mark image with skin's sprite
        UserData.customizables[randSelectedSkin].transform.GetChild(0).GetComponent<Image>().sprite = UserData.skinSprites[randSelectedSkin];
        // Hide question marks on the skin's description
        UserData.customizables[randSelectedSkin].transform.GetChild(3).gameObject.SetActive(false);


        UserData.isUnlockingCharacter = false;
    }
}
