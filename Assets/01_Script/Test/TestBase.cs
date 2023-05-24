using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    InputController inputActions;
    private void Awake()
    {
        inputActions = new InputController();
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += Test1;
        inputActions.Test.Test2.performed += Test2;
        inputActions.Test.Test3.performed += Test3;
        inputActions.Test.Test4.performed += Test4;
        inputActions.Test.Test5.performed += Test5;
    }
    private void OnDisable()
    {
        inputActions.Test.Test5.performed -= Test5;
        inputActions.Test.Test4.performed -= Test4;
        inputActions.Test.Test3.performed -= Test3;
        inputActions.Test.Test2.performed -= Test2;
        inputActions.Test.Test1.performed -= Test1;
        inputActions.Test.Disable();
    }

    virtual protected void Test1(InputAction.CallbackContext obj)
    {
    }
    virtual protected void Test2(InputAction.CallbackContext obj)
    {
    }
    virtual protected void Test3(InputAction.CallbackContext obj)
    {
    }
    virtual protected void Test4(InputAction.CallbackContext obj)
    {
    }
    virtual protected void Test5(InputAction.CallbackContext obj)
    {
    }
}
