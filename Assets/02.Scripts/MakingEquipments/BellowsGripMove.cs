using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellowsGripMove : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _localX = 0;
    private float _localY = 0;
    private float _localZ = 0;
    public bool _freezeAlongX = false;
    public bool _freezeAlongY = false;
    public bool _freezeAlongZ = false;

    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _localX = transform.localPosition.x;
        _localY = transform.localPosition.y;
    }

    void Update()
    {
        _localZ = transform.localPosition.z;

        if (_freezeAlongX) _localX = 0;
        if (_freezeAlongY) _localY = 0;
        if (_freezeAlongZ) _localZ = 0;
        transform.localPosition = new Vector3(_localX, _localY, _localZ);
    }
}
