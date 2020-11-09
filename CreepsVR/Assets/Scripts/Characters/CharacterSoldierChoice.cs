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
        meshRenderer.material = Database.PlayerInfos[character.playerNumber].transparentMaterial;
    }

    public void TurnOn()
    {
        isOn = true;
    }

    public void TurnOff()
    {
        isOn = false;
        if(meshRenderer)
            meshRenderer.material = Database.PlayerInfos[character.playerNumber].transparentMaterial;
    }

    public void OnHoverStart()
    {
        if (Game.Paused) return;
        if (!isOn) return;
        meshRenderer.material = Database.PlayerInfos[character.playerNumber].material;
    }

    public void OnClick()
    {
        if (!isOn) return;
        character.GetChosen();
    }

    public void OnHoverEnd()
    {
        if (Game.Paused) return;
        meshRenderer.material = Database.PlayerInfos[character.playerNumber].transparentMaterial;
    }
}
