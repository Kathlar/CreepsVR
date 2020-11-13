using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIElement, IClickable
{
    public Button button { get; private set; }
    public Image image { get; private set; }

    public bool worksWhenGamePaused;

    protected override void Awake()
    {
        base.Awake();

        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public override void OnHoverStart()
    {
        base.OnHoverStart();
        if (!worksWhenGamePaused && Game.Paused) return;
        image.color = button.colors.highlightedColor;
    }

    public override void OnHoverEnd()
    {
        base.OnHoverEnd();
        image.color = button.colors.normalColor;
    }

    public void OnClick()
    {
        if (!worksWhenGamePaused && Game.Paused) return;
        button.onClick.Invoke();
    }
}
