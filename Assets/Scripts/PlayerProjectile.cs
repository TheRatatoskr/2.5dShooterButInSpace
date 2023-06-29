using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float _moveSpeed = 1f;
    private float _highestYPoint = 1f;

    public void Initialize(float moveSpeed, float topOfCamera)
    {
        _moveSpeed = moveSpeed;
        _highestYPoint = topOfCamera;

    }
    void Update()
    {
        transform.Translate(transform.up * _moveSpeed * Time.deltaTime);
        if (transform.position.y > _highestYPoint)
        {
            Destroy(this.gameObject);
        }
    }
}
