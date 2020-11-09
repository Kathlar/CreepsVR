using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRController : MonoBehaviour
{
    public LineRenderer raycastLine { get; private set; }
    public Animator animator { get; private set; }

    public HorizontalSide side;

    public GameObject equipedWeapon;

    private void Awake()
    {
        raycastLine = GetComponentInChildren<LineRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(InputsVR.Hands.ContainsKey(side))
        {
            animator.SetFloat("Trigger", InputsVR.Hands[side].trigger.Value);
            animator.SetFloat("Grip", InputsVR.Hands[side].grip.Value);
        }
    }

    public void EquipWeapon(GameObject weapon)
    {
        animator.gameObject.SetActive(!weapon);
        weapon.transform.parent = transform;
        weapon.transform.ResetLocalTransform();
        equipedWeapon = weapon;
    }

    public void UnequipWeapon()
    {
        animator.gameObject.SetActive(true);
        Destroy(equipedWeapon);
    }
}
