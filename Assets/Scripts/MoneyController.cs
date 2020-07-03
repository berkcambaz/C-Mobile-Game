using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    public SpriteRenderer sprite;   // If used in game
    public Image image;             // If used in hud

    private float h;
    public float s; // Make them public so alpha can be set outside
    public float v; // Make them public so alpha can be set outside

    void Update()
    {
        float amount = Mathf.Min(Time.deltaTime, 0.0025f);

        // Dynamically change color of the money
        h += amount;
        if (h > 1f)
            h = 0f;

        s += amount;
        if (s > 1f)
            s = 1f;

        v += amount;
        if (v > 1f)
            v = 1f;

        // Set color
        if (sprite != null)
            sprite.color = Color.HSVToRGB(h, s, v);
        else
            image.color = Color.HSVToRGB(h, s, v);
    }
}
