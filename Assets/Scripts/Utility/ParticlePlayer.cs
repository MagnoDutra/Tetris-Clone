using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem[] allParticles;

    // Start is called before the first frame update
    void Start()
    {
        allParticles = GetComponentsInChildren<ParticleSystem>();    
    }

    public void Play()
    {
        foreach (ParticleSystem p in allParticles)
        {
            p.Stop();
            p.Play();
        }
    }

}
