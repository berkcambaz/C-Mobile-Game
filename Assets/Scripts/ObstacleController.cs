using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject particle;

    private bool isKilledItself = false;
    private bool gaveScore = false;

    void Update()
    {
        if (Utility.player != null && !gaveScore && Utility.player.transform.position.y > transform.position.y)
        {
            ScorePopUp.instance.PopupScore(transform.position);
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

    // If obstacle & player are collided
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
