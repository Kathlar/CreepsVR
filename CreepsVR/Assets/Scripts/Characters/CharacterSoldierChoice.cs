using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoldierChoice : MonoBehaviour, IHoverOver, IClickable
{
    protected MeshRenderer meshRenderer;

    [HideInInspector] public CharacterSoldier character;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        meshRenderer.material = Database.PlayerInfos[character.playerNumber].transparentMaterial;
    }

    public void OnHoverStart()
    {
        meshRenderer.material = Database.PlayerInfos[character.playerNumber].material;
    }

    public void OnClick()
    {
        character.ChooseCharacter();
    }

    public void OnHoverEnd()
    {
        meshRenderer.material = Database.PlayerInfos[character.playerNumber].transparentMaterial;
    }
}
