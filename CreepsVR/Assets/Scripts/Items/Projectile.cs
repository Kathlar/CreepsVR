using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float _maxLifeTime = 3;

    public TrailRenderer trail { get; private set; }

    public Gun gun { get; private set; }

    public float moveSpeed = 10;
    public int damagePower = 20;

    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
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
        if (other.transform.TryGetComponent(out IDamageable damageable))
        {
            if (damageable == gun.holder) return;
            damageable.GetDamage(damagePower, other.contacts[0].point, transform.forward * moveSpeed);
        }
        else if (other.transform.TryGetComponent(out Rigidbody hitRigid))
            hitRigid.AddForce(transform.forward * moveSpeed);

        if (trail)
        {
            trail.transform.SetParent(null);
            Destroy(trail.gameObject, trail.time);
        }
        OnHitEffect();
    }

    private void OnHitEffect()
    {
        gun.NotifyOfHit(this);
        Destroy(gameObject);
    }
}
