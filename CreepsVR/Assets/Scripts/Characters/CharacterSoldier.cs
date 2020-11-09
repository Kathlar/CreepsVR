using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoldier : Character
{
    public CharacterController controller { get; private set; }

    public GameObject regularModeObject;
    [HideInInspector] public CharacterSoldierChoice choice;

    [HideInInspector] public int playerNumber;

    public GameObject weaponSelectionButtonPrefab;
    public RectTransform weaponSelectionGrid;

    private Item spawnedItem;
    private bool holdingWeapon, attacking;
    public float moveSpeed = 6;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        choice = GetComponentInChildren<CharacterSoldierChoice>();
        choice.character = this;
        SetChoice(false);

        canvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 moveVector = Physics.gravity;
        if (holdingWeapon && !attacking)
        {
            Transform cameraTransform = Game.Player.mainCamera.transform;
            Vector2 moveInputValue = new Vector2(Mathf.Clamp(Inputs.MainHorizontal +
                InputsVR.LeftHand.joystick.Value.x, -1, 1), Mathf.Clamp(Inputs.MainVertical +
                InputsVR.LeftHand.joystick.Value.y, -1, 1));
            Vector3 walkVector = cameraTransform.forward.FlatY() * moveInputValue.y +
                cameraTransform.right.FlatY() * moveInputValue.x;
            moveVector += walkVector * moveSpeed;
        }
        controller.Move(moveVector * Time.deltaTime);
    }

    public void SetChoice(bool on)
    {
        choice.gameObject.SetActive(on);
    }

    public void ChooseCharacter()
    {
        regularModeObject.SetActive(false);
        SetAsPlayer();
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.weaponChoice);

        canvas.gameObject.SetActive(true);
        foreach(var weapon in Database.WeaponPrefabs)
        {
            WeaponSelectionIcon icon = Instantiate(weaponSelectionButtonPrefab, 
                weaponSelectionGrid).GetComponent<WeaponSelectionIcon>();
            icon.weaponPrefab = weapon;
            icon.soldier = this;
            Item item = weapon.GetComponent<Item>();
            icon.iconImage.sprite = item.icon;
            icon.nameText.text = item.itemName;
        }
    }

    public void ChooseWeapon(WeaponSelectionIcon icon)
    {
        holdingWeapon = true;
        canvas.gameObject.SetActive(false);
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.movement);
    }
}
