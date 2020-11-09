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
    private List<GameObject> weaponSelectionIcons = new List<GameObject>();

    private Item spawnedItem;
    private bool holdingWeapon, attacking;

    public SoldierInfoWindow infoWindow { get; private set; }
    private Vector3 startInfoWindowScale;

    public int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        infoWindow = GetComponentInChildren<SoldierInfoWindow>();
        startInfoWindowScale = infoWindow.transform.parent.localScale;

        choice = GetComponentInChildren<CharacterSoldierChoice>();
        choice.character = this;
        SetChoice(false);

        canvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        infoWindow.SetUp(playerNumber);
        SetHealth(maxHealth);
    }

    private void Update()
    {
        Vector3 moveVector = Physics.gravity;
        if (isPlayer && holdingWeapon)
        {
            if(!attacking)
            {
                Transform cameraTransform = Game.Player.mainCamera.transform;
                Vector2 moveInputValue = new Vector2(Mathf.Clamp(Inputs.MainHorizontal +
                    InputsVR.LeftHand.joystick.Value.x, -1, 1), Mathf.Clamp(Inputs.MainVertical +
                    InputsVR.LeftHand.joystick.Value.y, -1, 1));
                Vector3 walkVector = cameraTransform.forward.FlatY() * moveInputValue.y +
                    cameraTransform.right.FlatY() * moveInputValue.x;
                moveVector += walkVector * moveSpeed;

                if(InputsVR.LeftHand.triggerButton.WasPressed)
                {
                    attacking = true;
                    spawnedItem.TurnOn();
                }
            }
            else
            {
                if (InputsVR.RightHand.triggerButton.WasPressed) spawnedItem.UseStart();
                else if (InputsVR.RightHand.triggerButton.IsPressed) spawnedItem.UseContinue();
                else if (InputsVR.RightHand.triggerButton.WasReleased) spawnedItem.UseEnd();

                if(!spawnedItem.StillUsing())
                {
                    holdingWeapon = false;
                    attacking = false;

                    Game.Player.UnequipWeapon();
                    LevelFlow.SetTurnPart(LevelFlow.TurnPart.turnStart);
                    SetAsNotPlayer();
                    regularModeObject.SetActive(true);
                }
            }
        }
        controller.Move(moveVector * Time.deltaTime);
    }

    public void SetChoice(bool on)
    {
        choice.gameObject.SetActive(true);
        if (on) choice.TurnOn();
        else choice.TurnOff();
    }

    public void HideChoice()
    {
        choice.gameObject.SetActive(false);
        choice.TurnOff();
    }

    public void ChooseCharacter()
    {
        regularModeObject.SetActive(false);
        SetAsPlayer();
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.weaponChoice);

        canvas.gameObject.SetActive(true);
        for(int j = weaponSelectionIcons.Count - 1; j >= 0; j--)
        {
            Destroy(weaponSelectionIcons[j]);
        }
        weaponSelectionIcons.Clear();
        foreach(var weapon in Database.WeaponPrefabs)
        {
            WeaponSelectionIcon icon = Instantiate(weaponSelectionButtonPrefab, 
                weaponSelectionGrid).GetComponent<WeaponSelectionIcon>();
            icon.weaponPrefab = weapon;
            icon.soldier = this;
            Item item = weapon.GetComponent<Item>();
            icon.iconImage.sprite = item.icon;
            icon.nameText.text = item.itemName;
            weaponSelectionIcons.Add(icon.gameObject);
        }
    }

    public void ChooseWeapon(WeaponSelectionIcon icon)
    {
        holdingWeapon = true;
        canvas.gameObject.SetActive(false);

        GameObject weapon = Instantiate(icon.weaponPrefab);
        spawnedItem = weapon.GetComponent<Item>();
        spawnedItem.Set(playerNumber);

        Game.Player.EquipWeapon(weapon);

        LevelFlow.SetTurnPart(LevelFlow.TurnPart.movement);
    }

    public void GetDamage(int power)
    {
        SetHealth(Mathf.Clamp(currentHealth - power, 0, currentHealth));
    }

    private void SetHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        if (currentHealth == 0) Die();
        infoWindow.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void Die()
    {
        LevelFlow.NotifyOfDeath(this);
        Destroy(gameObject);
    }

    public void SetInfoWindowSize(bool small)
    {
        infoWindow.transform.parent.localScale = (small ? .25f : 1f) * startInfoWindowScale;
    }
}
