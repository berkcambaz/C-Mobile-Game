using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particle;

    void Update()
    {
        if (!particle.IsAlive())
            Destroy(gameObject);
    }
}
