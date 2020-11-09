using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public TrailRenderer trail { get; private set; }

    public Gun gun { get; private set; }

    public float moveSpeed = 10;
    public int damagePower = 20;

    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    public void Set(Gun gun)
    {
        this.gun = gun;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.TryGetComponent(out PlayerControllerCharacter character))
        //{
        //    if (character.playerNumber == gun.holderNumber) return;
        //    character.GetDamage(damagePower);
        //}
        gun.NotifyOfHit(this);
        if(trail)
        {
            trail.transform.SetParent(null);
            Destroy(trail.gameObject, trail.time);
        }
        Destroy(gameObject);
    }
}
