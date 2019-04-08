using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHitEffect : MonoBehaviour
{
    public ParticleSystem effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "INGOTHITSIDE" || other.tag == "CHANGINGMODEL")
        {
            effect.Play();
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "INGOTHITSIDE" || coll.collider.tag == "CHANGINGMODEL")
            effect.Play();
    }
}
