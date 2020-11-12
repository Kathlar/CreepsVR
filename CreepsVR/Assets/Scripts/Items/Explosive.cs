using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public AudioSource audioSource;

    public bool exploded { get; private set; }

    public float timeToExplode = 3;
    public float explosionRadius = 4;
    public int damagePower = 20;

    public ParticleSystem explosionEffect;

    public void Explode()
    {
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(timeToExplode);

        if(audioSource)
        {
            audioSource.transform.SetParent(null);
            audioSource.Play();
            Destroy(audioSource, audioSource.clip.length);
        }
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.transform.SetParent(null);
        explosionEffect.Play();
        Destroy(explosionEffect.gameObject, explosionEffect.duration);

        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider objectInRadius in objectsInRadius)
        {
            if (objectInRadius.TryGetComponent(out IDamageable damageable))
                damageable.GetDamage(damagePower, transform.position, Vector3.zero);
            else if (objectInRadius.TryGetComponent(out Rigidbody hitRB))
                hitRB.AddExplosionForce(50, transform.position, explosionRadius);
        }

        exploded = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
