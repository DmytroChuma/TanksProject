using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : ShootableObject
{
    public GameObject particlesPrefab;

    public override void OnHit(RaycastHit hit)
    {
        GameObject particles = Instantiate(particlesPrefab, hit.point + (hit.normal * 0.05f), Quaternion.LookRotation(hit.normal), transform.root.parent);
        ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (particleSystem)
        {
            particleSystem.startColor = renderer.material.color;
        }
        Destroy(particleSystem, 2f);
    }
}
