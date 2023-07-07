using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleLaser : MonoBehaviour
{
    [SerializeField] private float _shotTime;

    private void Start()
    {
        Destroy(this.gameObject, _shotTime);
    }

}
