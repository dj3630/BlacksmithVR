using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotHitCtrl : MonoBehaviour
{
    private float lengthFlattenRate = 1.6F;
    private float heightFlattenRate = 2.0F;
    private float widthFlattenRate = 1.6F;

    private float distance = 0.0F;
    public static bool hitAvailable = true;

    private Transform hammer;
    private AudioSource _audio;

    public GameObject changingModels;
    public Transform changingModelsHitPlace;

    public static int hitCount = 0;

    public GameObject effect;

    private void Start()
    {
        hammer = GameObject.Find("HammerHitSide").transform;
        _audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "HAMMERHITSIDE" && this.tag == "INGOTHITSIDE" && hitAvailable && !GetComponentInParent<IngotCtrl>().isMagneticAvailable_INGOT)
        {
            if (lengthFlattenRate >= 3.6F)
            //if(lengthFlattenRate > 1.6F)   //Test용
            {
                hitAvailable = false;
                StartCoroutine(HitReset());
                Instantiate(effect, coll.contacts[0].point, Quaternion.identity);
                _audio.Play();

                if (++hitCount == 3)
                {
                    GameObject _gameObject = Instantiate(changingModels, changingModelsHitPlace.position, changingModelsHitPlace.rotation);
                    DataManager.Instance.receivedTemp.Add(GetComponentInParent<HeatingCtrl>().objTemp);
                    _gameObject.GetComponent<HeatingCtrl>().resetValues = true;
                    _gameObject.GetComponent<HeatingCtrl>().Awake();
                    hitAvailable = false;
                    _gameObject.GetComponent<ModelHitCtrl>().modelHitCount = IngotHitCtrl.hitCount;
                    GetComponentsInParent<Transform>()[1].gameObject.SetActive(false);
                }
                return;
            }

            lengthFlattenRate += 0.1F;
            heightFlattenRate -= 0.05F;
            widthFlattenRate -= 0.05F;

            Instantiate(effect, coll.contacts[0].point, Quaternion.identity);

            this.GetComponentsInParent<Transform>()[1].localScale = new Vector3(lengthFlattenRate, heightFlattenRate, widthFlattenRate);
            hitAvailable = false;
            StartCoroutine(HitReset());
            _audio.Play();
        }
    }

    private IEnumerator HitReset()
    {
        while (!hitAvailable)
        {
            yield return null;
            distance = Vector3.Distance(this.transform.position, hammer.position);
            if (distance >= 0.8F) hitAvailable = true;
        }
        yield return null;
    }
}
