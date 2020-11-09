using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public MeshRenderer[] meshRenderers { get; private set; }

    public string itemName = "Item";
    public Sprite icon;

    public int holderNumber { get; private set; }

    public bool turnedOn { get; private set; } = false;
    public bool isUsed { get; private set; } = false;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void Start()
    {
        foreach(MeshRenderer mesh in meshRenderers)
        {
            Color c = mesh.material.color;
            c.a = .1f;
            mesh.material.color = c;
        }
    }

    public void Set(int playerNumber)
    {
        holderNumber = playerNumber;
    }

    public void TurnOn()
    {
        foreach (MeshRenderer mesh in meshRenderers)
        {
            Color c = mesh.material.color;
            c.a = 1f;
            mesh.material.color = c;
        }
        turnedOn = true;
    }

    public abstract bool StillUsing();

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
}
