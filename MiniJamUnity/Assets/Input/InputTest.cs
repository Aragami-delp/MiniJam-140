using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    BaseInput inputs;
    BaseInput.CartInputActions cartInput;
    // Start is called before the first frame update
    void Start()
    {
        inputs = new();
        cartInput = inputs.CartInput;

        cartInput.Enable();

        cartInput.Shoot.performed += TestPrint;
    }

    private void TestPrint(InputAction.CallbackContext obj)
    {
        Debug.Log("Test Shoot Performed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
