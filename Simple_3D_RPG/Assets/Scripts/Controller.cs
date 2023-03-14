using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public VirtualJoystick virtualJoystick;

    [SerializeField]
    private float moveSpeed;

    private void Update()
    {
        transform.Translate(moveSpeed * virtualJoystick.inputDirection.x, 0, moveSpeed * virtualJoystick.inputDirection.y);
    }
}
