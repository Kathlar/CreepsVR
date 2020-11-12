using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float _maxLifeTime = 3;

    public Collider mainCollider { get; private set; }
    public Explosive explose { get; private set; }

    public ParticleSystem hitEffectParticle;
    public GameObject trail;

    public Gun gun { get; private set; }

    public float moveSpeed = 10;
    public int damagePower = 20;

    bool hit;

    protected virtual void Awake()
    {
        mainCollider = GetComponentInChildren<Collider>();
        explose = GetComponent<Explosive>();
    }

    private void Start()
    {
        Invoke("OnHitEffect", _maxLifeTime);
    }

    public void Set(Gun gun)
    {
        this.gun = gun;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (hit) return;
        hit = true;
        bool shouldExplode = false;
        if(explose)
            shouldExplode = true;
        else
        {
            if (LevelFlow.DestructableGame && other.transform.TryGetComponent(out MeshDestroy meshDestroy))
                meshDestroy.DestroyMesh();
            if (hitEffectParticle)
            {
                hitEffectParticle.Play();
                hitEffectParticle.transform.SetParent(null);
                Destroy(hitEffectParticle.gameObject, hitEffectParticle.main.duration);
            }
            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                if ((Object)damageable == gun.holder) return;
                damageable.GetDamage(damagePower);
            }
            else if (other.transform.TryGetComponent(out Rigidbody hitRigid))
                hitRigid.AddForce(transform.forward * moveSpeed);
        }

        if (trail)
        {
            trail.transform.SetParent(null);
            if (trail.TryGetComponent(out TrailRenderer trailTrail))
                Destroy(trail.gameObject, trailTrail.time);
            else if (trail.TryGetComponent(out ParticleSystem trailParticle))
                Destroy(trail.gameObject, trailParticle.main.duration);
        }
        if(shouldExplode) explose.Explode();
        else OnHitEffect();
    }

    private void OnHitEffect()
    {
        gun.NotifyOfHit(this);
        Destroy(gameObject);
    }
}
