using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private AudioController _audio;
    private int nombreObjetsRamasses = 0;

    void OnTriggerEnter(Collider other) 
    {

        if (other.gameObject.CompareTag("PickUp")) 
        {   
            this._audio.PlayPickUpSFX();
            Destroy(other.gameObject);
            nombreObjetsRamasses++;
        }
    }

    public int GetNombreObjetsRamasses()
    {
        return nombreObjetsRamasses;
    }
}