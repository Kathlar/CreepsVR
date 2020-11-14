using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public GameObject gfxObject;
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
        Destroy(gfxObject);

        if (audioSource)
        {
            audioSource.transform.SetParent(null);
            audioSource.Play();
            Destroy(audioSource, audioSource.clip.length);
        }
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.transform.SetParent(null);
        explosionEffect.Play();
        Destroy(explosionEffect.gameObject, explosionEffect.main.duration);

        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider objectInRadius in objectsInRadius)
        {
            if (LevelFlow.levelSetupInfo.destructableGame && objectInRadius.TryGetComponent(out MeshDestroy meshDestroy))
                meshDestroy.DestroyMesh();
            if (objectInRadius.TryGetComponent(out IDamageable damageable))
                damageable.GetDamage(damagePower);
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
