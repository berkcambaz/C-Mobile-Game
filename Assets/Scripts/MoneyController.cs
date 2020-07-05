using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    public SpriteRenderer sprite;   // If used in game
    public Image image;             // If used in hud

    private Vector2 screenSize;

    private const float speed = 5f;

    private float h;
    public float s; // Make them public so alpha can be set outside
    public float v; // Make them public so alpha can be set outside

    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        float amount = Mathf.Min(Time.deltaTime, 0.005f);

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
        {
            transform.Translate(0f, -speed * Time.deltaTime, 0f);

            // If out of the screen
            if (transform.position.y + transform.localScale.y / 2f < -screenSize.y - 1f)
                Destroy(gameObject);

            sprite.color = Color.HSVToRGB(h, s, v);
        }
        else
            image.color = Color.HSVToRGB(h, s, v);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
