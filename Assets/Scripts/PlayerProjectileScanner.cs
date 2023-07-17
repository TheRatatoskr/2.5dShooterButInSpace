using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScanner : MonoBehaviour
{
    [SerializeField] private GameObject _mainProjectileObject;
    private IPlayerProjectile _mainProjectile;

    private void Start()
    {
        _mainProjectile = _mainProjectileObject.GetComponent<IPlayerProjectile>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("scanner scanned a thing");
        _mainProjectile?.HandleScannerDetectedSomething(collision);
    }
}
