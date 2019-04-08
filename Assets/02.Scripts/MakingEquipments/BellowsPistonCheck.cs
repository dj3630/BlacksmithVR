using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellowsPistonCheck : MonoBehaviour
{
    private bool loopCheck = false;
    private Vector3 initPos;
    private bool isPlayed = false;
    private AudioSource audio;

    public int loopCount = 0;
    public AudioClip[] bellowsSound;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        initPos = this.transform.localPosition;
    }

    private void Update()
    {
        BellowsSound();
    }

    public void BellowsSound()
    {
        if (transform.localPosition.z < 0.15 && !isPlayed)
        {
            audio.PlayOneShot(bellowsSound[0]);
            isPlayed = true;
        }
        else if (transform.localPosition.z >= 0.15 && isPlayed)
        {
            audio.PlayOneShot(bellowsSound[1]);
            isPlayed = false;
        }
    }

    public void Reset()
    {
        StartCoroutine(ResetPos());
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "FORWARDBLOCK")
        {
            loopCheck = true;
        }
        else if(coll.collider.tag == "BACKWARDBLOCK")
        {
            if (loopCheck)
            {
                loopCount++;
                GameObject.FindWithTag("FORGE").GetComponent<ForgeCtrl>().SendMessage("SetTemp");
            } 
            loopCheck = false;
        }
    }

    public IEnumerator ResetPos()
    {
        float t = 0.0F;
        float resetSpeed = 3.0F;

        while (t <= 1)
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, initPos, t * resetSpeed);
            t += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
