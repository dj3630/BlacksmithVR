using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_hammeringComplete : MonoBehaviour
{
    private void Awake()
    {
        GameManager_Tutorial.Instance.hammeringCompleteCheck = true;
    }
}
