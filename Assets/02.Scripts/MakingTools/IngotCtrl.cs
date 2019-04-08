using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotCtrl : MonoBehaviour
{
    public bool isGrabbed = false;

    public bool isMagneticAvailable_INGOT = true;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    public void IsGrabbed()
    {
        isGrabbed = true;
        isMagneticAvailable_INGOT = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void IsReleased()
    {
        isGrabbed = false;
    }
}
