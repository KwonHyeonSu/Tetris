using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_BlockDown : MonoBehaviour
{
    private ParticleSystem particleObj;
    public void DestroyParticle()
    {
        ObjectPool.ReturnObject(this.GetComponent<Particle_BlockDown>());
    }

    void Awake()
    {
        if(null == particleObj)
            particleObj = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        GoPlay();
    }

    void GoPlay()
    {
        Invoke("DestroyParticle", 2.0f);
        particleObj.Play();
    }
}
