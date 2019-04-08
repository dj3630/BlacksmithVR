using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Utility;
using System;
using UnityEngine.Events;

public class GrabbedCollider : MonoBehaviour
{
    Collider Grabbed_Collider;
    

    void Start()
    {
       
        Grabbed_Collider = GetComponent<Collider>();
    }



    public void OffCollider()
    {
       // if (ViveInputVirtualButton.isGrabbed)
        
            Grabbed_Collider.enabled = false;
        
    }


    public void OnCollider()
    {
        Grabbed_Collider.enabled = true;
    }
}
