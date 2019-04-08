using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_moving : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "INGOT")
        {
            Debug.Log("State transition to Bellowing");
            GameManager_Tutorial.Instance.movingBoxCheck = true;
        }
    }
}
