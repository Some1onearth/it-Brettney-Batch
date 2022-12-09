using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyMode : Joystick
{
    #region Variables
    [SerializeField] private float _moveThreshold = 1;
    [SerializeField] private JoystickType _joystickType = JoystickType.Fixed;
    private Vector2 _fixedPosition = Vector2.zero;
    #endregion
    #region Properties
    public float MoveThreshold { get => _moveThreshold; set => _moveThreshold = Mathf.Abs((value)); }
    #endregion

    public void SetMode(JoystickType joystickType)
    {
        //sets joystickType based on the passed in joystickType
        _joystickType = joystickType;
        //if joystickType is Fixed
        if (joystickType == JoystickType.Fixed)
        {
            //sets backgrounds anchored position to fixedPosition 
            background.anchoredPosition = _fixedPosition;
            //enables the background gameObject
            background.gameObject.SetActive(true);
        }
        else //if not set to Fixed
        {
            //disables the background gameObject
            background.gameObject.SetActive(false);
        }
    }

    protected override void Start()
    {
        //runs base Start method from inherited class Joystick
        base.Start();
        //fixedPosition is set to background anchored position
        _fixedPosition = background.anchoredPosition;
        //runs SetMode function with joystickType passed in
        SetMode(_joystickType);
    }

    public void Update()
    {
        //checks if joystickType is set to fixed && background gameObject local active state is set to false (each frame)
        if (_joystickType == JoystickType.Fixed && background.gameObject.activeSelf == false)
        {
            //runs SetMode function with joystickType passed in
            SetMode(_joystickType);
        }
    }

    public override void OnPointerDown(PointerEventData eventData) //runs when detecting touch input
    {
        //if joystickType is not set to Fixed
        if (_joystickType != JoystickType.Fixed)
        {
            //background anchored position is set to ScreenPointToAnchoredPosition from Joystick
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            //background position is set to the position passed in from the touch input event
            background.position = eventData.position;
            //background gameObject is enabled
            background.gameObject.SetActive(true);
        }
        //runs base OnPointerDown function from inherited Joystick class
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData) //runs when detecting touch input removal
    {
        //if joystickType is set to fixed
        if (_joystickType != JoystickType.Fixed)
        {
            //disabled background gameObject
            background.gameObject.SetActive(false);
        }
        //runs base OnPointerUp function from inherited Joystick class
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        //if joystickType is set to Dynamic && the passed in magnitude is above MoveThreshold
        if (_joystickType == JoystickType.Dynamic && magnitude > MoveThreshold)
        {
            //creates a Vector2 difference
            Vector2 difference = normalised * (magnitude - MoveThreshold) * radius;
            //background anchored position is increased by difference Vector2 as to make it appear at the position of the touch input
            background.anchoredPosition += difference;
        }
        //runs base HandleInput function from inherited Joystick class
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType
{
    Fixed, Floating, Dynamic
}
