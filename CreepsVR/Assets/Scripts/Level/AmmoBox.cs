using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public AudioSource audioSource { get; private set; }

    public Item itemPrefab;
    public int ammoValue = 1;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterSoldier soldier))
        {
            soldier.playerInstance.weaponInformations.weapons.Find(x => x.weaponPrefab.name == itemPrefab.name).
                usagesForStart += ammoValue;
            if(audioSource)
            {
                audioSource.transform.parent = null;
                audioSource.Play();
                Destroy(audioSource.gameObject, audioSource.clip.length);
            }
            Destroy(gameObject);
        }
    }
}
