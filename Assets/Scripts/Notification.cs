using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public GameObject content;
    public Image skin;
    public Text text;

    private float fadeTime;

    void Update()
    {
        fadeTime -= Time.deltaTime;

        // Move the contents to the middle of the screen
        content.transform.localPosition = Vector3.Lerp(content.transform.localPosition, new Vector3(0f, 0f, 0f), 0.1f);

        Color color = new Color(1f, 1f, 1f, fadeTime);
        skin.color = color;
        text.color = color;

        if (fadeTime <= 0f)
        {
            gameObject.SetActive(false);
            UserData.isUnlockingCharacter = false;
        }
    }

    void OnEnable()
    {
        // Set fade time
        fadeTime = 3f;

        // Set sprite of the skin
        skin.sprite = UserData.skinSprites[UserData.lastOpenedSkin];

        // Set position of the contents
        content.transform.localPosition = new Vector3(-1600f, 0f, 0f);
    }
}
