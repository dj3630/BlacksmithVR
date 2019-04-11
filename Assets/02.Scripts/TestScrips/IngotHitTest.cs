using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotHitTest : MonoBehaviour
{
    public Transform[] hitPoints;
    private int[] idx = {0,1,2,0,1,2, 0, 1, 2, 0, 1, 2 };
    public int count = 0;
    private bool hitMode = false;
    private bool ModeEnd = false;

    private void OnCollisionEnter(Collision coll)
    {
        if (ModeEnd || coll.collider.tag != "HAMMER") return;
        Debug.Log("OnCollisionEnter");
        if (!hitMode)
        {
            GameManager.Instance.StartHitEffect(hitPoints, idx, ChangeShape);
            hitMode = true;
        }
        else
        {
            Debug.Log("Bad hit!!!");
        }
    }

    private void ChangeShape()
    {
        Debug.Log("ChangeShape");
        if(++count == idx.Length)
        {
            Debug.Log("Change Shape End!!!");
            ModeEnd = true;
        }
    }
}
