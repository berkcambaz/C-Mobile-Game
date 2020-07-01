using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject particle;

    private const float speed = 5f;
    private bool isKilledItself = false;

    private Vector2 screenSize; // TODO: Create a static helper class that contains this

    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        transform.Translate(0f, -speed * Time.deltaTime, 0f);

        // If out of the screen
        if (transform.position.y + transform.localScale.y / 2f < -screenSize.y - 1f)
        {
            isKilledItself = true;
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // If it killed itself, it means it went out of the screen
        if (!UserData.isQuitting && !isKilledItself && screenSize.y > transform.position.y + transform.localScale.y / 2f)
        {
            GameObject particleInstance = Instantiate(particle, transform.position, transform.rotation);

            // Set particle color to red, because obstacles are red
            ParticleSystem.MainModule psmain = particleInstance.GetComponent<ParticleSystem>().main;
            psmain.maxParticles = UserData.particles;
            psmain.startColor = Color.red;
        }

        // If this obstacle is the last obstacle left in this obstacle set
        if (transform.parent.childCount == 1)
            UserData.isObstacleSetFinished = true;
    }

    // If obstacle & player are collided, destroy both of them
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
