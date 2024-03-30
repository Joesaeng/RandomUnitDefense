using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    ParticleSystem particle;
    private void Start()
    {
        if(particle == null)
            particle = GetComponent<ParticleSystem>();
        if (particle == null)
            particle = GetComponentInChildren<ParticleSystem>();
    }
    void Update()
    {
        if (particle.isStopped == true)
            Managers.Resource.Destroy(gameObject);
    }
}
