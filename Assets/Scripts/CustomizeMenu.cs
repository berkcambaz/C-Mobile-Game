using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenu : MonoBehaviour
{
    public GameObject player;

    public Sprite questionMark;

    private Touch touch;
    private Vector3 touchPos;
    private Vector3 touchPosOld;
    private float scrollSensitivity = 2f;
    private float move;

    private Vector2 dragStartPos;
    private bool isHeld = false;
    private bool isMoved = false;

    private bool isChanging = true;
    private int skinIndex = 0;

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

            dragStartPos.y = touchPos.y;
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

            dragStartPos.y = touchPos.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHeld = false;
            isMoved = false;
        }
#endif

        // If touching
        if (isHeld)
        {
            move = 0f;
            if (touchPos.y > dragStartPos.y)
                move = touchPos.y - dragStartPos.y;
            else if (touchPos.y < dragStartPos.y)
                move = touchPos.y - dragStartPos.y;

            if (move != 0f)
                isMoved = true;


            if (!Mathf.Approximately(touchPos.y, touchPosOld.y))
            {
                touchPosOld.y = touchPos.y;
                transform.localPosition = new Vector3(0f, transform.localPosition.y + move * scrollSensitivity, 0f);
            }
            dragStartPos = touchPos;
        }
    }

    void SlideControl()
    {
        if (!isChanging)
        {
            skinIndex = Mathf.RoundToInt(transform.localPosition.y / 650f);

            // Clamp the index
            if (skinIndex < 0)
                skinIndex = 0;
            else if (skinIndex > UserData.customizables.Length - 1)
                skinIndex = UserData.customizables.Length - 1;

            UserData.selectedSkinIndex = skinIndex;
        }
        if (!isHeld)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, UserData.selectedSkinIndex * 650f, 0f), 0.1f);
            if (Mathf.RoundToInt(transform.localPosition.y / 650f) == UserData.selectedSkinIndex)
                isChanging = false;

            for (int i = 0; i < UserData.customizables.Length; ++i)
            {
                if (i == UserData.selectedSkinIndex)
                    UserData.customizables[i].transform.localScale = Vector3.Lerp(UserData.customizables[i].transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);
                else
                    UserData.customizables[i].transform.localScale = Vector3.Lerp(UserData.customizables[i].transform.localScale, new Vector3(0.85f, 0.85f, 1f), 0.1f);
            }
        }
    }

    public void Select(int index)
    {
        if (!isMoved)
        {
            isChanging = true;

            // If skin is unlocked, select the skin
            if (UserData.skins[index])
            {
                UserData.customizables[UserData.playerSkinIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);  // Disable last selected one
                UserData.customizables[index].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);  // Enable newly selected one
                player.GetComponent<SpriteRenderer>().sprite = UserData.skinSprites[index]; // Change player's skin
                UserData.playerSkinIndex = index;   // Change index to selected skin's index
            }
            else if (skinIndex == index)
                SoundManager.PlaySound("cantSelect");

            UserData.selectedSkinIndex = index;
        }
    }

    public void InitSkins()
    {
        player.GetComponent<SpriteRenderer>().sprite = UserData.skinSprites[UserData.selectedSkinIndex];
        UserData.customizables[UserData.selectedSkinIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        for (int i = 1; i < UserData.customizables.Length; ++i)
        {
            // If skin is unlocked
            if (UserData.skins[i])
            {
                UserData.customizables[i].transform.GetChild(2).gameObject.SetActive(true);  // Activates skin's description
                UserData.customizables[i].transform.GetChild(3).gameObject.SetActive(false); // Deactivates skin's unlock text
            }
            else
            {
                // Hide skin by making it's image question mark
                UserData.customizables[i].transform.GetChild(0).GetComponent<Image>().sprite = questionMark;
            }
        }
    }

    /*
    public static void CheckSkinUnlockFromMapLevel()
    {
        int skinIndex = -1;
        switch (UserData.mapLevel)
        {
            case 5:
                skinIndex = 1;
                break;
            case 10:
                skinIndex = 2;
                break;
            case 25:
                skinIndex = 3;
                break;
            case 50:
                skinIndex = 4;
                break;
            case 100:
                skinIndex = 5;
                break;
            case 150:
                skinIndex = 6;
                break;
            case 200:
                skinIndex = 7;
                break;
            case 250:
                skinIndex = 8;
                break;
        }

        // If skin index is not -1 & the skin is not unlocked
        if (skinIndex != -1 && !UserData.skins[skinIndex])
        {
            // Activate skin from user data
            UserData.skins[skinIndex] = true;
            UserData.lastOpenedSkin = skinIndex;

            // Activate skin's image
            UserData.customizables[skinIndex].transform.GetChild(0).GetComponent<Image>().sprite = UserData.skinSprites[skinIndex];
            // Activates skin's description
            UserData.customizables[skinIndex].transform.GetChild(2).gameObject.SetActive(true);
            // Deactivates skin's unlock text
            UserData.customizables[skinIndex].transform.GetChild(3).gameObject.SetActive(false);

            // Send a notification
            UI.notification.SetActive(true);
        }
    }
    */
}
