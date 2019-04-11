using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quenching : MonoBehaviour
{
    public ParticleSystem effect;

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.tag == "Coll_1" || other.tag == "Coll_2" || other.tag == "Coll_3")) return;

        var script = other.GetComponentInParent<HeatingCtrl>();
        if (other.tag == "Coll_1")
            script.colliderCheck_1 = true;
        else if (other.tag == "Coll_2")
            script.colliderCheck_2 = true;
        else if (other.tag == "Coll_3")
            script.colliderCheck_3 = true;
        
        if(script.objTemp > 50.0F)
            effect.Play();
        GameManager_Tutorial.Instance.StopShining();
        other.GetComponentInParent<HeatingCtrl>().isQuenched = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!(other.tag == "Coll_1" || other.tag == "Coll_2" || other.tag == "Coll_3")) return;
        var script = other.GetComponentInParent<HeatingCtrl>();
        if (script.objTemp <= 50.0F)
            effect.Stop();
    }

    private void OnTriggerExit(Collider other)
    {
        HeatingCtrl script = other.GetComponentInParent<HeatingCtrl>();

        if ((other.tag == "Coll_1" || other.tag == "Coll_2" || other.tag == "Coll_3") && script.isQuenchingComplete)
        {
            Debug.Log(script.isQuenchingComplete);

            if (other.tag == "Coll_1")
                script.colliderCheck_1 = false;
            else if (other.tag == "Coll_2")
                script.colliderCheck_2 = false;
            else if (other.tag == "Coll_3")
                script.colliderCheck_3 = false;

            Debug.Log(script.colliderCheck_1);
            Debug.Log(script.colliderCheck_2);
            Debug.Log(script.colliderCheck_3);
            if (!script.colliderCheck_1 && !script.colliderCheck_2 && !script.colliderCheck_3)
            {
                Debug.Log("Trigger Out");
                script.GetComponent<ModelHitCtrl_FINAL>().isPlayed = false;
                script.GetComponent<ModelHitCtrl_FINAL>().SteamSoundOff();
                script.isQuenched = false;
                effect.Stop();
            }
        }
    }
}
