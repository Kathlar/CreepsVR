using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Throwable
{
    bool exploded = false;
    public float timeToExplode = 3;
    public float explosionRadius = 4;
    public int damagePower = 20;

    public ParticleSystem explosionEffect;

    public override void UseEnd()
    {
        base.UseEnd();
        StartCoroutine(ThrownCoroutine());
    }

    private IEnumerator ThrownCoroutine()
    {
        yield return new WaitForSeconds(timeToExplode);

        explosionEffect.gameObject.SetActive(true);
        explosionEffect.transform.SetParent(null);
        explosionEffect.Play();
        Destroy(explosionEffect.gameObject, explosionEffect.duration);

        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider objectInRadius in objectsInRadius)
        {
            if (objectInRadius.TryGetComponent(out IDamageable damageable))
                damageable.GetDamage(damagePower, transform.position, Vector3.zero);
            else if (objectInRadius.TryGetComponent(out Rigidbody hitRB))
                hitRB.AddExplosionForce(50, transform.position, explosionRadius);
        }

        exploded = true;
    }

    public override bool StillUsing()
    {
        return !exploded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
