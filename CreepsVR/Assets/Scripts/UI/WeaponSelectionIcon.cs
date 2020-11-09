using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionIcon : MonoBehaviour
{
    [HideInInspector] public GameObject weaponPrefab;
    [HideInInspector] public CharacterSoldier soldier;

    public Image iconImage;
    public Text nameText;

    public void Button_Select()
    {
        soldier.ChooseWeapon(this);
    }
}
