using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHitCtrl_FINAL : MonoBehaviour
{
    private Transform hammer;

    private AudioSource _audio;

    public AudioClip[] _audioClip;

    private bool isPlayed = false;

    private float distance = 0.0F;

    private bool modelHitAvailable = true;

    void Start()
    {
        hammer = GameObject.Find("HammerHitSide").transform;
        _audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "HAMMERHITSIDE" && this.tag == "FINALMODEL" && modelHitAvailable)
        {
            _audio.PlayOneShot(_audioClip[0], 2.0F);
            modelHitAvailable = false;
            StartCoroutine(HitReset());
        }
    }

    private IEnumerator HitReset()
    {
        while (!modelHitAvailable)
        {
            yield return null;
            distance = Vector3.Distance(this.transform.position, hammer.position);
            if (distance >= 0.4F) modelHitAvailable = true;
        }
        yield return null;
    }

    public void SteamSoundOn(float volume)
    {
        if (!isPlayed)
        {
            _audio.PlayOneShot(_audioClip[1], volume);
            isPlayed = true;
        }
    }

    public void SteamSoundOff()
    {
        _audio.Pause
    }
}
