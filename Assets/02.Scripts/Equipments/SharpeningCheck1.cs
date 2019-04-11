using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpeningCheck1 : MonoBehaviour
{
    // 고스트 모델의 상태 체크하는 변수
    private enum GhostState { RidableFront, RidableBack, Riding }
    private GhostState ghostState;
    public Transform originPos;
    public Transform endPos;

    // 판정을 관리하는 변수
    private short sharpeningCount = 0;
    private WaitForSeconds ws;

    // 고스트 모델의 반쪽 모델링 저장하는 변수
    public GameObject[] halfModel;

    // 고스트 모델 렌더링을 제어하는 변수
    private MeshRenderer _meshRenderer;
    private Color originColor;
    
    // 고스트 모델 애니메이션을 제어하는 변수
    private Animator _animator;
    private int hashAnim = 0;
    private int hashGhostMoveForward = Animator.StringToHash("GhostMoveForward");
    private int hashGhostMoveBackward = Animator.StringToHash("GhostMoveBackward");
    private int hashFlip = Animator.StringToHash("Flip");
    private bool isFlipping = false;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
        originColor = _meshRenderer.material.color;
        ws = new WaitForSeconds(0.3F);
        ghostState = GhostState.RidableFront;
    }

    private void OnTriggerStay(Collider coll)
    {
        if (ghostState == GhostState.Riding || isFlipping) return;
        
        // 트리거 충돌체의 태그와 위치/각도 확인
        if (coll.tag == "SHARPNINGOBJ" && Vector3.Distance(transform.position, coll.transform.position) <= 0.3F
            && Vector3.Angle(transform.forward, coll.transform.forward) <= 10.0F)
            {
            // 조건식 참이면 색상 변화(초록색)
            _meshRenderer.material.color = Color.green;
            // 조건식 참이면 애니메이션 설정
            if (ghostState == GhostState.RidableFront)
                hashAnim = hashGhostMoveBackward;
            else if(ghostState == GhostState.RidableBack)
                hashAnim = hashGhostMoveForward;
            _animator.SetTrigger(hashAnim);
            // 조건식 참이면 고스트 상태 전이(-> Riding)
            Debug.Log("Riding");
            ghostState = GhostState.Riding;
            // Riding 상태일 때 판정 코루틴 호출
            StartCoroutine(FollowChecking(coll));
        }
    }

    // 판정 코루틴
    private IEnumerator FollowChecking(Collider coll)
    {
        while (ghostState == GhostState.Riding)
        {
            // 모델의 위치/각도를 지속적으로 체크
            if (Vector3.Distance(transform.position, coll.transform.position) <= 0.3F && Vector3.Angle(transform.forward, coll.transform.forward) <= 10.0F)
            {
                // 잘 따라오고 있으면 판정에 반영
                Debug.Log("sharpeningCount: " + ++sharpeningCount);
            }
            else
            {
                Debug.Log("Out");
                // 이탈했으면 색상 변화(빨간색)
                _meshRenderer.material.color = Color.red;
                yield break;
            }
            yield return ws;
        }
    }

    // 후방에 도착하면 상태 전이 이벤트 호출
    public void StateFront()
    {
        ghostState = GhostState.RidableFront;
        _meshRenderer.material.color = originColor;
        //sharpeningCount = 0;
        if (sharpeningCount >= 20)
        {
            _animator.SetTrigger(hashFlip);
            isFlipping = true;
        }
    }

    // 전방에 도착하면 상태 전이 이벤트 호출
    public void StateBack()
    {
        ghostState = GhostState.RidableBack;
        _meshRenderer.material.color = originColor;
        //sharpeningCount = 0;
        if (sharpeningCount >= 20)
        {
            _animator.SetTrigger(hashFlip);
            isFlipping = true;
        }
    }
    
    // 뒤집기 애니메이션 이후 이벤트로 호출
    public void ResetFlipping()
    {
        isFlipping = false;
    }
}