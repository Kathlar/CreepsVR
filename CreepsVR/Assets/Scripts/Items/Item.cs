using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public AudioSource audioSource { get; private set; }
    protected MeshRenderer[] meshRenderers { get; private set; }
    protected Collider[] colliders { get; private set; }
    protected RaycastLineForward line;

    public string itemName = "Item";
    [TextArea]
    public string description;
    public Sprite icon;

    public CharacterSoldier holder { get; private set; }

    public bool turnedOn { get; private set; } = false;
    public bool isUsed { get; private set; } = false;

    public bool twoHanded;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
        line = GetComponentInChildren<RaycastLineForward>();
    }

    private void Start()
    {
        if (line) line.gameObject.SetActive(false);
        foreach(MeshRenderer mesh in meshRenderers)
        {
            Color c = mesh.material.color;
            c.a = .1f;
            mesh.material.color = c;
        }
    }

    public void Set(CharacterSoldier soldier)
    {
        holder = soldier;
    }

    public void TurnOn()
    {
        if (line) line.gameObject.SetActive(true);
        foreach (MeshRenderer mesh in meshRenderers)
        {
            Color c = mesh.material.color;
            c.a = 1f;
            mesh.material.color = c;
        }
        turnedOn = true;
    }

    public virtual void UseStart()
    {
        isUsed = true;
    }

    public virtual void UseContinue()
    {

    }

    public virtual void UseEnd()
    {
        isUsed = true;
    }

    public abstract bool StillUsing();
}
