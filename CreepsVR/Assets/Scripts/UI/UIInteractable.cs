using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractable : MonoBehaviour, IHoverOver, IClickable
{
    public Button button { get; private set; }
    public Image image { get; private set; }

    public bool worksWhenGamePaused;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void OnHoverStart()
    {
        if (!worksWhenGamePaused && Game.Paused) return;
        image.color = button.colors.highlightedColor;
    }

    public void OnHoverEnd()
    {
        image.color = button.colors.normalColor;
    }

    public void OnClick()
    {
        if (!worksWhenGamePaused && Game.Paused) return;
        button.onClick.Invoke();
    }

    public void DebugTest()
    {
        Debug.Log("TEST");
    }
}
