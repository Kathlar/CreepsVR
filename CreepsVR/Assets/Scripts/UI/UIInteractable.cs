using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractable : MonoBehaviour, IHoverOver, IClickable
{

    public Button button { get; private set; }
    public Image image { get; private set; }

    public GameObject tooltipWindow;

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
        if (tooltipWindow) tooltipWindow.gameObject.SetActive(true);
    }

    public void OnHoverEnd()
    {
        image.color = button.colors.normalColor;
        if (tooltipWindow) tooltipWindow.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (!worksWhenGamePaused && Game.Paused) return;
        button.onClick.Invoke();
        if (tooltipWindow) tooltipWindow.gameObject.SetActive(false);
    }
}
