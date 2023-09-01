using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    BaseInput inputs;
    BaseInput.CartInputActions cartInput;

    Camera mainCam;

    [SerializeField]
    private GameObject crosshair,realCannon;

    [SerializeField]
    private float maxMoveDistance;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new();
        cartInput = inputs.CartInput;

        cartInput.Enable();

        cartInput.Shoot.performed += TestPrint;

        mainCam = Camera.main;
    }

    private void TestPrint(InputAction.CallbackContext obj)
    {
        Debug.Log("Test Shoot Performed");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseScreenPos =  cartInput.MousePosition.ReadValue<Vector2>();
        Vector3 mousePosAsVec3 = new Vector3(mouseScreenPos.x, mouseScreenPos.y,0);

        Vector3 mouseToWorld = mainCam.ScreenToWorldPoint(mousePosAsVec3);

        mouseToWorld.z = 0;

        crosshair.transform.position = mouseToWorld;

        float distance =  Vector3.Distance(mouseToWorld, realCannon.transform.position);

        realCannon.transform.position = Vector3.MoveTowards(realCannon.transform.position, mouseToWorld, maxMoveDistance * Time.deltaTime);
    }

}
