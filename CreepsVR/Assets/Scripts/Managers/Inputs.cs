using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Inputs : Singleton<Inputs>
{
    protected InputDevice leftHandDevice, rightHandDevice;

    public ControllerInputValues leftHand, rightHand;
    public static ControllerInputValues LeftHand { get { return Instance.leftHand; } }
    public static ControllerInputValues RightHand { get { return Instance.rightHand; } }

    protected override void SingletonAwake()
    {
        leftHandDevice = GetHand(InputDeviceCharacteristics.Left);
        if (leftHandDevice != default)
        {
            leftHand = new ControllerInputValues(leftHandDevice);
        }

        rightHandDevice = GetHand(InputDeviceCharacteristics.Right);
        if (rightHandDevice != default)
        {
            rightHand = new ControllerInputValues(rightHandDevice);
        }
    }

    private void Update()
    {
        if (leftHandDevice == default)
        {
            leftHandDevice = GetHand(InputDeviceCharacteristics.Left);
            if (leftHandDevice != default)
            {
                leftHand = new ControllerInputValues(leftHandDevice);
            }
        }
        else
            leftHand.Update();

        if (rightHandDevice == default)
        {
            rightHandDevice = GetHand(InputDeviceCharacteristics.Right);
            if (rightHandDevice != default)
            {
                rightHand = new ControllerInputValues(rightHandDevice);
            }
        }
        else
            rightHand.Update();
    }

    InputDevice GetHand(InputDeviceCharacteristics side)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics characteristics =
            InputDeviceCharacteristics.Controller | side;
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        return (devices.Count > 0 ? devices[0] : default);
    }
}

[System.Serializable]
public class ControllerInputValues
{
    private InputDevice device;

    private List<PlayerInputTypeBase> inputTypes;
    public PlayerInputBool primaryButton, secondaryButton, menuButton;
    public PlayerInputFloat trigger, grip;
    public PlayerInputVector2 joystick;
    public PlayerInputBool joystickTouch, joystickClick;

    public ControllerInputValues(InputDevice device)
    {
        this.device = device;
        inputTypes = new List<PlayerInputTypeBase>();

        inputTypes.Add((primaryButton = new PlayerInputBool()).
            Set(device, CommonUsages.primaryButton));
        inputTypes.Add((secondaryButton = new PlayerInputBool()).
            Set(device, CommonUsages.secondaryButton));
        inputTypes.Add((menuButton = new PlayerInputBool()).
            Set(device, CommonUsages.menuButton));

        inputTypes.Add((trigger = new PlayerInputFloat()).
            Set(device, CommonUsages.trigger));
        inputTypes.Add((grip = new PlayerInputFloat()).
            Set(device, CommonUsages.grip));

        inputTypes.Add((joystick = new PlayerInputVector2()).
            Set(device, CommonUsages.primary2DAxis));
        inputTypes.Add((joystickTouch = new PlayerInputBool()).
            Set(device, CommonUsages.primary2DAxisTouch));
        inputTypes.Add((joystickClick = new PlayerInputBool()).
            Set(device, CommonUsages.primary2DAxisClick));
    }

    public void Update()
    {
        foreach (PlayerInputTypeBase inputType in inputTypes)
        {
            inputType.Update();
        }
    }
}

public interface PlayerInputTypeBase
{
    void Update();
}

[System.Serializable]
public abstract class PlayerInputType<T> : PlayerInputTypeBase
{
    public InputDevice device { get; private set; }
    public InputFeatureUsage<T> usage;

    public PlayerInputType<T> Set(InputDevice device, InputFeatureUsage<T> usage)
    {
        this.device = device;
        this.usage = usage;
        return this;
    }

    public abstract void Update();
}

[System.Serializable]
public class PlayerInputBool : PlayerInputType<bool>
{
    [SerializeField]
    private bool wasPressed, isPressed, wasReleased;
    public bool WasPressed { get { return wasPressed; } }
    public bool IsPressed { get { return isPressed; } }
    public bool WasReleased { get { return wasReleased; } }

    public override void Update()
    {
        device.TryGetFeatureValue(usage, out bool value);
        wasPressed = !isPressed && value;
        wasReleased = isPressed && !value;
        isPressed = value;
    }

    public static implicit operator bool(PlayerInputBool input)
    {
        return input.isPressed;
    }
}

[System.Serializable]
public class PlayerInputFloat : PlayerInputType<float>
{
    [SerializeField]
    private float value, lastValue;
    public float Value { get { return value; } }
    public float LastValue { get { return lastValue; } }

    public override void Update()
    {
        device.TryGetFeatureValue(usage, out float newValue);
        lastValue = value;
        value = newValue;
    }

    public static implicit operator float(PlayerInputFloat input)
    {
        return input.value;
    }
}

[System.Serializable]
public class PlayerInputVector2 : PlayerInputType<Vector2>
{
    [SerializeField]
    private Vector2 value, lastValue;
    public Vector2 Value { get { return value; } }
    public Vector2 LastValue { get { return lastValue; } }

    public override void Update()
    {
        device.TryGetFeatureValue(usage, out Vector2 newValue);
        lastValue = value;
        value = newValue;
    }

    public static implicit operator Vector2(PlayerInputVector2 input)
    {
        return input.value;
    }
}