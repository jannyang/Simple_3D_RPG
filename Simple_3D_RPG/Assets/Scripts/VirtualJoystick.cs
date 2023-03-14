using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    
    [SerializeField]
    private float cor;

    [SerializeField, Range(10, 150)]
    private float leverRange;

    public Vector2 inputDirection;
    private bool isInput;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(isInput)
        {
            InputControlVector();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControllJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControllJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        inputDirection = Vector2.zero;
        isInput = false;
    }

    private void ControllJoystickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = cor * inputVector;
        inputDirection = inputVector / leverRange;
        Debug.Log(inputDirection);
    }

    private void InputControlVector()
    {

    }
}
