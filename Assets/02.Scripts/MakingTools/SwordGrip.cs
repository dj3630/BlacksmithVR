using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGrip : MonoBehaviour
{
    public Transform SwordTr;
    public Transform GripTr;

    public bool A = false;
    


    void Start()
    {
        SwordTr = GameObject.Find("SwordPos").GetComponent<Transform>();
        GripTr = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    

}
