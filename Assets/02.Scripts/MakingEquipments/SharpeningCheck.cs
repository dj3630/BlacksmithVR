using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpeningCheck : MonoBehaviour
{
    // 고스트 모델의 상태 체크 변수
    private enum GhostState { Ridable, Moving }
    private GhostState ghostModelState;    
    private bool isLoaded = false;
    public Transform originPos;
    public Transform endPos;

    // 고스트 모델 색 변화 변수
    private MeshRenderer _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        // 고스트 모델의 상태 체크 코루틴 함수 실행
        StartCoroutine(GhostStateCheck());
    }

    private void OnTriggerStay(Collider other)
    {
        // 충돌한 트리거 태그로 날 세우기 모델인지 확인
        if (other.tag != "SHARPNINGOBJ") return;

        // 고스트 모델과 날 세우기 모델이 트리거 충돌중이면 빨간색으로 변화
        _meshRenderer.material.color = new Vector4(50.0F, 0.0F, 0.0F, 0.05F);

        // 고스트 모델의 상태가 Ridable일 때, 날 세우기 모델의 위치값이 근사한지 확인
        if (ghostModelState == GhostState.Ridable && Vector3.Distance(transform.position, other.transform.position) <= 0.3F)
            //&& Quaternion.Angle(transform.localRotation, other.transform.rotation) < 10.0F)
        {
            Debug.Log("Enter");
            // 날 세우기 모델을 고스트 모델에 태웠을 경우 초록색으로 변화
            _meshRenderer.material.color = new Vector4(0.0F, 50.0F, 0.0F, 0.05F);
            isLoaded = true;
        }

        // 고스트 모델의 상태가 Moving일 때, 날 세우기 모델을 태웠는지 여부, 날 세우기 모델의 위치값이 근사한지 확인
        if(ghostModelState == GhostState.Moving && isLoaded && Vector3.Distance(transform.position, other.transform.position) <= 0.3F)
            //&& Quaternion.Angle(transform.localRotation, other.transform.rotation) < 10.0F)
        {
            // 날 세우기 모델이 고스트 모델을 따라 움직인다면 초록색 유지
            _meshRenderer.material.color = new Vector4(0.0F, 50.0F, 0.0F, 0.05F);
        }
        // 고스트 모델의 상태가 Moving일 때, 날 세우기 모델을 태웠는지 여부, 날 세우기 모델의 위치값이 근사한지 확인
        else if (ghostModelState == GhostState.Moving && isLoaded && !(Vector3.Distance(transform.position, other.transform.position) <= 0.3F))
            //&& Quaternion.Angle(transform.localRotation, other.transform.rotation) < 10.0F))
        {
            // 날 세우기 모델이 고스트 모델을 따라 움직이지 않으면 빨간색으로 변화
            _meshRenderer.material.color = new Vector4(50.0F, 0.0F, 0.0F, 0.05F);
            isLoaded = false;
        }
        else if(ghostModelState == GhostState.Moving && !isLoaded)
        {
            // 날 세우기 모델이 고스트 모델에 타지 못했으면 빨간색으로 변화
            _meshRenderer.material.color = new Vector4(50.0F, 0.0F, 0.0F, 0.05F);
        }
    }
    
    // 고스트 모델의 상태 체크 로직
    private IEnumerator GhostStateCheck()
    {
        while (true)
        {
            // 0.1초 마다 상태 체크
            yield return new WaitForSeconds(0.1F);

            // 고스트 모델이 원위치에 있을 때 Ridable 상태
            if (transform.position == originPos.position || transform.position == endPos.position)
            {
                ghostModelState = GhostState.Ridable;
                Debug.Log("Ridable");
            }
            // 고스트 모델이 움직이고 있을 때 Moving 상태
            else ghostModelState = GhostState.Moving;
        }
    }
}
