using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopUp : MonoBehaviour
{
    public static ScorePopUp instance;

    public GameObject scorePopup;

    void Start()
    {
        instance = this;
    }

    public void PopupScore(Vector3 pos)
    {
        StartCoroutine(FadePopupScore(Instantiate(scorePopup, pos, transform.rotation, transform), 1f));
    }

    IEnumerator FadePopupScore(GameObject popup, float time)
    {
        float timer = 0f;

        while (time > timer)
        {
            float dt = Time.deltaTime;
            timer += dt;

            popup.transform.Translate(0f, dt, 0f);
            popup.GetComponent<Text>().color = new Color(1f, 1f, 1f, popup.GetComponent<Text>().color.a - dt);


            yield return null;
        }

        Destroy(popup);

        yield return null;
    }
}
