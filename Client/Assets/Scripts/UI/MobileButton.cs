using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType
    {
        Up, Down, Left, Right, Attack
    }

    [SerializeField] private ButtonType button;
    [SerializeField] private bool _isPressed;
    public PlayerController _pc;

    private void Update()
    {
        Debug.Log(_isPressed);

        if (_isPressed)
        {
            switch (button)
            {
                case ButtonType.Up:
                    _pc.upButton = true;
                    break;
                case ButtonType.Down:
                    _pc.downButton = true;
                    break;
                case ButtonType.Left:
                    _pc.leftButton = true;
                    break;
                case ButtonType.Right:
                    _pc.rightButton = true;
                    break;
                case ButtonType.Attack:
                    _pc.attackButton = true;
                    break;
            }
        }
        else
        {
            switch (button)
            {
                case ButtonType.Up:
                    _pc.upButton = false;
                    break;
                case ButtonType.Down:
                    _pc.downButton = false;
                    break;
                case ButtonType.Left:
                    _pc.leftButton = false;
                    break;
                case ButtonType.Right:
                    _pc.rightButton = false;
                    break;
                case ButtonType.Attack:
                    _pc.attackButton = false;
                    break;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    }
}
