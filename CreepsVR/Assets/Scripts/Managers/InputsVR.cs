using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Manager holding information about all vr inputs used in the game.
/// </summary>
public class InputsVR : Singleton<InputsVR>
{
    protected InputDevice leftHandDevice, rightHandDevice;

    #region Input Classes
    [System.Serializable]
    public class VRControllerInputValues
    {
        private InputDevice device;

        private List<IPlayerInputBase> inputTypes;
        public PlayerVRInputBool primaryButton, secondaryButton, menuButton;
        public PlayerVRInputBool triggerButton;
        public PlayerVRInputFloat trigger, grip;
        public PlayerVRInputVector2 joystick;
        public PlayerVRInputBool joystickTouch, joystickClick;

        public VRControllerInputValues(InputDevice device)
        {
            this.device = device;
            inputTypes = new List<IPlayerInputBase>();

            inputTypes.Add((primaryButton = new PlayerVRInputBool()).
                Set(device, CommonUsages.primaryButton));
            inputTypes.Add((secondaryButton = new PlayerVRInputBool()).
                Set(device, CommonUsages.secondaryButton));
            inputTypes.Add((menuButton = new PlayerVRInputBool()).
                Set(device, CommonUsages.menuButton));

            inputTypes.Add((triggerButton = new PlayerVRInputBool()).
                Set(device, CommonUsages.triggerButton));

            inputTypes.Add((trigger = new PlayerVRInputFloat()).
                Set(device, CommonUsages.trigger));
            inputTypes.Add((grip = new PlayerVRInputFloat()).
                Set(device, CommonUsages.grip));

            inputTypes.Add((joystick = new PlayerVRInputVector2()).
                Set(device, CommonUsages.primary2DAxis));
            inputTypes.Add((joystickTouch = new PlayerVRInputBool()).
                Set(device, CommonUsages.primary2DAxisTouch));
            inputTypes.Add((joystickClick = new PlayerVRInputBool()).
                Set(device, CommonUsages.primary2DAxisClick));
        }

        public void Update()
        {
            foreach (IPlayerInputBase inputType in inputTypes)
            {
                inputType.Update();
            }
        }
    }

    public abstract class PlayerVRInputType<T> : PlayerInputType<T>
    {
        public InputDevice device { get; private set; }
        public InputFeatureUsage<T> usage;

        public PlayerInputType<T> Set(InputDevice device, InputFeatureUsage<T> usage)
        {
            this.device = device;
            this.usage = usage;
            return this;
        }
    }

    [System.Serializable]
    public class PlayerVRInputBool : PlayerVRInputType<bool>
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

        public static implicit operator bool(PlayerVRInputBool input)
        {
            return input.isPressed;
        }
    }

    [System.Serializable]
    public class PlayerVRInputFloat : PlayerVRInputType<float>
    {
        [SerializeField]
        private float value, lastValue;
        public float Value { get { return value; } }
        public float LastValue { get { return lastValue; } }

        public override void Update()
        {
            device.TryGetFeatureValue(usage, out float newValue);
            if (newValue < .05f) newValue = 0;
            else if (newValue > .99f) newValue = 1;
            lastValue = value;
            value = newValue;
        }

        public static implicit operator float(PlayerVRInputFloat input)
        {
            return input.value;
        }
    }

    [System.Serializable]
    public class PlayerVRInputVector2 : PlayerVRInputType<Vector2>
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

        public static implicit operator Vector2(PlayerVRInputVector2 input)
        {
            return input.value;
        }
    }
    #endregion

    public VRControllerInputValues leftHand, rightHand;
    private Dictionary<HorizontalSide, VRControllerInputValues> hands =
        new Dictionary<HorizontalSide, VRControllerInputValues>();
    public static VRControllerInputValues LeftHand { get { return Instance.leftHand; } }
    public static VRControllerInputValues RightHand { get { return Instance.rightHand; } }
    public static Dictionary<HorizontalSide, VRControllerInputValues> Hands
        { get { return Instance.hands; } }

    protected override void SingletonAwake()
    {
        leftHandDevice = GetHand(InputDeviceCharacteristics.Left);
        if (leftHandDevice != default)
        {
            leftHand = new VRControllerInputValues(leftHandDevice);
            hands.Add(HorizontalSide.left, leftHand);
        }

        rightHandDevice = GetHand(InputDeviceCharacteristics.Right);
        if (rightHandDevice != default)
        {
            rightHand = new VRControllerInputValues(rightHandDevice);
            hands.Add(HorizontalSide.right, rightHand);
        }
    }

    private void Update()
    {
        if (leftHandDevice == default)
        {
            leftHandDevice = GetHand(InputDeviceCharacteristics.Left);
            if (leftHandDevice != default)
            {
                leftHand = new VRControllerInputValues(leftHandDevice);
                hands.Add(HorizontalSide.left, leftHand);
            }
        }
        else
            leftHand.Update();

        if (rightHandDevice == default)
        {
            rightHandDevice = GetHand(InputDeviceCharacteristics.Right);
            if (rightHandDevice != default)
            {
                rightHand = new VRControllerInputValues(rightHandDevice);
                hands.Add(HorizontalSide.right, rightHand);
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
