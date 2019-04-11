using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpeningCheck : MonoBehaviour
{
    // 고스트 모델의 상태 체크하는 변수
    private enum GhostState { Ridable, Moving, Turning }
    private GhostState ghostModelState;
    private bool isLoaded = false;
    private WaitForSeconds ws = new WaitForSeconds(0.1F);
    public Transform originPos;
    public Transform endPos;

    // 날 세우기 판정결과와 조건을 관리하는 변수
    private short sharpeningCount = 0;
    private bool isChecked = false;
    private float startTime = 0.0F;

    // 고스트 모델의 반쪽 모델링 저장하는 변수
    public GameObject[] halfModel;

    // 고스트 모델 렌더링을 제어하는 변수
    private MeshRenderer _meshRenderer;
    private Color originColor;
    
    // 고스트 모델 애니메이션을 제어하는 변수
    private Animator _animator;
    private int hashSharpeningCount = Animator.StringToHash("SharpeningCount");
    private int hashGhostMoveForward = Animator.StringToHash("GhostMoveForward");
    private int hashGhostMoveBackward = Animator.StringToHash("GhostMoveBackward");
    private int hashTurn = Animator.StringToHash("Turn");
    private bool isPlaying = false;
    private bool isTurned = false;

    private void OnEnable()
    {
        // 고스트 모델의 상태 체크 코루틴 함수 실행
        StartCoroutine(GhostStateCheck());
    }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
        originColor = _meshRenderer.material.color;
    }
    
    // 고스트 모델의 상태 체크 로직
    private IEnumerator GhostStateCheck()
    {
        while (true)
        {
            // 날 세우기 판정 횟수가 4회인 경우 Turning 상태로 전환
            if (sharpeningCount == 4 && isTurned)
            {
                ghostModelState = GhostState.Turning;
            }
            // 날 세우기 판정 횟수가 2회인 경우 Turning 상태로 전환
            else if (sharpeningCount == 2 && !isTurned)
            {
                ghostModelState = GhostState.Turning;
            }
            // 고스트 모델이 정지위치 안쪽에 있을 때 Ridable 상태로 전환
            else if (Vector3.Distance(transform.position, originPos.position) < 0.1F || Vector3.Distance(transform.position, endPos.position) < 0.1F)
            {
                ghostModelState = GhostState.Ridable;
            }
            // 고스트 모델이 정지위치 사이에 있을 때 Moving 상태로 전환
            else if (Vector3.Distance(transform.position, originPos.position) >= 0.1F || Vector3.Distance(transform.position, endPos.position) >= 0.1F)
            {
                ghostModelState = GhostState.Moving;
            }

            // 고스트 모델 상태에 따른 로직 실행
            if (!isPlaying) StateAction();

            // 0.1초 마다 상태 체크
            yield return ws;
        }
    }

    // 고스트 모델의 상태에 따라 로직
    private void StateAction()
    {
        switch (ghostModelState)
        {
            case GhostState.Ridable:
                if (Vector3.Distance(transform.position, originPos.position) < 0.1F && isLoaded)
                {
                    _animator.SetTrigger(hashGhostMoveForward);
                    startTime = Time.time;
                    isPlaying = true;
                }
                else if (Vector3.Distance(transform.position, endPos.position) < 0.1F && isLoaded)
                {
                    _animator.SetTrigger(hashGhostMoveBackward);
                    startTime = Time.time;
                    isPlaying = true;
                }
                break;

            case GhostState.Moving:
                break;

            case GhostState.Turning:
                Debug.Log("Turning");
                if (sharpeningCount == 2)
                {
                    Debug.Log("Turning Animation");
                    halfModel[0].SetActive(true);
                    _animator.SetInteger(hashSharpeningCount, 1);
                    _animator.SetTrigger(hashTurn);
                    isTurned = true;
                    isPlaying = true;
                }
                else if (sharpeningCount == 4)
                {
                    halfModel[0].SetActive(true);
                    isPlaying = true;
                }
                break;
        }
    }

    // 트리거 충돌 시 고스트 모델 색상 변화
    private void OnTriggerStay(Collider other)
    {
        // 충돌한 트리거의 태그가 날 세우기 모델인지 확인
        if (other.tag != "SHARPNINGOBJ" || ghostModelState == GhostState.Turning) return;

        Debug.DrawRay(transform.position, transform.forward, Color.cyan, 100.0F);
        Debug.DrawRay(other.transform.position, other.transform.forward, Color.yellow, 100.0F);

        // 고스트 모델의 상태가 Ridable일 때
        if (ghostModelState == GhostState.Ridable)
        {
            Debug.Log("Ridable");
            //날 세우기 모델의 위치값/회전값이 근사한지 확인
            if (Vector3.Distance(transform.position, other.transform.position) <= 0.3F
                && Vector3.Angle(transform.forward, other.transform.forward) <= 10.0F)
            {
                Debug.Log("Loaded");
                // 날 세우기 모델을 고스트 모델에 태웠을 경우 초록색으로 변화
                _meshRenderer.material.color = new Vector4(0.0F, 50.0F, 0.0F, 0.05F);
                isLoaded = true;
            }
            // 고스트 모델에 태웠으나 잠깐이라도 이탈한 경우 빨간색으로 변화
            else if (isLoaded && (Vector3.Distance(transform.position, other.transform.position) > 0.3F
                || Vector3.Angle(transform.forward, other.transform.forward) > 10.0F))
            {
                Debug.Log("out");
                _meshRenderer.material.color = new Vector4(50.0F, 0.0F, 0.0F, 0.05F);
                isLoaded = false;
            }
            // 고스트 모델에 태우지 못했지만 트리거 충돌은 한 경우 오리지널 색 유지
            else if (!isPlaying)
            {
                Debug.Log("notLoaded");
                _meshRenderer.material.color = originColor;
                isLoaded = false;
            }
        }

        // 고스트 모델의 상태가 Moving일 때
        if (ghostModelState == GhostState.Moving)
        {
            // 날 세우기 모델을 태웠는지 여부, 날 세우기 모델의 위치값/회전값이 근사한지 확인
            if (isLoaded && !isChecked && Vector3.Distance(transform.position, other.transform.position) <= 0.3F
                && Vector3.Angle(transform.forward, other.transform.forward) <= 10.0F)
            {
                // 고스트 모델에 태운 채로 함께 이동중이면 초록색 유지
                _meshRenderer.material.color = new Vector4(0.0F, 50.0F, 0.0F, 0.05F);

                if (!isChecked)
                {
                    // 날 세우기 판정 로직(1초 이상 태우고 이동해야 판정 카운트)
                    var currTime = Time.time;
                    var duration = currTime - startTime;
                    Debug.Log("Duration: " + duration);
                    if (duration > 1.0F)
                    {
                        Debug.Log("sharpeningCount :" + (++sharpeningCount));
                        isChecked = true;
                    }
                }
            }
            // 판정에 성공한 후에는 이탈하더라도 끝에 도달할 때까지 초록색 유지
            else if(isChecked)
            {
                _meshRenderer.material.color = new Vector4(0.0F, 50.0F, 0.0F, 0.05F);
            }
            // 고스트 모델에 태웠으나 잠깐이라도 이탈한 경우 빨간색으로 변화
            else if (isLoaded && (Vector3.Distance(transform.position, other.transform.position) > 0.3F
                    || Vector3.Angle(transform.forward, other.transform.forward) > 10.0F))
            {
                _meshRenderer.material.color = new Vector4(50.0F, 0.0F, 0.0F, 0.05F);
                isLoaded = false;
            }
            else if (!isLoaded)
            {
                _meshRenderer.material.color = new Vector4(50.0F, 0.0F, 0.0F, 0.05F);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 트리거를 빠져나가는 물체의 태그가 날 세우기 모델인지 확인
        if (other.tag != "SHARPNINGOBJ" || ghostModelState == GhostState.Turning) return;

        // 맞다면 오리지널 색으로 바꾸기
        _meshRenderer.material.color = originColor;
    }

    public void ResetPlaying()
    {
        isChecked = false;
        isLoaded = false;
        isPlaying = false;
    }

    public void ResetTurning()
    {
        isPlaying = false;
        ghostModelState = GhostState.Ridable;
    }
}