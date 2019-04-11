using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; } //싱글턴

    public int Score { get; set; }

    public GameObject hitPosEffect;

    private Action callback;
    private int[] hitSeq;
    private int hitSeqIdx;
    private Transform[] hitTr;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(gameObject);
    }

    public void StartHitEffect(Transform[] tr, int[] idx, Action func)
    {
        Debug.Log("StartHitEffect");
        callback = func;
        hitSeq = idx;
        hitTr = tr;
        hitSeqIdx = 0;
        GameObject effect = Instantiate(hitPosEffect, tr[idx[hitSeqIdx]]);
        effect.transform.SetParent(tr[idx[hitSeqIdx]]);
    }

    public void HitCallback(int score)
    {
        Debug.Log("HitCallback idx : " + hitSeqIdx);
        Score += score;
        hitSeqIdx++;
        callback();
        if(hitSeqIdx <= hitSeq.Length - 1)
        {
            GameObject effect = Instantiate(hitPosEffect, hitTr[hitSeq[hitSeqIdx]]);
            effect.transform.SetParent(hitTr[hitSeq[hitSeqIdx]]);
        }
    }
}

