using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDamageable
{
    private MeshRenderer meshRenderer;

    public int maxDamageDurability = 10;
    public bool destroyPartsAfterTime;

    protected List<DestructablePart> parts = new List<DestructablePart>();

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        foreach (var part in GetComponentsInChildren<DestructablePart>(true))
        {
            parts.Add(part);
            part.Set(meshRenderer.material);
        }
    }

    public void GetDamage(int power, Vector3 hitPoint, Vector3 damageVelocity)
    {
        if(power > maxDamageDurability)
        {
            foreach (DestructablePart part in parts)
            {
                part.gameObject.SetActive(true);
                part.rb.AddExplosionForce(damageVelocity.magnitude, hitPoint, 1);
                part.StartCoroutine(part.OnDestruction(destroyPartsAfterTime));
            }

            foreach (DestructablePart part in parts)

            Destroy(gameObject);
        }
    }
}
