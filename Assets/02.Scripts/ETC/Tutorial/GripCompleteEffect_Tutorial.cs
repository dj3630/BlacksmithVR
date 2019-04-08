using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripCompleteEffect_Tutorial : MonoBehaviour
{
    public GameObject airEffect;
    public GameObject splashEffect;
    public GameObject firework;
    public GameObject endingWindow;
    public GameObject controller_collider;
    public GameObject controller_pointer;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void ParticlePlayBack()
    {
        Instantiate(airEffect, transform.position, Quaternion.identity);
        Instantiate(splashEffect, transform.position, Quaternion.identity);
        _audio.Play();
        StartCoroutine(UserWindowPop());
    }

    private IEnumerator UserWindowPop()
    {
        yield return new WaitForSeconds(1.0F);
        endingWindow.SetActive(true);
        controller_collider.SetActive(false);
        controller_pointer.SetActive(true);
    }
}
