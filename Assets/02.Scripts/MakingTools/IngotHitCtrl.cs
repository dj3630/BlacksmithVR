using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotHitCtrl : MonoBehaviour
{
    // 메질에 따른 철괴 스케일 값 변수
    private float lengthFlattenRate = 1.6F;
    private float heightFlattenRate = 2.0F;
    private float widthFlattenRate = 1.6F;

    // 메질 재실행 조건 체크하는 변수
    private Transform hammer;
    private float distance = 0.0F;
    public static bool hitAvailable = true;

    // 무기 중간 모델링 변수
    public GameObject changingModels;
    // 무기 중간 모델링 모루 상단 위치 변수
    public Transform changingModelsHitPlace;

    // 메질 횟수 체크 변수
    public static int hitCount = 0;

    // 메질에 따른 파티클 변수
    public GameObject effect;

    // 메질에 따른 사운드 효과 변수
    private AudioSource _audio;

    public float testValue = 2.0F;

    private void Start()
    {
        hammer = GameObject.Find("HammerHitSide").transform;
        _audio = GetComponent<AudioSource>();
    }

    // 망치와 철괴 충돌판정 로직
    private void OnCollisionEnter(Collision coll)
    {
        // 충돌체 태그, 메질 재실행 조건, 모루 상단 자석효과, 메질 가능 온도(300.0F) 충족 여부 확인
        if (coll.collider.tag == "HAMMERHITSIDE" && this.tag == "INGOTHITSIDE"// && hitAvailable 
            && !GetComponentInParent<IngotCtrl>().isMagneticAvailable_INGOT && GetComponentInParent<HeatingCtrl>().objTemp > 200.0F
            && coll.relativeVelocity.y <= - testValue)
        {
            if (lengthFlattenRate >= 3.6F)
            //if(lengthFlattenRate > 1.6F)   //Test용
            {
                hitAvailable = false;
                StartCoroutine(HitReset(coll));
                Instantiate(effect, coll.contacts[0].point, Quaternion.identity);
                _audio.Play();

                // 철괴의 길이(local X scale)가 3.6F 보다 크거나 같을 때 세 번 더 두드리면 중간 모델링으로 변환
                if (++hitCount == 3)
                {
                    GameObject _gameObject = Instantiate(changingModels, changingModelsHitPlace.position, changingModelsHitPlace.rotation);
                    DataManager.Instance.receivedTemp.Add(GetComponentInParent<HeatingCtrl>().objTemp);  // 중간 모델링으로 온도 전달
                    _gameObject.GetComponent<HeatingCtrl>().resetValues = true;
                    _gameObject.GetComponent<HeatingCtrl>().Awake();
                    hitAvailable = false;
                    _gameObject.GetComponent<ModelHitCtrl>().modelHitCount = IngotHitCtrl.hitCount;  // 중간 모델링으로 메질 횟수 전달
                    GetComponentsInParent<Transform>()[1].gameObject.SetActive(false);  // 철괴 비활성화
                }
                return;
            }

            // 철괴 스케일 값 변화정도 조절
            lengthFlattenRate += 0.1F;
            heightFlattenRate -= 0.05F;
            widthFlattenRate -= 0.05F;
            this.GetComponentsInParent<Transform>()[1].localScale = new Vector3(lengthFlattenRate, heightFlattenRate, widthFlattenRate);

            // 메질에 다른 파티클 효과 생성
            Instantiate(effect, coll.contacts[0].point, Quaternion.identity);
           
            // 메질 재실행 조건 코루틴 실행
            hitAvailable = false;
            StartCoroutine(HitReset(coll));
            
            // 메질에 따른 사운드 효과 재생
            _audio.Play();
        }
    }

    // 메질 재실행 조건 로직
    private IEnumerator HitReset(Collision coll)
    {
        while (!hitAvailable)
        {
            yield return null;
            distance = Vector3.Distance(this.transform.position, hammer.position);

            if (distance >= 0.3F)
                hitAvailable = true;
        }
        yield return null;
    }
}