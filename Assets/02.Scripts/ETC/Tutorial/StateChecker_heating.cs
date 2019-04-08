using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_heating : MonoBehaviour
{
    private HeatingCtrl script;

    private bool isUpdated = true;

    private void Start()
    {
        script = GetComponentInParent<HeatingCtrl>();
    }

    void Update()
    {
        if(script.objTemp > 200.0F && isUpdated)
        {
            Debug.Log("State transition to Hammering");
            GameManager_Tutorial.Instance.tempCheck = true;
            isUpdated = false;
        }
    }
}
