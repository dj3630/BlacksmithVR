using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WheelSwitch : MonoBehaviour
{
    public ParticleSystem effect;

    private void OnCollisionEnter(Collision coll)
    {
        effect.transform.position = coll.contacts[0].point;
        
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, coll.contacts[0].normal);
        effect.transform.rotation = rot;
        effect.Play();
    }

    private void OnCollisionStay(Collision coll)
    {
        effect.transform.position = coll.contacts[0].point;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, coll.contacts[0].normal);
        effect.transform.rotation = rot;

    }

    private void OnCollisionExit(Collision collision)
    {
        effect.Stop();
    }
}