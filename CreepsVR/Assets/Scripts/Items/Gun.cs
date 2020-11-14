using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public List<Transform> shootPoints = new List<Transform>();
    public int numberOfBullets = 1;
    public float timeBetweenShots = 0;
    private float timeOfLastShot = -100;
    public GameObject bulletPrefab;
    protected List<Projectile> projectiles = new List<Projectile>();

    public ParticleSystem shootEffect;

    public override void UseContinue()
    {
        base.UseContinue();
        if (numberOfBullets > 0 && Time.timeSinceLevelLoad > timeOfLastShot + timeBetweenShots)
        {
            timeOfLastShot = Time.timeSinceLevelLoad;
            numberOfBullets--;
            if (audioSource) audioSource.PlayOneShot(audioSource.clip);
            if (shootEffect) shootEffect.Play();
            foreach(Transform shootPoint in shootPoints)
            {
                Projectile bullet = Instantiate(bulletPrefab, shootPoint.position,
                    shootPoint.rotation).GetComponent<Projectile>();
                bullet.Set(this);
                projectiles.Add(bullet);

                foreach (Collider col in colliders)
                    Physics.IgnoreCollision(col, bullet.mainCollider);
            }
        }
    }

    public void NotifyOfHit(Projectile projectile)
    {
        projectiles.Remove(projectile);
    }

    public override bool StillUsing()
    {
        return numberOfBullets > 0 || projectiles.Count > 0;
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        if(shootEffect)
        {
            shootEffect.transform.SetParent(null);
            Destroy(shootEffect.gameObject, shootEffect.main.duration);
        }
    }
}
