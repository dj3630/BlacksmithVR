using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForgeState { COOLED, PREWARM, READY, MAX }

public class ForgeCtrl : MonoBehaviour
{
    private float fireTemp = 100.0F;
    private float coolingSpeed = 0.1F;
    private float firePower = 0;

    private ParticleSystem forgeFire;
    private GameObject forgeSmoke;
    private GameObject forgeLight;

    private float wobbleIntensity = 0.0F;
    private float wobbleSpeed = 0.0F;
    private float lightIntensity = 0.0F;

    public ForgeState _forgeState = ForgeState.COOLED;
    
    void Start()
    {
        forgeFire = GetComponentsInChildren<ParticleSystem>()[1];
        forgeSmoke = GameObject.Find("Smoke");
        forgeLight = GameObject.Find("ForgeLight");
        
        //wobbleIntensity = forgeLight.GetComponent<FirelightWobble>().WobbleIntensity;
        //wobbleSpeed = forgeLight.GetComponent<FirelightWobble>().WobbleSpeed;
        //lightIntensity = forgeLight.GetComponent<FirelightWobble>().LightIntensity;
        forgeSmoke.SetActive(false);
    }

    void Update()
    {
        // pistonCount = GameObject.FindGameObjectWithTag("BELLOWS").GetComponent<BellowsPistonCheck>().loopCount;
        Cooling();
        SetState(fireTemp);
        Firing(_forgeState);
    }

    public void SetTemp()
    {
        fireTemp++;
    }

    private void Cooling()
    {
        if (fireTemp > 100.0F)
        {
            fireTemp -= 0.05F * Time.deltaTime;
        }
        else if (fireTemp <= 100.0F)
        {
            fireTemp = 100.0F;
        }
    }

    private void SetState(float temp)
    {
        if(temp >= 100 && temp < 102)
        {
            _forgeState = ForgeState.COOLED;
            firePower = (int)(temp - 100);
        }
        else if (temp >= 102 && temp < 105)
        {
            _forgeState = ForgeState.PREWARM;
            firePower = (int)(temp - 100);
        }
        else if (temp >= 105 && temp < 110)
        {
            _forgeState = ForgeState.READY;
            firePower = (int)(temp - 100);
        }
        else if (temp >= 110)
        {
            _forgeState = ForgeState.MAX;
            firePower = 10;
        }
        Debug.Log(_forgeState);
    }

    private void Firing(ForgeState state)
    {
        var main = forgeFire.main;
        float addLifeTime = 1.5F;
        float multiplySpeed = 0.5F;
        float addSize = 0.01F;

        switch (state)
        {
            case ForgeState.COOLED:
                addLifeTime += 0.1F * firePower;
                multiplySpeed += 0.1F * firePower;
                addSize += 0.005F * firePower;
                break;
            case ForgeState.PREWARM:
                addLifeTime += 0.1F * firePower;
                multiplySpeed += 0.1F * firePower;
                addSize += 0.005F * firePower;
                forgeSmoke.SetActive(false);
                break;
            case ForgeState.READY:
                addLifeTime = 1.9F + 0.1F * firePower;
                multiplySpeed = 0.9F + 0.1F * firePower;
                addSize = 0.03F + 0.005F * firePower;
                forgeSmoke.SetActive(true);
                break;
            case ForgeState.MAX:
                addLifeTime = 3.0F;
                multiplySpeed = 2.0F;
                addSize = 0.08F;
                break;
        }

        main.startLifetime = addLifeTime;
        main.startSpeedMultiplier = multiplySpeed;
        main.startSize = addSize;


        forgeLight.GetComponent<FirelightWobble>().WobbleIntensity = 0.01F + 0.01F * firePower;
        forgeLight.GetComponent<FirelightWobble>().WobbleSpeed = 1.0F + 2.0F * firePower;
        forgeLight.GetComponent<FirelightWobble>().LightIntensity = 0.3F + 0.2F * firePower;
    }
}
