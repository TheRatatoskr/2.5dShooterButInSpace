using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleLaser : MonoBehaviour
{
    [SerializeField] private float _shotTime;
    [SerializeField] private float _startTime;

    private void Start()
    {
        Destroy(this.gameObject, _shotTime);
    }

}
