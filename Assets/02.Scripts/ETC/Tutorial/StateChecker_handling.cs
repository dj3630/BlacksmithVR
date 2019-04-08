using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_handling : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SWORDGRIP")
        {
            Debug.Log("State transition to Completing");
            GameManager_Tutorial.Instance.handlingBoxCheck = true;
        }
    }
}
