using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackLaser : MonoBehaviour
{
    [SerializeField] private float _destroyTimer;

    void Start()
    {
        Destroy(gameObject, _destroyTimer);
    }

}
