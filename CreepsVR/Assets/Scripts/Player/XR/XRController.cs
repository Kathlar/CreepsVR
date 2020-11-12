using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRController : MonoBehaviour
{
    public LineRenderer raycastLine { get; private set; }
    public SkinnedMeshRenderer meshRenderer { get; private set; }
    public Animator animator { get; private set; }

    public XRPlayer player { get; private set; }

    private Material regularMaterial;

    public HorizontalSide side;

    public Item equipedItem;

    private void Awake()
    {
        raycastLine = GetComponentInChildren<LineRenderer>();
        animator = GetComponentInChildren<Animator>();
        meshRenderer = animator.GetComponentInChildren<SkinnedMeshRenderer>();
        regularMaterial = meshRenderer.material;
    }

    public void Set(XRPlayer player)
    {
        this.player = player;
    }

    private void Update()
    {
        if(equipedItem)
        {
            HorizontalSide otherSide = side == HorizontalSide.left ? HorizontalSide.right : HorizontalSide.left;
            if (equipedItem.twoHanded) equipedItem.transform.LookAt(player.controllers[otherSide].transform.position);
        }
        else if(InputsVR.Hands.ContainsKey(side))
        {
            animator.SetFloat("Trigger", InputsVR.Hands[side].trigger.Value);
            animator.SetFloat("Grip", InputsVR.Hands[side].grip.Value);
        }
    }

    public void EquipItem(Item weapon)
    {
        animator.gameObject.SetActive(!weapon);
        weapon.transform.parent = transform;
        weapon.transform.ResetLocalTransform();
        equipedItem = weapon;
    }

    public void EquipAsSecond()
    {
        animator.gameObject.SetActive(false);
    }

    public void UnequipItem()
    {
        animator.gameObject.SetActive(true);
        if(equipedItem) Destroy(equipedItem.gameObject);
    }

    public void SetMaterial(Material m = null)
    {
        if (m == null) meshRenderer.material = regularMaterial;
        else meshRenderer.material = m;
    }
}
