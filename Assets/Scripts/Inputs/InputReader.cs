using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IShipControlsActions
{
    private PlayerInput input;

    public event Action<bool> PrimaryFirePressed;
    public event Action<Vector2> MovementKeysChanged;

    private void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInput();
            input.ShipControls.SetCallbacks(this);
        }
        input.ShipControls.Enable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log (" from onfire, " + context.performed.ToString ());
        //when we press the button
        if (context.performed)
        {
            PrimaryFirePressed?.Invoke(true);
        }
        //if we let got of the button
        else if (context.canceled)
        {
            PrimaryFirePressed?.Invoke(false);
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log("from input reader movement keys. " + context.ReadValue<Vector2>().ToString());
        MovementKeysChanged?.Invoke(context.ReadValue<Vector2>());
    }
}
