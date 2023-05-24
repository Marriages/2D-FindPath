using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestRaycast : TestBase
{
    protected override void Test1(InputAction.CallbackContext obj)
    {
        Debug.Log("Left Ray");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 1f);
        if(hit.collider != null )
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
    protected override void Test2(InputAction.CallbackContext obj)
    {
        Debug.Log("Up Ray");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
    protected override void Test3(InputAction.CallbackContext obj)
    {
        Debug.Log("Right Ray");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
    protected override void Test4(InputAction.CallbackContext obj)
    {
        Debug.Log("Down Ray");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
