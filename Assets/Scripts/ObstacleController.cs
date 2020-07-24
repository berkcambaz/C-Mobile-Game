using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject particle;

    private const float speed = 5f;
    private bool isKilledItself = false;
    private bool gaveScore = false;

    void Update()
    {
        transform.Translate(0f, -speed * Time.deltaTime, 0f);

        if (!gaveScore && Utility.player.transform.position.y > transform.position.y)
        {
            // TODO: +1 score animation
            gaveScore = true;
            UserData.score += 1;
            UI.UpdateScore();
        }

        // If out of the screen
        if (transform.position.y + transform.localScale.y / 2f < -Utility.screenSize.y - 1f)
        {
            isKilledItself = true;
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // If it killed itself, it means it went out of the screen
        if (!UserData.isQuitting && !isKilledItself && Utility.screenSize.y > transform.position.y - transform.localScale.y / 2f)
        {
            GameObject particleInstance = Instantiate(particle, transform.position, transform.rotation);

            // Set particle color to red, because obstacles are red
            ParticleSystem.MainModule psmain = particleInstance.GetComponent<ParticleSystem>().main;
            psmain.maxParticles = UserData.particles;
            psmain.startColor = Color.red;
        }
    }

    // If obstacle & player are collided, destroy both of them
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
