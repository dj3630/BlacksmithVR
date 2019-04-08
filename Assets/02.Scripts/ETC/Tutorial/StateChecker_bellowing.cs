using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_bellowing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GRIP")
        {
            Debug.Log("State transition to Heating");
            GameManager_Tutorial.Instance.bellowingBoxCheck = true;
        }
    }
}
