using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoldier : Character, IDamageable
{
    public CharacterController controller { get; private set; }

    public Animator animator { get; private set; }

    [Header("Soldier Info")]
    public GameObject regularModeObject;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public CharacterSoldierChoice choice;
    public SkinnedMeshRenderer characterMesh;

    public int maxHealth = 100;
    private int currentHealth;
    private float timeOfLastDamage = -10, timeOfLastDamageAnimation = -10;

    [Header("Weapon Selection")]
    public RectTransform weaponSelectionGrid;
    public GameObject weaponSelectionButtonPrefab;
    private List<GameObject> weaponSelectionIcons = new List<GameObject>();

    private Item spawnedItem;
    private bool holdingWeapon, attacking;

    [Header("Soldier Info Window")]
    public Transform soldierInfoPivot;
    public SoldierInfoWindow infoWindow { get; private set; }
    private Vector3 startInfoWindowScale;

    [Header("Velocity Info")]
    public Transform groundPoint;
    private float yVelocity;
    public LayerMask groundLayer;

    private bool endingTurn;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        infoWindow = GetComponentInChildren<SoldierInfoWindow>();
        startInfoWindowScale = soldierInfoPivot.transform.parent.localScale;

        choice = GetComponentInChildren<CharacterSoldierChoice>();
        choice.character = this;
        SetChoice(false);

        canvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        infoWindow.SetUp(playerNumber);
        SetHealth(maxHealth);
        if (characterMesh) characterMesh.material = Database.PlayerInfos[playerNumber].polygonPrototypeMaterial;
    }

    private void Update()
    {
        Vector3 moveVector = Vector3.zero;
        if (!endingTurn && isPlayer && holdingWeapon)
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

                if(InputsVR.LeftHand.triggerButton.WasPressed || Inputs.RightMouse.WasPressed)
                {
                    StartAttackMode();
                }
            }
            else
            {
                if (InputsVR.RightHand.triggerButton.WasPressed || Inputs.LeftMouse.WasPressed) spawnedItem.UseStart();
                else if (InputsVR.RightHand.triggerButton.IsPressed || Inputs.LeftMouse.IsPressed) spawnedItem.UseContinue();
                else if (InputsVR.RightHand.triggerButton.WasReleased || Inputs.LeftMouse.WasReleased) spawnedItem.UseEnd();

                if(!spawnedItem.StillUsing())
                {
                    EndTurn();
                }
            }
        }
        bool isGrounded = Physics.CheckSphere(groundPoint.position, .2f, groundLayer);
        if (isGrounded) yVelocity = -1;
        else yVelocity = Mathf.Lerp(yVelocity, Physics.gravity.y, Time.deltaTime * 3);
        moveVector += new Vector3(0, yVelocity, 0);
        controller.Move(moveVector * Time.deltaTime);
    }

    public void EndTurn()
    {
        endingTurn = true;
        holdingWeapon = false;
        attacking = false;

        StartCoroutine(EndTurnCoroutine());
    }

    private IEnumerator EndTurnCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Game.Player.UnequipItem();
        soldierInfoPivot.gameObject.SetActive(true);
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.turnStart);
        SetAsNotPlayer();
        regularModeObject.SetActive(true);
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

    public void GetChosen()
    {
        endingTurn = false;
        regularModeObject.SetActive(false);
        SetAsPlayer();
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.soldierWeaponChoice);
        LevelFlow.SetCurrentSoldier(this);

        canvas.gameObject.SetActive(true);
        for(int j = weaponSelectionIcons.Count - 1; j >= 0; j--)
        {
            Destroy(weaponSelectionIcons[j]);
        }
        weaponSelectionIcons.Clear();
        soldierInfoPivot.gameObject.SetActive(false);
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
        spawnedItem.Set(this);

        Game.Player.EquipItem(spawnedItem);

        LevelFlow.SetTurnPart(LevelFlow.TurnPart.soldierMovement);
    }

    public void StartAttackMode()
    {
        attacking = true;
        spawnedItem.TurnOn();
        Game.Player.timer.TurnOffTimer();
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.soliderAttack);
        Game.Player.timer.SetTimer(5);
    }

    public void GetDamage(int power, Vector3 hitPoint, Vector3 damageVelocity)
    {
        timeOfLastDamage = Time.timeSinceLevelLoad;
        if(Time.timeSinceLevelLoad > timeOfLastDamageAnimation + 1)
        {
            timeOfLastDamageAnimation = Time.timeSinceLevelLoad;
            animator.SetInteger("Action", Random.Range(1, 5));
            animator.SetTrigger("HitTrigger");
        }
        SetHealth(Mathf.Clamp(currentHealth - power, 0, currentHealth));
    }

    private void SetHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        if (currentHealth == 0) Die();
        infoWindow.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void Die()
    {
        LevelFlow.NotifyOfDeath(this);
        animator.SetTrigger("DieTrigger");
        Destroy(gameObject);
    }

    public void SetInfoWindowSize(bool small)
    {
        soldierInfoPivot.transform.localScale = (small ? .25f : 1f) * startInfoWindowScale;
    }
}
