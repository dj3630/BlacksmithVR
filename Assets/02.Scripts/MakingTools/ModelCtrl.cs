using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCtrl : MonoBehaviour
{
    public bool isGrabbed = false;

    public bool isMagneticAvailable_MODEL = true;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    public void IsGrabbed()
    {
        isGrabbed = true;
        isMagneticAvailable_MODEL = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void IsReleased()
    {
        isGrabbed = false;
    }
}
