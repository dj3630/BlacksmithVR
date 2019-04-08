using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHitCtrl : MonoBehaviour
{
    private Transform hammer;

    private AudioSource _audio;

    public AudioClip _audioClip;

    private AudioClip playingClip;

    private float distance = 0.0F;

    public int modelHitCount = 0;

    private bool modelHitAvailable = true;

    private int modelIdx = 0;

    public Mesh[] meshes;

    private MeshFilter _meshFilter;

    private MeshRenderer _meshRenderer;

    private MeshCollider _meshCollider;

    public GameObject finalModelPrefab;

    public GameObject effect;

    void Start()
    {
        hammer = GameObject.Find("HammerHitSide").transform;
        _audio = GetComponent<AudioSource>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "HAMMERHITSIDE" && this.tag == "CHANGINGMODEL" && modelHitAvailable && !GetComponent<ModelCtrl>().isMagneticAvailable_MODEL)
        {
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

            Instantiate(effect, coll.contacts[0].point, Quaternion.identity);
            _audio.PlayOneShot(_audioClip);
            modelHitAvailable = false;
            StartCoroutine(HitReset());
        }
    }

    public void UpdateModel()
    {
        _meshFilter.mesh = meshes[modelIdx];
        _meshCollider.sharedMesh = meshes[modelIdx];
        modelIdx++;
    }

    private IEnumerator HitReset()
    {
        while (!modelHitAvailable)
        {
            yield return null;
            distance = Vector3.Distance(this.transform.position, hammer.position);
            if (distance >= 0.8F) modelHitAvailable = true;
        }
        yield return null;
    }
}
