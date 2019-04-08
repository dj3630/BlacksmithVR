using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSelect_Intro : MonoBehaviour
{
    public Transform clickRot;

    private AudioSource _audio;

    public bool isClickAvailable = true;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void Click()
    {
        transform.rotation = clickRot.rotation;
        isClickAvailable = false;
        _audio.Play();
    }
}
