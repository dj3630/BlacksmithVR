using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_hammering : MonoBehaviour
{
    private MagneticSetPos script;

    private void Start()
    {
        script = GameObject.FindGameObjectWithTag("MAGNETICZONE").GetComponent<MagneticSetPos>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "INGOT" && script.isSetCompleted)
        {
            //GameManager_Tutorial.Instance.magneticCheck = true;
        }
    }
}
