using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablePart : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public MeshRenderer meshRenderer { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Set(Material material)
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    public IEnumerator OnDestruction(bool destroyAfterTime = true)
    {
        transform.SetParent(null);
        yield return new WaitForSeconds(2);
        if(destroyAfterTime) Destroy(gameObject);
    }
}
