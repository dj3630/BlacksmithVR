using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChecker_recipe : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "INGOT")
        {
            Debug.Log("State transition to Moving");
            GameManager_Tutorial.Instance.recipeBoxCheck = true;
        }
    }
}
