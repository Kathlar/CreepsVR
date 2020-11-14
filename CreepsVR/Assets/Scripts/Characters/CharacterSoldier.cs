using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoldier : Character, IDamageable
{
    public CharacterController controller { get; private set; }

    public Animator animator { get; private set; }

    [Header("Soldier Info")]
    public GameObject regularModeObject;
    [HideInInspector] public PlayerInstance playerInstance;
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
    private float timeOfAttackingStart;

    [Header("Soldier Info Window")]
    public Transform soldierInfoPivot;
    public SoldierInfoWindow infoWindow { get; private set; }
    private Vector3 startInfoWindowScale;

    [Header("Velocity Info")]
    public Transform groundPoint;
    private float yVelocity;

    private bool endingTurn;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        infoWindow = GetComponentInChildren<SoldierInfoWindow>();
        startInfoWindowScale = soldierInfoPivot.transform.parent.localScale;

        choice = GetComponentInChildren<CharacterSoldierChoice>();
        choice.character = this;
        ShowChoiceObject(false);

        canvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        infoWindow.SetUp(playerInstance);
        SetHealth(maxHealth);
        if (characterMesh) characterMesh.material = playerInstance.information.polygonPrototypeMaterial;
    }

    private void Update()
    {
        Vector3 moveVector = Vector3.zero;
        if(isPlayer)
        {
            if(holdingWeapon || endingTurn)
            {
                Transform cameraTransform = Game.Player.mainCamera.transform;
                Vector2 moveInputValue = new Vector2(Mathf.Clamp(Inputs.MainHorizontal +
                    InputsVR.LeftHand.joystick.Value.x, -1, 1), Mathf.Clamp(Inputs.MainVertical +
                    InputsVR.LeftHand.joystick.Value.y, -1, 1));
                Vector3 walkVector = cameraTransform.forward.FlatY() * moveInputValue.y +
                    cameraTransform.right.FlatY() * moveInputValue.x;
                moveVector += walkVector * moveSpeed * (attacking ? .4f : 1);
            }

            if(holdingWeapon && !attacking)
            {
                if (InputsVR.RightHand.triggerButton.WasPressed || Inputs.LeftMouse.WasPressed)
                    StartAttackMode();
            }

            if(holdingWeapon && attacking && Time.timeSinceLevelLoad > timeOfAttackingStart + .5f)
            {
                if (InputsVR.RightHand.triggerButton.WasPressed || Inputs.LeftMouse.WasPressed) spawnedItem.UseStart();
                else if (InputsVR.RightHand.triggerButton.IsPressed || Inputs.LeftMouse.IsPressed) spawnedItem.UseContinue();
                else if (InputsVR.RightHand.triggerButton.WasReleased || Inputs.LeftMouse.WasReleased) spawnedItem.UseEnd();

                if (!spawnedItem.StillUsing())
                    EndTurn();
            }
        }

        bool isGrounded = Physics.CheckSphere(groundPoint.position, .2f, Database.Layers.walkableLayers);
        if (isGrounded) yVelocity = -1;
        else yVelocity = Mathf.Lerp(yVelocity, Physics.gravity.y, Time.deltaTime * 3);
        moveVector += new Vector3(0, yVelocity, 0);

        controller.Move(moveVector * Time.deltaTime);
    }

    public void ShowChoiceObject(bool clickable)
    {
        choice.gameObject.SetActive(true); 
        choice.SetState(clickable);
    }

    public void HideChoiceObject()
    {
        choice.gameObject.SetActive(false);
        choice.SetState(false);
    }

    public void GetChosen()
    {
        endingTurn = attacking = holdingWeapon = false;
        regularModeObject.SetActive(false);
        SetAsPlayer();
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.soldierWeaponChoice);

        canvas.gameObject.SetActive(true);
        for(int j = weaponSelectionIcons.Count - 1; j >= 0; j--)
            Destroy(weaponSelectionIcons[j]);

        weaponSelectionIcons.Clear();
        soldierInfoPivot.gameObject.SetActive(false);

        foreach(var weapon in playerInstance.weaponInformations.weapons)
        {
            WeaponSelectionIcon icon = Instantiate(weaponSelectionButtonPrefab, weaponSelectionGrid).GetComponent<WeaponSelectionIcon>();
            Item item = weapon.weaponPrefab.GetComponent<Item>();

            icon.weapon = weapon;
            icon.soldier = this;
            icon.iconImage.sprite = item.icon;
            icon.nameText.text = item.itemName;
            icon.weaponCountText.text = weapon.usagesForStart.ToString();
            icon.uIInteractable.tooltipText.text = item.description;
            if (weapon.usagesForStart <= 0)
            {
                icon.bgImage.color = Color.grey;
                icon.button.enabled = false;
            }
            weaponSelectionIcons.Add(icon.gameObject);
        }
    }

    public void ChooseWeapon(WeaponSelectionIcon icon)
    {
        holdingWeapon = true;
        canvas.gameObject.SetActive(false);

        GameObject weapon = Instantiate(icon.weapon.weaponPrefab);
        spawnedItem = weapon.GetComponent<Item>();
        spawnedItem.Set(this);

        Game.Player.EquipItem(spawnedItem);

        LevelFlow.SetTurnPart(LevelFlow.TurnPart.soldierMovement);
        if (LevelFlow.levelSetupInfo.timerGame)
        {
            Game.Player.timer.SetTimer(10);
            StartCoroutine(ChooseWeaponCoroutine(10));
        }
        else StartCoroutine(ChooseWeaponCoroutine(Mathf.Infinity));
    }

    private IEnumerator ChooseWeaponCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (!attacking && !endingTurn) StartAttackMode();
    }

    public void StartAttackMode()
    {
        attacking = true;
        timeOfAttackingStart = Time.timeSinceLevelLoad;
        spawnedItem.TurnOn();
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.soliderAttack);

        if (LevelFlow.levelSetupInfo.timerGame)
        {
            Game.Player.timer.SetTimer(7);
            StartCoroutine(StartAttackModeCoroutine(7));
        }
        else StartCoroutine(StartAttackModeCoroutine(Mathf.Infinity));
    }

    private IEnumerator StartAttackModeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (!endingTurn) EndTurn();
    }

    public void GetDamage(int power)
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
        if (isPlayer) DoEndTurn();
        LevelFlow.NotifyOfSoldierDeath(this);
        animator.SetTrigger("DieTrigger");
        Destroy(gameObject);
    }

    public void SetInfoWindowSize(bool small)
    {
        soldierInfoPivot.transform.localScale = (small ? .25f : 1f) * startInfoWindowScale;
    }

    public void EndTurn()
    {
        endingTurn = true;
        attacking = false;
        holdingWeapon = false;

        Game.Player.timer.TurnOffTimer();
        Game.Player.timer.SetTimer(3);
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.soldierFinish);
        Game.Player.UnequipItem();

        StartCoroutine(EndTurnCoroutine());
    }

    private IEnumerator EndTurnCoroutine()
    {
        yield return new WaitForSeconds(3f);
        DoEndTurn();
    }

    private void DoEndTurn()
    {
        Game.Player.timer.TurnOffTimer();
        soldierInfoPivot.gameObject.SetActive(true);

        Vector3 lookAtVec = transform.position + Game.Player.mainCamera.transform.forward;
        lookAtVec.y = transform.position.y;
        transform.LookAt(lookAtVec);

        LevelFlow.SetTurnPart(LevelFlow.TurnPart.turnStart);
        SetAsNotPlayer();
        regularModeObject.SetActive(true);
    }
}
