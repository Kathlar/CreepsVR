using UnityEngine;

public interface IDamageable
{
    void GetDamage(int power, Vector3 hitPoint, Vector3 damageVelocity);
}
