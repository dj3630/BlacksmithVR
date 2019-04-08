using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageMoving_Tutorial : MonoBehaviour
{
    private RectTransform tr;
    private Vector3 startingPos;
    private Vector3 endPos;
    private bool changeDir = false;
    private float speed = 0.15F;

    void Start()
    {
        tr = GetComponent<RectTransform>();
        startingPos = tr.localPosition;
        endPos = tr.localPosition - new Vector3(0, 0.1F, 0);
    }

    void Update()
    {
        if (Vector3.Distance(tr.localPosition, endPos) <= 0.0005F)
            changeDir = true;
        else if (changeDir && Vector3.Distance(tr.localPosition, startingPos) <= 0.0005F)
            changeDir = false;

        if (changeDir)
            tr.localPosition = Vector3.Slerp(transform.localPosition, startingPos, speed);
        else
            tr.localPosition = Vector3.Slerp(transform.localPosition, endPos, speed);
    }
}
