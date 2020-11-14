using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : UIElement, IClickable
{
    public Toggle toggle { get; private set; }
    public Image onImage;

    public bool worksWhenGamePaused;

    protected override void Awake()
    {
        base.Awake();

        toggle = GetComponent<Toggle>();
        onImage.gameObject.SetActive(toggle.isOn);
    }

    public void OnClick()
    {
        if (!worksWhenGamePaused && Game.Paused) return;
        toggle.isOn = !toggle.isOn;
        onImage.gameObject.SetActive(toggle.isOn);

        toggle.onValueChanged.Invoke(toggle.isOn);
    }

    public void ChangeValue(bool on)
    {
        toggle.isOn = on;
        onImage.gameObject.SetActive(toggle.isOn);
    }
}
