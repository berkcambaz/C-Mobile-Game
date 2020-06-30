#define DEBUG       // To play game on pc
//#define RELEASE   // To play game on mobile, specifically android

using UnityEngine;

public class CustomizeMenu : MonoBehaviour
{
    public GameObject player;

    public GameObject[] customizables;

    private Touch touch;
    private Vector3 touchPos;

    private Vector2 dragStartPos;
    private bool isHeld = false;
    private bool isChanging = true;

    void Update()
    {
#if RELEASE
        // If received at least 1 touch
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPos = touch.position;
        }
#endif

#if DEBUG
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

            dragStartPos.y = touchPos.y - transform.localPosition.y;
        }

        // If touch has stopped
        if (touch.phase == TouchPhase.Ended)
        {
            isHeld = false;
        }
#endif

#if DEBUG
        if (Input.GetMouseButtonDown(0))
        {
            isHeld = true;

            dragStartPos.y = touchPos.y - transform.localPosition.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHeld = false;
        }
#endif

        // If touching
        if (isHeld)
        {
            transform.localPosition = new Vector3(0f, touchPos.y - dragStartPos.y, transform.position.z);
        }
    }

    void SlideControl()
    {
        if (!isChanging)
        {
            int index = Mathf.RoundToInt(transform.localPosition.y / 650f);

            // Clamp the index
            if (index < 0)
                index = 0;
            else if (index > customizables.Length - 1)
                index = customizables.Length - 1;

            UserData.selectedSkinIndex = index;
        }
        if (!isHeld)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, UserData.selectedSkinIndex * 650f, 0f), 0.1f);
            if (Mathf.RoundToInt(transform.localPosition.y / 650f) == UserData.selectedSkinIndex)
                isChanging = false;

            for (int i = 0; i < customizables.Length; ++i)
            {
                if (i == UserData.selectedSkinIndex)
                    customizables[i].transform.localScale = Vector3.Lerp(customizables[i].transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);
                else
                    customizables[i].transform.localScale = Vector3.Lerp(customizables[i].transform.localScale, new Vector3(0.85f, 0.85f, 1f), 0.1f);
            }
        }
    }

    public void Select(int index)
    {
        isChanging = true;

        // If skin is unlocked, select the skin
        if (UserData.skins[index])
        {
            customizables[UserData.playerSkinIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);  // Disable last selected one
            customizables[index].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);  // Enable newly selected one
            player.GetComponent<SpriteRenderer>().sprite = UserData.skinSprites[index]; // Change player's skin
            UserData.playerSkinIndex = index;   // Change index to selected skin's index
        }

        UserData.selectedSkinIndex = index;
    }

    public void InitSelectedSkin()
    {
        player.GetComponent<SpriteRenderer>().sprite = UserData.skinSprites[UserData.selectedSkinIndex];
        customizables[UserData.selectedSkinIndex].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        for (int i = 1; i < customizables.Length; ++i)
        {
            // If skin is unlocked
            if (UserData.skins[i])
            {
                customizables[i].transform.GetChild(2).gameObject.SetActive(true);  // Activates skin's description
                customizables[i].transform.GetChild(3).gameObject.SetActive(false); // Deactivates skin's unlock text
            }
        }
    }
}
