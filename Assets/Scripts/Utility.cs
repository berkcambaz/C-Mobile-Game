using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Utility
{
    public static Camera camera = Camera.main;
    public static Vector2 screenSize = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));

    public static ColorBlock ButtonColor(bool isPressed)
    {
        ColorBlock colorBlock = new ColorBlock();

        colorBlock.normalColor = new Color(1f, 1f, 1f);
        colorBlock.highlightedColor = new Color(1f, 1f, 1f);
        colorBlock.pressedColor = new Color(0.6f, 0.6f, 0.6f);
        if (!isPressed) // If true button will fade to black when pressed properly
            colorBlock.selectedColor = new Color(1f, 1f, 1f);

        colorBlock.colorMultiplier = 1f;
        colorBlock.fadeDuration = 0.35f;

        return colorBlock;
    }

    public static IEnumerator CameraShake(float shakeTime, float shakeAmount)
    {
        Vector3 originalPos = camera.transform.localPosition;

        while (shakeTime > 0f)
        {
            Vector2 shake = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * shakeAmount;
            camera.transform.localPosition = new Vector3(shake.x, shake.y, originalPos.z);
            shakeTime -= Time.deltaTime;

            yield return null;
        }

        camera.transform.localPosition = originalPos;   // Set camera to it's original pos
    }
}
