using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotHitEffect : MonoBehaviour
{
    private int objectIdx;
    public Animator anim;
    public GameObject parent;
    
    private void Start()
    {
        objectIdx = 0;
    }

    private void OnCollisionEnter(Collision colls)
    {
        if(objectIdx == 0 && colls.collider.tag == "HAMMER"
            && colls.relativeVelocity.magnitude > 2.0f)
        {
            objectIdx = colls.gameObject.GetInstanceID();
            Debug.Log("OnCollisionEnter");
            //float score = (anim.playbackTime) * 100;
            float score = anim.GetCurrentAnimatorStateInfo(0).normalizedTime 
                % anim.GetCurrentAnimatorStateInfo(0).length * 100f;
            Debug.Log("score : " + score);
            anim.speed = 0f;
            Destroy(parent, 0.4f);
            StartCoroutine(Callback((int)score));
        }
    }

    private IEnumerator Callback(int score)
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.HitCallback(score);
    }

    private void OnCollisionExit(Collision colls)
    {
        objectIdx = 0;
    }
}
