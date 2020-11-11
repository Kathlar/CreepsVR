using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDie : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterSoldier soldier))
            soldier.Die();
        else
            Destroy(other.gameObject);
    }
}
