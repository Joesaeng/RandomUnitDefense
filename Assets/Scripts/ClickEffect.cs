using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    ParticleSystem particle;
    private void Start()
    {
        if(particle == null)
            particle = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (particle.isStopped == true)
            Managers.Resource.Destroy(gameObject);
    }
}
