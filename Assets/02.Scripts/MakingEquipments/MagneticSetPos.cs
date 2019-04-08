using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticSetPos : MonoBehaviour
{
    public Transform ingotHitPlace;
    public Transform modelHitPlace;

    private Rigidbody ingotRigidbody;
    private Rigidbody modelRigidbody;

    public bool isSetCompleted = false;

    private void OnTriggerStay(Collider other)
    {
        if (this.tag != "MAGNETICZONE" || !(other.tag == "INGOT" || other.tag == "CHANGINGMODEL"))
            return;

        if (other.tag == "INGOT")
        {
            bool isGrabbed = other.GetComponent<IngotCtrl>().isGrabbed;
            bool isMagneticAvailable = other.GetComponent<IngotCtrl>().isMagneticAvailable_INGOT;

            if (!isGrabbed && isMagneticAvailable)
            {
                other.GetComponent<IngotCtrl>().isMagneticAvailable_INGOT = !isMagneticAvailable;
                StartCoroutine(SetPos(other));
            }
        }
        else if (other.tag == "CHANGINGMODEL")
        {
            bool isGrabbed = other.GetComponent<ModelCtrl>().isGrabbed;
            bool isMagneticAvailable = other.GetComponent<ModelCtrl>().isMagneticAvailable_MODEL;

            if (!isGrabbed && isMagneticAvailable)
            {
                other.GetComponent<ModelCtrl>().isMagneticAvailable_MODEL = !isMagneticAvailable;
                StartCoroutine(SetModelPos(other));
            }
        }
    }

    private IEnumerator SetPos(Collider other)
    {
        ingotRigidbody = other.GetComponent<Rigidbody>();
        ingotRigidbody.isKinematic = true;

        Vector3 magnetPos = ingotHitPlace.transform.position;
        Quaternion magnetRot = ingotHitPlace.transform.rotation;

        while (true)
        {
            Vector3 objPos = other.transform.position;
            Quaternion objRot = other.transform.rotation;

            if(Vector3.Distance(objPos, magnetPos) >= 0.005F && Quaternion.Angle(objRot,magnetRot) > 0.05F)
            {
                other.transform.position = Vector3.Slerp(objPos, magnetPos, 6.0F * Time.deltaTime);
                other.transform.rotation = Quaternion.Slerp(objRot, magnetRot, 8.0F * Time.deltaTime);
            }
            if(Vector3.Distance(objPos, magnetPos) >= 0.005F)
            {
                other.transform.position = Vector3.Slerp(objPos, magnetPos, 6.0F * Time.deltaTime);
            }
            else if(Quaternion.Angle(objRot, magnetRot) > 0.05F)
            {
                other.transform.rotation = Quaternion.Slerp(objRot, magnetRot, 8.0F * Time.deltaTime);
            }
            else break;
            yield return null;
        }

        isSetCompleted = true;
        ingotRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        ingotRigidbody.isKinematic = false;
        yield return null;
    }

    private IEnumerator SetModelPos(Collider other)
    {
        ingotRigidbody = other.GetComponent<Rigidbody>();
        ingotRigidbody.isKinematic = true;

        Vector3 magnetPos = modelHitPlace.transform.position;
        Quaternion magnetRot = modelHitPlace.transform.rotation;

        while (true)
        {
            Vector3 objPos = other.transform.position;
            Quaternion objRot = other.transform.rotation;

            if (objPos != magnetPos && objRot != magnetRot)
            {
                other.transform.position = Vector3.Slerp(objPos, magnetPos, 6.0F * Time.deltaTime);
                other.transform.rotation = Quaternion.Slerp(objRot, magnetRot, 8.0F * Time.deltaTime);
            }
            else if (objPos != magnetPos)
            {
                other.transform.position = Vector3.Slerp(objPos, magnetPos, 6.0F * Time.deltaTime);
            }
            else if (objRot != magnetRot)
            {
                other.transform.rotation = Quaternion.Slerp(objRot, magnetRot, 8.0F * Time.deltaTime);
            }
            else break;

            yield return null;
        }
        ingotRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        ingotRigidbody.isKinematic = false;
        yield return null;
    }
}
