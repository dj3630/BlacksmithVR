using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatingCtrl : MonoBehaviour
{
    // 온도 관련 변수
    private float minTemp = 50.0F;
    private float maxTemp = 400.0F;
    public float objTemp;

    // 메질에 따른 게임오브젝트 변환 시 온도 전달 변수
    public bool tempReceive = false;
    public bool resetValues = false;

    // 담금질 관련 변수
    public bool isQuenched = false;
    public bool colliderCheck_1 = false;
    public bool colliderCheck_2 = false;
    public bool colliderCheck_3 = false;
    private float quenchingLevel_1 = 50.0f;
    private float quenchingLevel_2 = 75.0f;
    private float quenchingLevel_3 = 100.0f;
    private float quenchingCoolingSpeed = 0.0F;
    public bool isQuenchingComplete = false;

    // 가열 및 냉각속도 조절 변수
    private float heatingSpeed = 0.0F;
    private float coolingSpeed = 3.0F;

    // 온도에 따른 색 조절 변수
    private Color originColour;
    private float coloringOffset = 1300.0F;
    private MeshRenderer _meshRenderer;
    private ForgeState state;
    
    // 최저온도 도달 여부 체크 변수
    public bool isMinTemp = true;

    public void Awake()
    {
        // 중간 무기 모델링으로 변환 시 온도 전달
        if(resetValues && this.tag == "CHANGINGMODEL")
        {
            DataManager mgr = DataManager.Instance;
            objTemp = mgr.receivedTemp[++mgr.idx];
            resetValues = false;
            Debug.Log(objTemp);
        }
        // 최종 무기 모델링으로 변환 시 온도 전달
        else if (resetValues && this.tag == "FINALMODEL")
        {
            DataManager mgr = DataManager.Instance;
            objTemp = mgr.receivedTemp[mgr.idx];
            resetValues = false;
            Debug.Log(objTemp);
        }
        // 최초 생성 시 기본 온도로 설정
        else objTemp = minTemp;
    }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        originColour = _meshRenderer.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        IronCooling();  // 화덕 안에 있지 않을 때 서서히 냉각
        if (isQuenched && !isMinTemp) QuenchingCooling();  // 물통에 담갔을 때 담금질 로직 실행
        IronColoring();  // 온도에 따른 색 변화
    }

    // 화덕 안에 들어가면 가열 시작
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "HEATPLACE")
        {
            IronHeating();
        }
    }

    // 가열 로직
    private void IronHeating()
    {
        if (objTemp >= maxTemp) return;

        isMinTemp = false;
        state = GameObject.Find("ForgeChimney").GetComponent<ForgeCtrl>()._forgeState;

        // 화덕의 화력에 따라 가열속도 조절
        if (state == ForgeState.READY) heatingSpeed = 10.0F;
        else if (state == ForgeState.MAX) heatingSpeed = 30.0F;
        else heatingSpeed = 0.0F;

        objTemp += heatingSpeed * Time.deltaTime;
    }

    // 냉각 로직
    private void IronCooling()
    {
        if (objTemp > minTemp)
            objTemp -= coolingSpeed * Time.deltaTime;
        else if (objTemp <= minTemp)
        {
            objTemp = minTemp;
            isMinTemp = true;
        }
    }

    // 온도에 따른 색 변화 로직
    private void IronColoring()
    {
        float HeatingRate =  Mathf.InverseLerp(50.0F, 400.0F, objTemp);
        Color ironColour = originColour * HeatingRate * coloringOffset;
        _meshRenderer.material.SetColor("_EmissionColor", ironColour);
    }

    // 담금질 로직
    private void QuenchingCooling()
    {
        // 무기 모델링이 물통에 담가진 정도(3단계)에 따라 담금질 속도 조절
        if (colliderCheck_1 && colliderCheck_2 && colliderCheck_3)
        {
            quenchingCoolingSpeed = quenchingLevel_3;
        }
        else if(colliderCheck_1 && colliderCheck_2)
        {
            quenchingCoolingSpeed = quenchingLevel_2;
        }
        else if(colliderCheck_2 && colliderCheck_3)
        {
            quenchingCoolingSpeed = quenchingLevel_2;
        }
        else if(colliderCheck_3)
        {
            quenchingCoolingSpeed = quenchingLevel_1;
        }
        else if (colliderCheck_2)
        {
            quenchingCoolingSpeed = quenchingLevel_1;
        }
        else if(colliderCheck_1)
        {
            quenchingCoolingSpeed = quenchingLevel_1;
        }

        objTemp -= quenchingCoolingSpeed * Time.deltaTime;
        float steamSoundVolume = objTemp * 0.0025F;
        GetComponent<ModelHitCtrl_FINAL>().SteamSoundOn(steamSoundVolume);

        // 최저온도 보다 낮아지면 담금질 종료
        if (objTemp <= minTemp)
        {
            objTemp = minTemp;
            isMinTemp = true;
            isQuenchingComplete = true;
            Debug.Log("minTemp");
        }
    }
}
