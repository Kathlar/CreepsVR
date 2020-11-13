using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionIcon : MonoBehaviour
{
    public UIButton uIInteractable { get; private set; }
    public Image bgImage { get; private set; }
    public Button button { get; private set; }
    public Image iconImage;
    public Text nameText;
    public Text weaponCountText;

    [HideInInspector] public WeaponInformation weapon;
    [HideInInspector] public CharacterSoldier soldier;

    private void Awake()
    {
        uIInteractable = GetComponent<UIButton>();
        bgImage = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void Button_Select()
    {
        if (weapon.usagesForStart > 0)
        {
            weapon.usagesForStart--;
            soldier.ChooseWeapon(this);
        }
    }
}
