using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : Singleton<Inputs>
{
    #region Input Classes
    [System.Serializable]
    public class PlayerInputBool : PlayerInputType<bool>
    {
        [SerializeField] private KeyCode key = KeyCode.None;

        [SerializeField]
        private bool wasPressed, isPressed, wasReleased;
        public bool WasPressed { get { return wasPressed; } }
        public bool IsPressed { get { return isPressed; } }
        public bool WasReleased { get { return wasReleased; } }

        public PlayerInputBool Set(KeyCode newKey)
        {
            key = newKey;
            return this;
        }

        public override void Update()
        {
            wasPressed = Input.GetKeyDown(key);
            isPressed = Input.GetKey(key);
            wasReleased = Input.GetKeyUp(key);
        }

        public static implicit operator bool(PlayerInputBool input)
        {
            return input.isPressed;
        }
    }

    [System.Serializable]
    public class PlayerInputAxis : PlayerInputType<float>
    {
        [SerializeField] private string axisName;

        [SerializeField]
        private float value, lastValue;
        public float Value { get { return value; } }
        public float LastValue { get { return lastValue; } }

        public PlayerInputAxis Set(string newAxisName)
        {
            axisName = newAxisName;
            return this;
        }

        public override void Update()
        {
            lastValue = value;
            value = Input.GetAxis(axisName);
        }

        public static implicit operator float(PlayerInputAxis input)
        {
            return input.value;
        }
    }
    #endregion

    private List<IPlayerInputBase> inputTypes;
    [SerializeField] private PlayerInputBool leftMouse, rightMouse, middleMouse;
    [SerializeField] private PlayerInputBool space, leftShift, enter, escape;
    [SerializeField] private PlayerInputAxis mainHorizontal, mainVertical, secondaryHorizontal, secondaryVertical;
    [SerializeField] private PlayerInputBool rKey, pKey;

    #region Input Static Getters
    public static PlayerInputBool LeftMouse { get { return Instance.leftMouse; } }
    public static PlayerInputBool RightMouse { get { return Instance.rightMouse; } }
    public static PlayerInputBool MiddleMouse { get { return Instance.middleMouse; } }

    public static PlayerInputBool Space { get { return Instance.space; } }
    public static PlayerInputBool LeftShift { get { return Instance.leftShift; } }
    public static PlayerInputBool Enter { get { return Instance.enter; } }
    public static PlayerInputBool Escape { get { return Instance.escape; } }

    public static PlayerInputAxis MainHorizontal { get { return Instance.mainHorizontal; } }
    public static PlayerInputAxis MainVertical { get { return Instance.mainVertical; } }
    public static PlayerInputAxis SecondaryHorizontal { get { return Instance.secondaryHorizontal; } }
    public static PlayerInputAxis SecondaryVertical { get { return Instance.secondaryVertical; } }

    public static PlayerInputBool RKey { get { return Instance.rKey; } }
    public static PlayerInputBool PKey { get { return Instance.pKey; } }
    #endregion

    protected override void SingletonAwake()
    {
        inputTypes = new List<IPlayerInputBase>();

        inputTypes.Add((leftMouse = new PlayerInputBool()).Set(KeyCode.Mouse0));
        inputTypes.Add((rightMouse = new PlayerInputBool()).Set(KeyCode.Mouse1));
        inputTypes.Add((middleMouse = new PlayerInputBool()).Set(KeyCode.Mouse2));

        inputTypes.Add((space = new PlayerInputBool()).Set(KeyCode.Space));
        inputTypes.Add((leftShift = new PlayerInputBool()).Set(KeyCode.LeftShift));
        inputTypes.Add((enter = new PlayerInputBool()).Set(KeyCode.KeypadEnter));
        inputTypes.Add((escape = new PlayerInputBool()).Set(KeyCode.Escape));

        inputTypes.Add((mainHorizontal = new PlayerInputAxis()).Set("Horizontal"));
        inputTypes.Add((mainVertical = new PlayerInputAxis()).Set("Vertical"));
        inputTypes.Add((secondaryHorizontal = new PlayerInputAxis()).Set("Mouse X"));
        inputTypes.Add((secondaryVertical = new PlayerInputAxis()).Set("Mouse Y"));

        inputTypes.Add((pKey = new PlayerInputBool()).Set(KeyCode.P));
        inputTypes.Add((rKey = new PlayerInputBool()).Set(KeyCode.R));
    }

    protected void Update()
    {
        foreach (IPlayerInputBase inputType in inputTypes)
        {
            inputType.Update();
        }
    }
}

public interface IPlayerInputBase
{
    void Update();
}

public abstract class PlayerInputType<T> : IPlayerInputBase
{
    public abstract void Update();
}