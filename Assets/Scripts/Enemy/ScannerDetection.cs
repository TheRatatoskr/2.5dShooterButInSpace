using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerDetection : MonoBehaviour
{
    [SerializeField] private GameObject _mainShipObject;
    private IEnemy _mainShip;

    private void Start()
    {
        _mainShip = _mainShipObject.GetComponent<IEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _mainShip?.HandleScannerDetection(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _mainShip?.HandleScannerLostDetection(collision);
    }
}
