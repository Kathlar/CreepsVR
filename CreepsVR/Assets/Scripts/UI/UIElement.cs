using UnityEngine;
using UnityEngine.UI;

public abstract class UIElement : MonoBehaviour, IHoverOver
{
    public GameObject tooltipWindow;
    public Text tooltipText { get; private set; }

    protected virtual void Awake()
    {
        if (tooltipWindow) tooltipText = tooltipWindow.GetComponentInChildren<Text>();
    }

    public virtual void OnHoverStart()
    {
        if (tooltipWindow) tooltipWindow.gameObject.SetActive(true);
    }

    public virtual void OnHoverEnd()
    {
        if (tooltipWindow) tooltipWindow.gameObject.SetActive(false);
    }
}
