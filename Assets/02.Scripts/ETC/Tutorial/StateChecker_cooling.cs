using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_cooling : MonoBehaviour
{
    private HeatingCtrl script;

    private bool isUpdated = true;

    private void Start()
    {
        script = GetComponent<HeatingCtrl>();
    }

    void Update()
    {
        if (script.isQuenchingComplete && isUpdated)
        {
            Debug.Log("State transition to Handling");
            GameManager_Tutorial.Instance.coolingCheck = true;
            isUpdated = false;
        }
    }
}
