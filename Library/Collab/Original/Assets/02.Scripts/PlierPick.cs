using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlierPick : MonoBehaviour
{
    private Transform plierTr;
    private Transform pickedObj;

    private bool isPickable = true;

    void Start()
    {
        plierTr = this.transform;
    }

    private void Update()
    {
        PickManager();
    }

    public void PickManager()
    {
        if (ControllerManager.Grip_Press && isPickable)
        {
            if (Physics.CheckBox(plierTr.position, Vector3.one * 0.2F, Quaternion.identity, 11 << 8, QueryTriggerInteraction.Collide))
            {
                pickedObj = Physics.OverlapBox(plierTr.position, Vector3.one * 0.2F, Quaternion.identity, 11 << 8, QueryTriggerInteraction.Collide)[0].transform;
                Pick();
            }
            else return;
        }
        else if (ControllerManager.Grip_PressUp && !isPickable)
        {
            Release();
        }
    }

    public void Pick()
    {
        if (isPickable)
        {
            pickedObj.SetParent(plierTr);
            pickedObj.localPosition = Vector3.zero;
            pickedObj.SendMessage("IsGrabbed");
            pickedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            pickedObj.GetComponent<Rigidbody>().useGravity = false;
            isPickable = false;
        }
    }

    public void Release()
    {
        if (!isPickable)
        {
            pickedObj.SetParent(null);
            pickedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            pickedObj.GetComponent<Rigidbody>().useGravity = true;
            pickedObj.SendMessage("IsReleased");
            pickedObj = null;
            isPickable = true;
        }
    }
}
