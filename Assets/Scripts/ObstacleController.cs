using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject particle;

    public float speed;
    public Vector2 size;

    private Vector2 screenSize; // TODO: Create a static helper class that contains this

    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        transform.localScale = new Vector3(size.x, size.y, transform.localScale.z);
    }

    void Update()
    {
        transform.Translate(0f, -speed * Time.deltaTime, 0f);

        // If out of the screen
        if (transform.position.y < -screenSize.y - 1f)
        {
            Destroy(gameObject);
        }
    }

    // If obstacle & player are collided, destroy both of them
    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject particleInstance = Instantiate(particle, transform.position, transform.rotation);

        // Set particle color to red, because obstacles are red
        ParticleSystem.MainModule psmain = particleInstance.GetComponent<ParticleSystem>().main;
        psmain.maxParticles = UserData.particles;
        psmain.startColor = Color.red;

        Destroy(gameObject);
    }
}
