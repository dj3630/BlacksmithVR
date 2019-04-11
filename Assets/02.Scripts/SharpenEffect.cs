using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpenEffect : MonoBehaviour
{
    public ParticleSystem wheelEffect;
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.name == "Wheel")
        {
            wheelEffect.Play();
        }
    }

}
