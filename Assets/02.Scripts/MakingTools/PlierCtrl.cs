using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlierCtrl : MonoBehaviour
{
   public void ColliderOff()
    {
        this.GetComponent<Collider>().enabled = false;
    }

    public void ColliderOn()
    {
        this.GetComponent<Collider>().enabled = true;
    }
}
