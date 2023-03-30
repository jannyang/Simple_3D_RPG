using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 mouseDelta;
    private Rigidbody rig;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public LayerMask groundLayerMask;
    private Vector2 curMovementInput;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    //called when we move our mouse
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }

        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        
        if(context.phase == InputActionPhase.Started)
        {
            
            if (IsGrounded())
            {
                Debug.Log("Jump");
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }


    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;

        //assign Rigidbody velocity
        rig.velocity = dir;
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        //rotate the player left and right
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.Log(Physics.Raycast(rays[i],2f));
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down * 0.1f);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down * 0.1f);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down * 0.1f);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down * 0.1f);
    }
}
