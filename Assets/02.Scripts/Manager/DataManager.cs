using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public List<float> receivedTemp = new List<float>();

    public int idx = -1;

    private void Start()
    {
        Instance = this;
    }
}
