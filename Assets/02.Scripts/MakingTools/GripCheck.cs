using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;   //추가

public class GripCheck : MonoBehaviour
{
    public bool isGripable = false;
    private Transform swordTr; 
    private Transform gripTr;
    private BasicGrabbable grabScript;  //추가
    
    void Start()
    {
        swordTr = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SWORDGRIP")
        {
            isGripable = true;
            Debug.Log("OnTriggerEnter");
            gripTr = other.transform;
            grabScript = other.GetComponent<BasicGrabbable>();
            grabScript.onGrabberDrop += Attach;
         }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SWORDGRIP")
        {
            isGripable = false;
            Debug.Log("OnTriggerExit");

            grabScript.onGrabberDrop -= Attach;
            gripTr = null;
        }
    }

    public void Attach()
    {
        Debug.Log("Attach");
        if (isGripable)
        {
            
            Debug.Log("B");
            gripTr.GetComponent<Rigidbody>().isKinematic = true;
            gripTr.SetParent(swordTr);
            gripTr.localPosition = Vector3.zero; // 포지션고정
            Quaternion q = gripTr.transform.localRotation;  //축 고정
            q.x = -180;
            gripTr.transform.localRotation = q;
            gripTr.GetComponent<Collider>().enabled = false;
            gripTr.GetComponent<GripCompleteEffect_Tutorial>().ParticlePlayBack();  //추가
        }
    }
}


