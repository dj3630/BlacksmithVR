using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatingCtrl : MonoBehaviour
{
    private float minTemp = 50.0F;
    private float maxTemp = 400.0F;
    public float objTemp;
    public bool tempReceive = false;
    public bool resetValues = false;
    public bool isQuenched = false;
    public bool colliderCheck_1 = false;
    public bool colliderCheck_2 = false;
    public bool colliderCheck_3 = false;
    private float quenchingLevel_1 = 50.0f;
    private float quenchingLevel_2 = 75.0f;
    private float quenchingLevel_3 = 100.0f;
    private float quenchingCoolingSpeed = 0.0F;
    private float steamSoundVolume = 0.0F;
    private float prevVolume = 0.0F;
    private bool isMinTemp = true;
    private float heatingSpeed = 0.0F;
    private float coolingSpeed = 1.0F;
    private Color originColour;
    private float coloringOffset = 1300.0F;
    private ForgeState state;
    private MeshRenderer _meshRenderer;

    public void Awake()
    {
        if(resetValues && this.tag == "CHANGINGMODEL")
        {
            DataManager mgr = DataManager.Instance;
            objTemp = mgr.receivedTemp[++mgr.idx];
            resetValues = false;
            Debug.Log(objTemp);
        }
        else if (resetValues && this.tag == "FINALMODEL")
        {
            DataManager mgr = DataManager.Instance;
            objTemp = mgr.receivedTemp[mgr.idx];
            resetValues = false;
            Debug.Log(objTemp);
        }
        else objTemp = minTemp;
    }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        originColour = _meshRenderer.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        IronCooling();
        if (isQuenched && isMinTemp) QuenchingCooling();
        IronColoring();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "HEATPLACE")
        {
            IronHeating();
            isMinTemp = true;
        }
    }

    private void IronHeating()
    {
        if (objTemp >= maxTemp) return;

        state = GameObject.Find("ForgeChimney").GetComponent<ForgeCtrl>()._forgeState;

        if (state == ForgeState.READY) heatingSpeed = 10.0F;
        else if (state == ForgeState.MAX) heatingSpeed = 30.0F;
        else heatingSpeed = 0.0F;

        objTemp += heatingSpeed * Time.deltaTime;
    }

    private void IronCooling()
    {
        if (objTemp > minTemp)
            objTemp -= coolingSpeed * Time.deltaTime;
        else if (objTemp <= minTemp)
            objTemp = minTemp;
    }

    private void IronColoring()
    {
        float HeatingRate =  Mathf.InverseLerp(50.0F, 400.0F, objTemp);
        Color ironColour = originColour * HeatingRate * coloringOffset;
        _meshRenderer.material.SetColor("_EmissionColor", ironColour);
    }

    private void QuenchingCooling()
    {
        if (colliderCheck_1 && colliderCheck_2 && colliderCheck_3)
        {
            quenchingCoolingSpeed = quenchingLevel_3;
            steamSoundVolume = 2.0F;
        }
        else if(colliderCheck_1 && colliderCheck_2)
        {
            quenchingCoolingSpeed = quenchingLevel_2;
            steamSoundVolume = 1.5F;
        }
        else if(colliderCheck_2 && colliderCheck_3)
        {
            quenchingCoolingSpeed = quenchingLevel_2;
            steamSoundVolume = 1.5F;
        }
        else if(colliderCheck_3)
        {
            quenchingCoolingSpeed = quenchingLevel_1;
            steamSoundVolume = 1.0F;
        }
        else if (colliderCheck_2)
        {
            quenchingCoolingSpeed = quenchingLevel_1;
            steamSoundVolume = 1.0F;
        }
        else if(colliderCheck_1)
        {
            quenchingCoolingSpeed = quenchingLevel_1;
            steamSoundVolume = 1.0F;
        }
        
        if (objTemp > minTemp)
        {
            objTemp -= quenchingCoolingSpeed * Time.deltaTime;

            float currVolume = steamSoundVolume;
            if (prevVolume != currVolume)
            {
                GetComponent<ModelHitCtrl_FINAL>().SteamSound(steamSoundVolume);
                prevVolume = steamSoundVolume;
            }
            else return;
        }
        else if (objTemp <= minTemp)
        {
            objTemp = minTemp;
            isMinTemp = false;
        }
    }
}
