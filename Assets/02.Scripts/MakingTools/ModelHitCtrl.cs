using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHitCtrl : MonoBehaviour
{
    // 메질 재실행 조건 체크하는 변수
    private Transform hammer;
    private float distance = 0.0F;
    private bool modelHitAvailable = true;

    // 메질 횟수에 따른 모델링 변화 변수
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;
    public Mesh[] meshes;
    private int modelIdx = 0;
    public int modelHitCount = 0;
    public GameObject finalModelPrefab;

    // 메질에 따른 파티클 변수
    public GameObject effect;

    // 메질에 따른 사운드 효과 변수
    private AudioSource _audio;
    public AudioClip _audioClip;

    void Start()
    {
        hammer = GameObject.Find("HammerHitSide").transform;
        _audio = GetComponent<AudioSource>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    // 망치와 중간 모델링 충돌 판정 로직
    private void OnCollisionEnter(Collision coll)
    {
        // 충돌체 태그, 메질 재실행 조건, 모루 상단 자석효과, 메질 가능 온도(300.0F) 충족 여부 확인
        if (coll.collider.tag == "HAMMERHITSIDE" && this.tag == "CHANGINGMODEL" && modelHitAvailable 
            && !GetComponent<ModelCtrl>().isMagneticAvailable_MODEL && GetComponent<HeatingCtrl>().objTemp > 200.0F)
        {
            // 메질 횟수에 따라 모델링 총 10단계로 변화
            if (++modelHitCount == 7)
            {
                UpdateModel();
            }
            else if(modelHitCount == 10)
            {
                UpdateModel();
            }
            else if (modelHitCount == 13)
            {
                UpdateModel();
            }
            else if (modelHitCount == 16)
            {
                UpdateModel();
            }
            else if (modelHitCount == 19)
            {
                UpdateModel();
            }
            else if (modelHitCount == 22)
            {
                UpdateModel();
            }
            else if (modelHitCount == 25)
            {
                UpdateModel();
            }
            else if (modelHitCount == 28)
            {
                UpdateModel();
            }
            else if (modelHitCount == 31)
            {
                UpdateModel();
            }
            // 총 34회 두드리면 최종 모델링으로 변환
            else if (modelHitCount == 34)
            {
                GameObject finalModel = Instantiate(finalModelPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                finalModel.transform.Rotate(Vector3.up * 180);
                DataManager.Instance.receivedTemp[DataManager.Instance.idx] = this.GetComponent<HeatingCtrl>().objTemp;
                finalModel.GetComponent<HeatingCtrl>().resetValues = true;
                finalModel.GetComponent<HeatingCtrl>().Awake();
                this.gameObject.SetActive(false);
                return;
            }

            // 메질에 따른 파티클 효과 생성
            Instantiate(effect, coll.contacts[0].point, Quaternion.identity);

            // 메질에 따른 사운드 효과 재생
            _audio.PlayOneShot(_audioClip);

            // 메질 재실행 조건 코루틴 실행
            modelHitAvailable = false;
            StartCoroutine(HitReset());
        }
    }

    // 중간 모델링 단계별 교체 로직
    public void UpdateModel()
    {
        _meshFilter.mesh = meshes[modelIdx];
        _meshCollider.sharedMesh = meshes[modelIdx];
        modelIdx++;
    }

    // 메질 재실행 조건 로직
    private IEnumerator HitReset()
    {
        while (!modelHitAvailable)
        {
            yield return null;
            distance = Vector3.Distance(this.transform.position, hammer.position);
            if (distance >= 0.3F) modelHitAvailable = true;
        }
        yield return null;
    }
}
