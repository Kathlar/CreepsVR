using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoldierChoice : MonoBehaviour, IHoverOver, IClickable
{
    protected MeshRenderer meshRenderer;

    [HideInInspector] public CharacterSoldier character;

    bool isOn;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        meshRenderer.material = character.player.information.transparentMaterial;
    }

    public void SetState(bool on)
    {
        isOn = on;
        if (!on && meshRenderer)
            meshRenderer.material = character.player.information.transparentMaterial;
    }

    public void OnHoverStart()
    {
        if (Game.Paused) return;
        if (!isOn) return;
        meshRenderer.material = character.player.information.material;
    }

    public void OnClick()
    {
        if (!isOn) return;
        character.GetChosen();
    }

    public void OnHoverEnd()
    {
        if (Game.Paused) return;
        meshRenderer.material = character.player.information.transparentMaterial;
    }
}
