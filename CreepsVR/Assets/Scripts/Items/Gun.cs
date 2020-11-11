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

    public override void UseContinue()
    {
        base.UseContinue();
        if (numberOfBullets > 0 && Time.timeSinceLevelLoad > timeOfLastShot + timeBetweenShots)
        {
            numberOfBullets--;
            if (audioSource) audioSource.PlayOneShot(audioSource.clip);
            foreach(Transform shootPoint in shootPoints)
            {
                Projectile bullet = Instantiate(bulletPrefab, shootPoint.position,
                    shootPoint.rotation).GetComponent<Projectile>();
                bullet.Set(this);
                projectiles.Add(bullet);
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
}
